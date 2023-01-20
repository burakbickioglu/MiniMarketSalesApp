namespace BaseProject.Service.Utilities.Helpers
{
    public interface IGoogleLoginHelper
    {
        Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(ExternalAuthDTO externalAuth);
    }
}
