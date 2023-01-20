namespace BaseProject.Service.Utilities.Helpers;

public static class ProfileHelper
{
    public static List<Profile> GetProfiles()
    {
        return new List<Profile>
        {
            new BaseEntityProfile(),
            new UserProfile(),
            new ProductProfile(),
            new BasketProfile()
        };
    }
}
