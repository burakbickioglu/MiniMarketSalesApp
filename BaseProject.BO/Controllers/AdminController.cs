namespace BaseProject.BO.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly HttpClient _client;
    private readonly ICurrentUserService _currentUserService;

    public AdminController(HttpClient httpClient, ICurrentUserService currentUserService)
    {
        _client = httpClient;
        var token = $"Bearer {currentUserService.GetToken()}";
        _currentUserService = currentUserService;
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {currentUserService.GetToken()}");
    }

    //[Route("Admin/UserList/{page=1}")]
    public async Task<IActionResult> UserList()
    {
        //var content = new ContactMailViewModel { UserName = "burak", Key = "WelcomeCustomer", Email = "burak.bickioglu@nextlua.com", Subject = "Deneme Başlığı" };
        //var mailresponse = await _client.PostAsJsonAsync($"Mail/SendMail", content);

        return View();
    }
    public async Task<IActionResult> UserList_Read([DataSourceRequest] DataSourceRequest request, string requestFilters)
    {
        request.Filters = null;
        var searchModel = new DataTableApiRequest
        {
            Request = request,
            RequestFilters = requestFilters,
        };
        var response = await _client.PostAsJsonAsync($"Admin/GetUsers", searchModel);
        var data = await response.ReadContentAsDataSource<UserListViewModel>(request);

        return Json(data.Data);
    }


    public async Task<IActionResult> RemoveUser(Guid id)
    {
        var response = await _client.DeleteAsync($"Admin/DeleteUser/{id}");
        return RedirectToAction(nameof(UserList));
    }

    public async Task<IActionResult> UpdateUser(Guid id)
    {
        var userResponse = await _client.GetAsync($"Admin/GetUser/{id}");
        var userData = await userResponse.ReadContentAs<UserDTO>();
        ViewBag.CountryList = GetCountries();

    

        var model = new UserDetailViewModel() { UserDTO = userData.Data };
       
        return View(model);


    }

    [HttpPost]
    public async Task<IActionResult> UpdateUser(UserDetailViewModel model)
    {
        var user = model.UserDTO;
        if (ModelState.IsValid)
        {
            var response = await _client.PostAsJsonAsync($"Admin/UpdateUser", user);
            var data = await response.ReadContentAs<UserDTO>();
            return RedirectToAction(nameof(UserList));
        }
        return View(user);
    }

    [Route("/Admin/AddUser"), HttpGet]
    public async Task<IActionResult> AddUser()
    {
        ViewBag.CountryList = GetCountries();

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddUser(AddUserDTO user)
    {
        if (ModelState.IsValid)
        {
            var response = await _client.PostAsJsonAsync($"Admin/AddUser", user);
            var data = await response.ReadContentAs<UserDTO>();
            return RedirectToAction(nameof(UserList));
        }
        return View(user);
    }

    public async Task<IActionResult> DeleteTest(Guid userId, Guid testId)
    {
        var response = await _client.DeleteAsync($"Test/DeleteTest/{testId}");
        return RedirectToAction(nameof(UpdateUser), new { id = userId });
    }

    public async Task<IActionResult> SendResetPassword(Guid id)
    {
        var userResponse = await _client.GetAsync($"Admin/GetUser/{id}");
        var userData = await userResponse.ReadContentAs<UserDTO>();
        var response = await _client.PostAsJsonAsync($"Auth/ForgotPassword", new ForgotPasswordDTO { Email = userData.Data.Email, CallBackUrl = "https://www.geciciurl.com/" });
        return RedirectToAction(nameof(UpdateUser), new { id = id });
    }
    public List<string> GetCountries()
    {
        //List<string> countryList = new List<string>();
        //CultureInfo[] CInfoList = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
        //foreach (CultureInfo CInfo in CInfoList)
        //{
        //    RegionInfo R = new RegionInfo(CInfo.LCID);
        //    if (!countryList.Contains(R.EnglishName))
        //    {
        //        countryList.Add(R.EnglishName);
        //    }
        //}
        string[] countriesArray = { "United States", "Canada", "Afghanistan", "Albania", "Algeria", "American Samoa", "Andorra", "Angola", "Anguilla", "Antarctica", "Antigua and/or Barbuda", "Argentina", "Armenia", "Aruba", "Australia", "Austria", "Azerbaijan", "Bahamas", "Bahrain", "Bangladesh", "Barbados", "Belarus", "Belgium", "Belize", "Benin", "Bermuda", "Bhutan", "Bolivia", "Bosnia and Herzegovina", "Botswana", "Bouvet Island", "Brazil", "British Indian Ocean Territory", "Brunei Darussalam", "Bulgaria", "Burkina Faso", "Burundi", "Cambodia", "Cameroon", "Cape Verde", "Cayman Islands", "Central African Republic", "Chad", "Chile", "China", "Christmas Island", "Cocos (Keeling) Islands", "Colombia", "Comoros", "Congo", "Cook Islands", "Costa Rica", "Croatia (Hrvatska)", "Cuba", "Cyprus", "Czech Republic", "Denmark", "Djibouti", "Dominica", "Dominican Republic", "East Timor", "Ecudaor", "Egypt", "El Salvador", "Equatorial Guinea", "Eritrea", "Estonia", "Ethiopia", "Falkland Islands (Malvinas)", "Faroe Islands", "Fiji", "Finland", "France", "France, Metropolitan", "French Guiana", "French Polynesia", "French Southern Territories", "Gabon", "Gambia", "Georgia", "Germany", "Ghana", "Gibraltar", "Greece", "Greenland", "Grenada", "Guadeloupe", "Guam", "Guatemala", "Guinea", "Guinea-Bissau", "Guyana", "Haiti", "Heard and Mc Donald Islands", "Honduras", "Hong Kong", "Hungary", "Iceland", "India", "Indonesia", "Iran (Islamic Republic of)", "Iraq", "Ireland", "Israel", "Italy", "Ivory Coast", "Jamaica", "Japan", "Jordan", "Kazakhstan", "Kenya", "Kiribati", "Korea, Democratic People's Republic of", "Korea, Republic of", "Kosovo", "Kuwait", "Kyrgyzstan", "Lao People's Democratic Republic", "Latvia", "Lebanon", "Lesotho", "Liberia", "Libyan Arab Jamahiriya", "Liechtenstein", "Lithuania", "Luxembourg", "Macau", "Macedonia", "Madagascar", "Malawi", "Malaysia", "Maldives", "Mali", "Malta", "Marshall Islands", "Martinique", "Mauritania", "Mauritius", "Mayotte", "Mexico", "Micronesia, Federated States of", "Moldova, Republic of", "Monaco", "Mongolia", "Montserrat", "Morocco", "Mozambique", "Myanmar", "Namibia", "Nauru", "Nepal", "Netherlands", "Netherlands Antilles", "New Caledonia", "New Zealand", "Nicaragua", "Niger", "Nigeria", "Niue", "Norfork Island", "Northern Mariana Islands", "Norway", "Oman", "Pakistan", "Palau", "Panama", "Papua New Guinea", "Paraguay", "Peru", "Philippines", "Pitcairn", "Poland", "Portugal", "Puerto Rico", "Qatar", "Reunion", "Romania", "Russian Federation", "Rwanda", "Saint Kitts and Nevis", "Saint Lucia", "Saint Vincent and the Grenadines", "Samoa", "San Marino", "Sao Tome and Principe", "Saudi Arabia", "Senegal", "Seychelles", "Sierra Leone", "Singapore", "Slovakia", "Slovenia", "Solomon Islands", "Somalia", "South Africa", "South Georgia South Sandwich Islands", "South Sudan", "Spain", "Sri Lanka", "St. Helena", "St. Pierre and Miquelon", "Sudan", "Suriname", "Svalbarn and Jan Mayen Islands", "Swaziland", "Sweden", "Switzerland", "Syrian Arab Republic", "Taiwan", "Tajikistan", "Tanzania, United Republic of", "Thailand", "Togo", "Tokelau", "Tonga", "Trinidad and Tobago", "Tunisia", "Turkey", "Turkmenistan", "Turks and Caicos Islands", "Tuvalu", "Uganda", "Ukraine", "United Arab Emirates", "United Kingdom", "United States minor outlying islands", "Uruguay", "Uzbekistan", "Vanuatu", "Vatican City State", "Venezuela", "Vietnam", "Virigan Islands (British)", "Virgin Islands (U.S.)", "Wallis and Futuna Islands", "Western Sahara", "Yemen", "Yugoslavia", "Zaire", "Zambia", "Zimbabwe" };
        List<string> countryList = new List<string>(countriesArray);

        countryList.Sort();
        return countryList;
    }
    //public async Task<JsonResult> UserList_Read(DataSourceRequest dataSourceRequest)
    //{
    //    var response = await _httpClient.GetAsync("Admin/GetUsers");
    //    var data = await response.ReadContentAs<List<UserDTO>>();
    //  return Json(data.Data.ToDataSourceResult(new DataSourceRequest()));
    //}

    //public async Task<JsonResult> UserList_Read(DataSourceRequest dataSourceRequest)
    //{
    //    var response = await _httpClient.GetAsync("Admin/GetUsers");
    //    var data = await response.ReadContentAs<List<UserDTO>>();
    //  return Json(data.Data.ToDataSourceResult(new DataSourceRequest()));
    //}
}
