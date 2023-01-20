namespace BaseProject.Service.Services.Admin.Queries;

public class GetUsersQuery : CommandBase<CommandResult<DataSourceResult>>
{
    public DataTableApiRequest Expression { get; }

    public GetUsersQuery(DataTableApiRequest expression)
    {
        Expression = expression;
    }

    public class Handler : IRequestHandler<GetUsersQuery, CommandResult<DataSourceResult>>
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

        public async Task<CommandResult<DataSourceResult>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = _userManager.Users.AsNoTracking();

            var dsrequest = request.Expression.Request;
            if (!string.IsNullOrEmpty(request.Expression.RequestFilters))
                dsrequest.Filters = FilterDescriptorFactory.Create(request.Expression.RequestFilters);


            var result = await users.ToDataSourceResultAsync(dsrequest, r => _mapper.Map<UserListViewModel>(r));

            return CommandResult<DataSourceResult>.GetSucceed(result);
        }
    }
}
