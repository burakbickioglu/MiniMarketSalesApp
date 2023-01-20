namespace BaseProject.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme/*, Roles ="SuperAdmin")*/)]
public class AdminController : ControllerBase
{
    protected readonly IMediator _mediator;

    public AdminController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("GetUser/{id}")]
    public async Task<CommandResult<UserDTO>> GetUser(Guid id) => await _mediator.Send(new GetUserQuery(id));

    [HttpPost]
    [Route("GetUsers")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommandResult<DataSourceResult>))]
    public async Task<DataSourceResult> GetUsers([FromBody] DataTableApiRequest request)
    {
        var data = await _mediator.Send(new GetUsersQuery(request));
        return data.Data;
    }

    [HttpGet]
    [Route("GetUsersByFilter/{filter}")]
    public async Task<CommandResult<List<UserDTO>>> GetUsersByFilter(string filter) => await _mediator.Send(new GetUsersByFilterQuery(filter));

    [HttpGet]
    [Route("GetUserStatistics")]
    public async Task<CommandResult<UserStatisticsDTO>> GetUserStatistics() => await _mediator.Send(new GetUserStatisticsQuery());

    [HttpGet]
    [Route("GetLastFiveUsers")]
    public async Task<CommandResult<List<UserDTO>>> GetLastFiveUsers() => await _mediator.Send(new GetLastFiveUsersQuery());

    //[HttpPost]
    //[Route("AddUser")]
    //public async Task<CommandResult> AddUser([FromBody] AddUserDTO model) => await _mediator.Send(new AddUserCommand(model));

    [HttpPost]
    [Route("UpdateUser")]
    public async Task<CommandResult<UserDTO>> UpdateUser([FromBody] UserDTO model) => await _mediator.Send(new UpdateUserCommand(model));

    [HttpDelete]
    [Route("DeleteUser/{id}")]
    public async Task<CommandResult> DeleteUser(Guid id) => await _mediator.Send(new DeleteUserCommand(id));
}
