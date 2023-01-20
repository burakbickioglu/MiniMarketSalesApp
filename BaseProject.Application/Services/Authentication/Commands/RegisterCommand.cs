namespace BaseProject.Application.Services.Authentication.Commands;

public class RegisterCommand : CommandBase<CommandResult<AuthResponseDTO>>
{
    public RegisterRequestDTO Model { get; set; }

    public RegisterCommand(RegisterRequestDTO model)
    {
        Model = model;
    }

    public class Handler : IRequestHandler<RegisterCommand, CommandResult<AuthResponseDTO>>
    {
        private readonly UserManager<AppUser> _userManager;
        protected readonly IMediator _mediator;
        //public readonly IMailService _emailSender;
        public IConfiguration _configuration;
        private readonly IGenericRepository<GeneralContent> _generalContentRepository;

        public Handler(UserManager<AppUser> userManager, IMediator mediator,/* IMailService emailSender*/ IConfiguration configuration, IGenericRepository<GeneralContent> generalContentRepository)
        {
            _userManager = userManager;
            _mediator = mediator;
            //_emailSender = emailSender;
            _configuration = configuration;
            _generalContentRepository = generalContentRepository;
        }

        public async Task<CommandResult<AuthResponseDTO>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (request.Model.Captcha != null)
                if (!await GoogleRecaptchaHelper.IsReCaptchaPassedAsync(request.Model.Captcha, _configuration["GoogleRecaptcha:RecaptchaV3SecretKey"]))
                    return CommandResult<AuthResponseDTO>.GetFailed(ErrorMessageConstants.CAPTCHA_ERROR); // true dönüyor ?

            var User = await _userManager.FindByEmailAsync(request.Model.Email);
            if (User != null)
                return CommandResult<AuthResponseDTO>.GetFailed(ErrorMessageConstants.INVALID_MAIL_OR_PASSWORD);

            AppUser createUser = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = request.Model.Email,
                Email = request.Model.Email,
                CreatedAt = DateTimeOffset.UtcNow
            };
            IdentityResult result = await _userManager.CreateAsync(createUser, request.Model.Password);
            var roleResult = await _userManager.AddToRoleAsync(createUser, Roles.Admin.ToString());

            if (result.Succeeded)
            {
                //var mail = await _generalContentRepository.GetFirst(p => p.Key == "WelcomeCustomer");
                //var mailContent = mail.Value.Replace("??Email??", createUser.Email);
                //var layout = await _generalContentRepository.GetFirst(p => p.Key == "Layout");
                //layout.Value = layout.Value.Replace("??Content??", mailContent);
                //ContactMailViewModel contactMailViewModel = new ContactMailViewModel
                //{
                //    Email = createUser.Email,
                //    Subject = "Hoş Geldiniz",
                //    Content = layout.Value,
                //    UserName = createUser.UserName
                //};
                //var emailResult = await _emailSender.SendMailToUser(contactMailViewModel);
                return await _mediator.Send(new LoginCommand(new LoginRequestDTO(createUser.Email, request.Model.Password)));
            }

            var errorMessages = "";
            foreach (var error in result.Errors)
            {
                errorMessages += $"{error.Code} - {error.Description}";
            }
            return CommandResult<AuthResponseDTO>.GetFailed(errorMessages);
        }
    }
}
