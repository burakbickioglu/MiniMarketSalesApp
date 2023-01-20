namespace BaseProject.Domain.Models.Dtos.Authorization;

public class RegisterRequestDTO
{

    [Display(Name = "E-posta Adresi")]
    public string Email { get; set; }
    [Display(Name = "Şifre")]
    public string Password { get; set; }
    public string? Captcha { get; set; } // Şu an için nullable

}
