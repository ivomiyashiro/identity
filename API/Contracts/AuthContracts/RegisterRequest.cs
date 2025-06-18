namespace API.Contracts.AuthContracts;

public sealed record RegisterRequest(string FullName, string Email, string Password, string ConfirmPassword);