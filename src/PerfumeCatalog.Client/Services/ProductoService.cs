using Microsoft.Extensions.Configuration;
using PerfumeCatalog.Client.Models;
using Supabase;

namespace PerfumeCatalog.Client.Services;

public class ProductoService
{
    private readonly Supabase.Client _supabase;

    public ProductoService(IConfiguration config)
    {
        var url = config["Supabase:Url"]
            ?? throw new InvalidOperationException("Falta Supabase:Url en appsettings.json");
        var key = config["Supabase:AnonKey"]
            ?? throw new InvalidOperationException("Falta Supabase:AnonKey en appsettings.json");

        _supabase = new Supabase.Client(url, key, new SupabaseOptions
        {
            AutoRefreshToken = true,
            AutoConnectRealtime = false
        });
    }

    private async Task EnsureInitializedAsync()
    {
        if (_supabase.Auth.CurrentSession == null && _supabase.Auth.CurrentUser == null)
        {
            await _supabase.InitializeAsync();
        }
    }

    public async Task<List<Producto>> ObtenerProductosAsync()
    {
        await EnsureInitializedAsync();
        var respuesta = await _supabase
            .From<Producto>()
            .Where(p => p.Disponible == true)
            .Get();

        return respuesta.Models;
    }
}