namespace BaseProject.Service.Services.Admin.Queries;
public class GetUserStatisticsQuery : CommandBase<CommandResult<UserStatisticsDTO>>
{
    public class Handler : IRequestHandler<GetUserStatisticsQuery, CommandResult<UserStatisticsDTO>>
    {
        public UserManager<AppUser> _userManager { get; set; }

        public Handler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CommandResult<UserStatisticsDTO>> Handle(GetUserStatisticsQuery request, CancellationToken cancellationToken)
        {
            var users = _userManager.Users;
            UserStatisticsDTO model = new();
            model.TotalUserCount = users.Count();
            model.TodayUserCount = users.Where(p => p.CreatedAt.Date == DateTime.Today).Count();
            model.ThisMonthUserCount = users.Where(p => p.CreatedAt > DateTimeOffset.Now.AddDays(-30)).Count();
            return CommandResult<UserStatisticsDTO>.GetSucceed(model);
        }
    }
}
