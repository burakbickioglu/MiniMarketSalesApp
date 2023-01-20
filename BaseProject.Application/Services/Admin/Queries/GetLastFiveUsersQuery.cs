namespace BaseProject.Service.Services.Admin.Queries;
public class GetLastFiveUsersQuery : CommandBase<CommandResult<List<UserDTO>>>
{
    public class Handler : IRequestHandler<GetLastFiveUsersQuery, CommandResult<List<UserDTO>>>
    {
        public UserManager<AppUser> _userManager { get; set; }
        private readonly IMapper _mapper;
        public Handler(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<CommandResult<List<UserDTO>>> Handle(GetLastFiveUsersQuery request, CancellationToken cancellationToken)
        {
            var users = _userManager.Users;
            var model = await users.OrderByDescending(p => p.CreatedAt).Take(5).ToListAsync();
            return CommandResult<List<UserDTO>>.GetSucceed(_mapper.Map<List<UserDTO>>(model));
        }
    }
}
