namespace BaseProject.Domain.Models;

public class AppUser : IdentityUser
{
    [MaxLength(100)]
    public string? Name { get; set; }
    [MaxLength(100)]
    public string? Surname { get; set; }
    public string? PhoneNumber { get; set; }
    public string? AvatarImage { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    //public AccountType AccountType { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset LastLogin { get; set; }
    //public virtual ICollection<string> UserTitles { get; set; }
    public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }
    public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
    public virtual ICollection<IdentityUserToken<string>> Tokens { get; set; }
    public virtual ICollection<AppUserRole> UserRoles { get; set; }
    public virtual ICollection<Basket> Baskets { get; set; }
    public virtual ICollection<Sales> Sales { get; set; }
}
