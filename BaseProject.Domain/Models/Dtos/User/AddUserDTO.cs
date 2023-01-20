namespace BaseProject.Domain.Models.Dtos.User;

public class AddUserDTO
{
    [Display(Name = "name")]
    public string Name { get; set; }
    [Display(Name = "surname")]
    public string Surname { get; set; }

    [Display(Name = "email")]
    public string Email { get; set; }
    [Display(Name = "avatar")]
    public string? AvatarImage { get; set; }
    [Display(Name = "password")]
    public string Password { get; set; }
    public bool IsAdmin { get; set; }

}
