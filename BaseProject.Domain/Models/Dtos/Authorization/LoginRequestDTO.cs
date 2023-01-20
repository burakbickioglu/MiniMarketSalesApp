namespace BaseProject.Domain.Models.Dtos.Authorization;

public class LoginRequestDTO
{
    [Required]
    [Display(Name = "email")]
    public string Email { get; set; }

    [Required]
    [Display(Name = "password")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    public string? Captcha { get; set; } // Şu an için nullable

    public LoginRequestDTO()
    {

    }

    public LoginRequestDTO(string email, string password)
    {
        Email = email;
        Password = password;
    }

    public LoginRequestDTO(string email, string password, string captcha)
    {
        Email = email;
        Password = password;
        Captcha = captcha;
    }
}
