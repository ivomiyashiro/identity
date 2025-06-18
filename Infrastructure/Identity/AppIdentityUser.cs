using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public class AppIdentityUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
}