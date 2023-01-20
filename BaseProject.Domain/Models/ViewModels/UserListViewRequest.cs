namespace BaseProject.Domain.Models.ViewModels;

public class UserListViewRequest : DataSourceApiRequest
{
    public string? UserFilter { get; set; }
}
