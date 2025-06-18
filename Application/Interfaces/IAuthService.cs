namespace Application.Interfaces;

public interface IAuthService
{
    // Task<string> GetCurrentUser();
    Task<string> Login(string email, string password);
    Task<string> Logout();
    Task<string> Register(string fullName, string email, string password);
    // Task<string> ResetPassword();
}