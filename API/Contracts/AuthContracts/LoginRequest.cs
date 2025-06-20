namespace API.Contracts.AuthContracts;

public sealed record LoginRequest(string Email, string Password);