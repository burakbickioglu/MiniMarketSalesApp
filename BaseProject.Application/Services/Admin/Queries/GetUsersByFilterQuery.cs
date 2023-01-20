namespace BaseProject.Service.Services.Admin.Queries;
public class GetUsersByFilterQuery : CommandBase<CommandResult<List<UserDTO>>>
{
    public string Filter { get; set; }

    public GetUsersByFilterQuery(string filter)
    {
        Filter = filter;
    }

    public class Handler : IRequestHandler<GetUsersByFilterQuery, CommandResult<List<UserDTO>>>
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public Handler(IMapper mapper, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<CommandResult<List<UserDTO>>> Handle(GetUsersByFilterQuery request, CancellationToken cancellationToken)
        {
            var users = await _userManager.Users.Where(p => !string.IsNullOrEmpty(p.Name) && p.Name.ToLower().Contains(request.Filter)
                || !string.IsNullOrEmpty(p.Surname) && p.Surname.ToLower().Contains(request.Filter)
                || !string.IsNullOrEmpty(p.Email) && p.Email.ToLower().Contains(request.Filter)).ToListAsync();

            var response = _mapper.Map<List<UserDTO>>(users.ToList());
            
            return response != null
                ? CommandResult<List<UserDTO>>.GetSucceed(response)
                : CommandResult<List<UserDTO>>.GetSucceed(new List<UserDTO>());
        }
    }
}
