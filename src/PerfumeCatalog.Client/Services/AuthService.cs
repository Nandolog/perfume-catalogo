using Supabase.Gotrue;

namespace PerfumeCatalog.Client.Services;

public class AuthService
{
    private readonly SupabaseProvider _provider;

    public AuthService(SupabaseProvider provider)
    {
        _provider = provider;
    }

    public async Task<Session?> LoginAsync(string email, string password)
    {
        var client = await _provider.GetClientAsync();
        return await client.Auth.SignIn(email, password);
    }

    public async Task LogoutAsync()
    {
        var client = await _provider.GetClientAsync();
        await client.Auth.SignOut();
    }

    public async Task<bool> EstaAutenticadoAsync()
    {
        var client = await _provider.GetClientAsync();
        return client.Auth.CurrentUser != null;
    }

    public async Task<string?> EmailActualAsync()
    {
        var client = await _provider.GetClientAsync();
        return client.Auth.CurrentUser?.Email;
    }

    public async Task CambiarPasswordAsync(string nuevaPassword)
{
    var client = await _provider.GetClientAsync();
    await client.Auth.Update(new Supabase.Gotrue.UserAttributes
    {
        Password = nuevaPassword
    });
}
}