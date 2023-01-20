//namespace BaseProject.Service.Services.Admin.Commands;

//public class AddUserCommand : CommandBase<CommandResult>
//{
//    public AddUserDTO Model { get; set; }

//    public AddUserCommand(AddUserDTO model)
//    {
//        Model = model;
//    }

//    public class Handler : IRequestHandler<AddUserCommand, CommandResult>
//    {
//        private readonly UserManager<AppUser> _userManager;
//        public readonly IMailService _emailSender;

//        public Handler(UserManager<AppUser> userManager, IMailService emailSender)
//        {
//            _userManager = userManager;
//            _emailSender = emailSender;
//        }

//        public async Task<CommandResult> Handle(AddUserCommand request, CancellationToken cancellationToken)
//        {
//            var User = await _userManager.FindByEmailAsync(request.Model.Email);
//            if (User != null)
//                return CommandResult.GetFailed(ErrorMessageConstants.INVALID_MAIL_OR_PASSWORD);

//            AppUser createUser = new AppUser
//            {
//                Id = Guid.NewGuid().ToString(),
//                UserName = request.Model.Email,
//                Name = request.Model.Name,
//                Surname = request.Model.Surname,
//                Email = request.Model.Email,
//                AvatarImage = request.Model.AvatarImage,
//                CreatedAt = DateTimeOffset.UtcNow
//            };

//            //var randomGenerator = new Random(123);
//            //var password = randomGenerator.Next(153845, 999999).ToString();

//            IdentityResult result = await _userManager.CreateAsync(createUser, request.Model.Password);
//            var role = request.Model.IsAdmin ? Roles.Admin.ToString() : Roles.SystemUser.ToString();
//            var roleResult = await _userManager.AddToRoleAsync(createUser, role);

//            if (result.Succeeded)
//            {
//                return CommandResult.GetSucceed();
//            }
//            return CommandResult.GetFailed(ErrorMessageConstants.USER_CANT_CREATE);
//        }


//    }
//}
