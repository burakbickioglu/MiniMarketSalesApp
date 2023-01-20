namespace BaseProject.BO.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly JwtSettings _settings;
        private readonly ICurrentUserService _currentUserService;

        public AccountController(HttpClient httpClient, JwtSettings settings, ICurrentUserService currentUserService)
        {
            _httpClient = httpClient;
            _settings = settings;
            _currentUserService = currentUserService;
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequestDTO model)
        {
            var response = await _httpClient.PostAsJsonAsync("auth/login", model);
            if (!response.IsSuccessStatusCode)
                return View();
            var responseBody = await response.ReadContentAs<AuthResponseDTO>();
            
            if (!ModelState.IsValid)
                return View(model);


            if (responseBody.IsSucceed && responseBody?.Data?.Token is not null)
            {

                var claimsPrincipal = JwtHelper.GetPrincipalFromToken(_settings, responseBody.Data.Token);
                var claims = claimsPrincipal.Claims.ToList();

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1),
                    IssuedUtc = DateTimeOffset.UtcNow,
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return RedirectToAction("Index", "Products");
            }


            ModelState.AddModelError("error", responseBody?.Message ?? string.Empty);
            return View(model);
        }

        public IActionResult Deneme()
        {
            return View();
        }
    }
}
