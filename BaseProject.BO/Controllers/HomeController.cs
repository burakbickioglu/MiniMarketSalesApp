namespace BaseProject.BO.Controllers;

[Authorize(Roles = "Admin")]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]

public class HomeController : Controller
{
    private readonly HttpClient _client;
    private readonly ICurrentUserService _currentUserService;

    public HomeController(HttpClient httpClient, ICurrentUserService currentUserService)
    {
        _client = httpClient;
        var token = $"Bearer {currentUserService.GetToken()}";
        _currentUserService = currentUserService;
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {currentUserService.GetToken()}");
    }

    public async Task<IActionResult> Index()
    {
        var lastFiveUsersResponse = await _client.GetAsync("Admin/GetLastFiveUsers");
        var lastFiveUsersData = await lastFiveUsersResponse.ReadContentAs<List<UserDTO>>();
        
        DashboardViewModel model = new DashboardViewModel
        {
            LastFiveUser = lastFiveUsersData.Data,
        };
        return View(model);
    }

    [HttpPost]
    public async Task<JsonResult> GetUserStatistics()
    {
        var userResponse = await _client.GetAsync("Admin/GetUserStatistics");
        var userData = await userResponse.ReadContentAs<UserStatisticsDTO>();
        return Json(userData.Data);
    }

 

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult ExceptError()
    {
        Convert.ToInt32("asd");
        return View();
    }

    [HttpPost]
    public IActionResult CultureManagement(string culture, string returnUrl)
    {
        Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)), new CookieOptions { Expires = DateTimeOffset.Now.AddDays(30) });

        return LocalRedirect(returnUrl);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [AllowAnonymous]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}