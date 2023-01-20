namespace BaseProject.Service.Services.Authentication.Commands;

public class ForgotPasswordCommand : CommandBase<CommandResult>
{
    public ForgotPasswordDTO Model { get; set; }

    public ForgotPasswordCommand(ForgotPasswordDTO model)
    {
        Model = model;
    }

    public class Handler : IRequestHandler<ForgotPasswordCommand, CommandResult>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly JwtSettings _settings;
        //public readonly IMailService _emailSender;
        private readonly IGenericRepository<GeneralContent> _generalContentRepository;

        public Handler(UserManager<AppUser> userManager, JwtSettings settings,/* IMailService emailSender*/ IGenericRepository<GeneralContent> generalContentRepository)
        {
            _userManager = userManager;
            _settings = settings;
            //_emailSender = emailSender;
            _generalContentRepository = generalContentRepository;
        }

        public async Task<CommandResult> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Model.Email))
                return CommandResult.GetFailed(ErrorMessageConstants.EMPTY_MAIL);

            var user = await _userManager.FindByEmailAsync(request.Model.Email);
            //if(user == null)
            //{ // şifre gönderilemese bile mesaj şifre gönderilmiş gibi gönderilmeli, daha sonra istenebilir bundan dolayı yorum bıraktım
            //    return CommandResult.GetFailed("Şifre yenileme bağlantınız e-mail adresinize gönderildi.");
            //}

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var passwordResetToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var claims = new List<Claim>
            {
                new Claim("token", passwordResetToken),
                new Claim(ClaimTypes.Email, user.Email),
            };
            var jwtToken = JwtHelper.CreateJwtToken(_settings, Audiences.Public, DateTime.UtcNow.AddDays(1), claims);
            var mail = await _generalContentRepository.GetFirst(p => p.Key == "ResetPassword");
            var Key = request.Model.CallBackUrl + "?token=" + jwtToken;
            var mailContent = mail.Value.Replace("??Key??", Key).Replace("??Email??", user.Email);

            var layout = await _generalContentRepository.GetFirst(p => p.Key == "Layout");

            layout.Value = layout.Value.Replace("??Content??", mailContent);

            ContactMailViewModel contactMailViewModel = new ContactMailViewModel()
            {
                UserName = user.UserName,
                Content = layout.Value,
                Email = user.Email,
                Subject = "BaseProject Yeni Şifre Belirleme"
            };

            //var mail2 =  new MailActivation("ResetPassword", "Layout", user.Email, user.Name).ToNotificationSenderModel(_generalContentRepository.GetAll());

            //var emailResult = await _emailSender.SendMailToUser(contactMailViewModel);
            return CommandResult.GetSucceed();

        }
    }
}
