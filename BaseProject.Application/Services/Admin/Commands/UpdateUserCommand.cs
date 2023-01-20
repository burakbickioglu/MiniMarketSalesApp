namespace BaseProject.Service.Services.Admin.Commands;

public class UpdateUserCommand : CommandBase<CommandResult<UserDTO>>
{
    public UserDTO Model { get; set; }

    public UpdateUserCommand(UserDTO model)
    {
        Model = model;
    }

    public class Handler : IRequestHandler<UpdateUserCommand, CommandResult<UserDTO>>
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public Handler(IMapper mapper, UserManager<AppUser> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<CommandResult<UserDTO>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Model.Id);
            if (user == null)
                return CommandResult<UserDTO>.GetFailed(ErrorMessageConstants.USER_NOT_FOUND);

            user.Name = request.Model.Name;
            user.Surname = request.Model.Surname;
            var updateResult = await _userManager.UpdateAsync(user);
            return updateResult.Succeeded
                ? CommandResult<UserDTO>.GetSucceed(request.Model)
                : CommandResult<UserDTO>.GetFailed(ErrorMessageConstants.USER_CANT_UPDATE);
        }
    }
}
