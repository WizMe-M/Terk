namespace Terk.API.Contracts.Responses;

/// <summary>
/// Represents file attached with 
/// </summary>
/// <param name="Name">File name set for client on server</param>
/// <param name="Data">File data</param>
public record FileResponse(string Name, byte[] Data);