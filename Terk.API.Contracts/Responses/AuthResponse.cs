namespace Terk.API.Contracts.Responses;

/// <summary>
/// Authentication response
/// </summary>
/// <param name="Id">ID of authenticated user</param>
/// <param name="JwtToken">String with JWT bearer token to use</param>
public record AuthResponse(int Id, string JwtToken);