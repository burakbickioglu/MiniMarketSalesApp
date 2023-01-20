namespace BaseProject.Application.Interfaces;

public interface ITokenHandler
{
    Token CreateAccessToken(int day);
}
