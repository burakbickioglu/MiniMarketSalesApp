namespace BaseProject.BO.Models;
public class DashboardViewModel
{
    public UserStatisticsDTO? UserStatics { get; set; }
    public List<UserDTO>? LastFiveUser { get; set; }
}
