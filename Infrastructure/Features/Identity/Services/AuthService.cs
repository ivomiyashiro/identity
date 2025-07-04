using Domain.Entities;
using Domain.Repositories;
using Domain.Services;
using Infrastructure.Features.Identity.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace Infrastructure.Features.Identity.Services;

public class AuthService(
    UserManager<AppIdentityUser> userManager,
    SignInManager<AppIdentityUser> signInManager,
    IUserRepository userRepository,
    IConfiguration configuration) : IAuthService
{
    private readonly UserManager<AppIdentityUser> _userManager = userManager;
    private readonly SignInManager<AppIdentityUser> _signInManager = signInManager;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IConfiguration _configuration = configuration;

    public async Task<bool> CreateUserAccountAsync(User user, string password, CancellationToken cancellationToken = default)
    {
        var identityUser = user.ToIdentityUser();
        var result = await _userManager.CreateAsync(identityUser, password);

        return result.Succeeded;
    }

    public async Task<bool> SignInAsync(User user, CancellationToken cancellationToken = default)
    {
        var identityUser = await _userManager.FindByEmailAsync(user.Email);
        if (identityUser == null) return false;

        await _signInManager.SignInAsync(identityUser, isPersistent: false);
        return true;
    }

    public async Task SignOutAsync(CancellationToken cancellationToken = default)
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<bool> ValidateCredentialsAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var result = await _signInManager.PasswordSignInAsync(email, password, false, false);
        return result.Succeeded;
    }

    public async Task<User?> GetCurrentUserAsync(IIdentity? identity, CancellationToken cancellationToken = default)
    {
        if (identity?.IsAuthenticated != true || identity.Name == null)
            return null;

        var user = await _userRepository.GetByEmailAsync(identity.Name, cancellationToken);
        return user;
    }

    public async Task<string?> ForgotPasswordAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByEmailAsync(email, cancellationToken);
        if (user == null) return null;

        var otp = GenerateOtp();
        var hashedOtp = HashOtp(otp);
        user.PasswordResetOtp = hashedOtp;
        user.OtpExpiresAt = DateTime.UtcNow.AddMinutes(10);
        user.OtpAttempts = 0;

        await _userRepository.UpdateAsync(user, cancellationToken);

        return otp;
    }

    public async Task<string?> VerifyResetOtpAsync(string email, string hashedOtp, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByEmailAsync(email, cancellationToken);

        if (user?.PasswordResetOtp == null || user.OtpExpiresAt < DateTime.UtcNow)
            return null;

        if (user.OtpAttempts >= 3)
        {
            user.PasswordResetOtp = null;
            user.OtpExpiresAt = null;
            user.OtpAttempts = 0;
            await _userRepository.UpdateAsync(user, cancellationToken);
            return null;
        }

        if (user.PasswordResetOtp != hashedOtp)
        {
            user.OtpAttempts++;
            await _userRepository.UpdateAsync(user, cancellationToken);
            return null;
        }

        user.PasswordResetOtp = null;
        user.OtpExpiresAt = null;
        user.OtpAttempts = 0;
        await _userRepository.UpdateAsync(user, cancellationToken);

        return GenerateResetToken(user.Id);
    }

    public async Task<bool> ResetPasswordAsync(string resetToken, string newPassword, CancellationToken cancellationToken = default)
    {
        var userId = ValidateResetToken(resetToken);
        if (userId == null) return false;

        var user = await _userRepository.GetByIdAsync((Guid)userId, cancellationToken);
        if (user == null) return false;

        var identityUser = await _userManager.FindByEmailAsync(user.Email);
        if (identityUser == null) return false;

        var token = await _userManager.GeneratePasswordResetTokenAsync(identityUser);
        var result = await _userManager.ResetPasswordAsync(identityUser, token, newPassword);

        return result.Succeeded;
    }

    private static string GenerateOtp()
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString();
    }

    private string GenerateResetToken(Guid userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? "your-secret-key-here");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim("userId", userId.ToString()),
                new Claim("purpose", "password-reset")
            ]),
            Expires = DateTime.UtcNow.AddMinutes(30),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private Guid? ValidateResetToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? "your-secret-key-here");

        tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        }, out SecurityToken validatedToken);

        var jwtToken = (JwtSecurityToken)validatedToken;
        var userIdClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == "userId")?.Value;
        var purposeClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == "purpose")?.Value;

        if (purposeClaim != "password-reset" || userIdClaim == null)
            return null;

        return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
    }

    private static string HashOtp(string otp)
    {
        return Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(otp)));
    }
}