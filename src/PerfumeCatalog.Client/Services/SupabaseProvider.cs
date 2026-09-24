using Microsoft.Extensions.Configuration;
using Supabase;

namespace PerfumeCatalog.Client.Services;

public class SupabaseProvider
{
    private readonly string _url;
    private readonly string _key;
    private Supabase.Client? _client;
    private Task? _initTask;

    public SupabaseProvider(IConfiguration config)
    {
        _url = config["Supabase:Url"]!;
        _key = config["Supabase:AnonKey"]!;
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
        _client = new Supabase.Client(_url, _key, new SupabaseOptions
        {
            AutoRefreshToken = true,
            AutoConnectRealtime = false
        });
        await _client.InitializeAsync();
    }
}