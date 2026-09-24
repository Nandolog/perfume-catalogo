using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using Supabase;

namespace PerfumeCatalog.Client.Services;

public class SupabaseProvider
{
    private readonly string _url;
    private readonly string _key;
    private Supabase.Client? _client;
    private Task? _initTask;
    private readonly IJSRuntime _js;

    public SupabaseProvider(IConfiguration config, IJSRuntime js)
    {
        _url = config["Supabase:Url"]!;
        _key = config["Supabase:AnonKey"]!;
        _js = js;
    }

    public async Task<Supabase.Client> GetClientAsync()
    {
        if (_client is not null) return _client;
        if (_initTask is null) _initTask = InicializarAsync();
        await _initTask;
        return _client!;
    }

    private async Task InicializarAsync()
    {
        // Inicializar el handler con el JS runtime ANTES de crear el cliente
        SupabaseSessionHandler.Initialize(_js);

        _client = new Supabase.Client(_url, _key, new SupabaseOptions
        {
            AutoRefreshToken = true,
            AutoConnectRealtime = false,
            SessionHandler = new SupabaseSessionHandler()
        });

        await _client.InitializeAsync();

        // Intentar restaurar la sesión guardada
        await RestaurarSesionAsync();
    }

    private async Task RestaurarSesionAsync()
{
    try
    {
        var json = await _js.InvokeAsync<string?>("localStorageSync.get", "supabase.session");
        if (string.IsNullOrEmpty(json)) return;

        var session = System.Text.Json.JsonSerializer.Deserialize<Supabase.Gotrue.Session>(json);
        if (session?.AccessToken is null || session.RefreshToken is null) return;

        // En esta versión, SetSession requiere accessToken + refreshToken
        await _client!.Auth.SetSession(session.AccessToken, session.RefreshToken);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[SupabaseProvider] Error restaurando sesión: {ex.Message}");
    }
}
}