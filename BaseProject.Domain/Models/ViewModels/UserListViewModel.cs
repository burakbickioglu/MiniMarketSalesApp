namespace BaseProject.BO.Models;

public class UserListViewModel
{
    public string Id { get; set; }
    [Display(Name = "name")]
    public string? Name { get; set; }
    [Display(Name = "surname")]
    public string? Surname { get; set; }
    [Display(Name = "email")]
    public string? Email { get; set; }
    [Display(Name = "avatar")]
    public string? AvatarImage { get; set; }
    [Display(Name = "last_login")]
    public DateTimeOffset LastLogin { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    //public virtual ICollection<string>? UserTitles { get; set; }
    public virtual ICollection<string>? Roles { get; set; }

}
