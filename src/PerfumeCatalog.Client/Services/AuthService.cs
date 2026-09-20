using Microsoft.Extensions.Configuration;
using Supabase;
using Supabase.Gotrue;

namespace PerfumeCatalog.Client.Services;
public class AuthService
{
    private readonly Supabase.Client _supabase;
    private bool _inicializado;

    public AuthService(IConfiguration config)
    {
        var url = config["Supabase:Url"];
        var key = config["Supabase:Anonkey"];
        _supabase = new Supabase.Client(url, key, new SupabaseOptions
        {
            AutoRefreshToken = true,
            AutoConnectRealtime = true
        });
    }

    private async Task EnsureInitializedAsync()
    {
        if (!_inicializado)
        {
            await _supabase.InitializeAsync();
            _inicializado = true;
        }
    }

    public async Task<Session?> LoginAsync(string email, string password)
    {
        await EnsureInitializedAsync();
        return await _supabase.Auth.SignIn(email, password);
        
    }

    public async Task LogoutAsync()
    {
        await _supabase.Auth.SignOut();
    }

    public async Task<bool> EstaAutenticadoAsync()
    {
        await EnsureInitializedAsync();
        return _supabase.Auth.CurrentUser != null;
    }

    public string? EmailActual()
    {
        return _supabase.Auth.CurrentUser?.Email;
    }

}