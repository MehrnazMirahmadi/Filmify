namespace Filmify.UI.Services;

public class IdentityApiClient(HttpClient client)
{
    public async Task<AuthResponse> LoginAsync(string email, string password)
    {
        var response = await client.PostAsJsonAsync("api/Auth/login", new { email, password });
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
        if (result == null) throw new Exception("Response was null");
        return result;
    }

    public async Task RegisterAsync(string fullName, string email, string password, string[] roles)
    {
        var response = await client.PostAsJsonAsync("api/Auth/register", new { fullName, email, password, roles });
        response.EnsureSuccessStatusCode();
    }
}

