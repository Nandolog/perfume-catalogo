using Microsoft.Extensions.Configuration;
using PerfumeCatalog.Client.Models;
using Supabase;

namespace PerfumeCatalog.Client.Services;

public class ProductoService
{
    private readonly Supabase.Client _supabase;
    private bool _inicializado;

    public ProductoService(IConfiguration config)
    {
        var url = config["Supabase:Url"]!;
        var key = config["Supabase:AnonKey"]!;

        _supabase = new Supabase.Client(url, key, new SupabaseOptions
        {
            AutoRefreshToken = true,
            AutoConnectRealtime = false
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

    // Lectura pública (solo disponibles)
    public async Task<List<Producto>> ObtenerProductosAsync()
    {
        await EnsureInitializedAsync();
        var respuesta = await _supabase
            .From<Producto>()
            .Where(p => p.Disponible == true)
            .Get();
        return respuesta.Models;
    }

    // Lectura admin (todos, disponibles o no)
    public async Task<List<Producto>> ObtenerTodosAsync()
    {
        await EnsureInitializedAsync();
        var respuesta = await _supabase.From<Producto>().Get();
        return respuesta.Models;
    }

    // Crear
    public async Task<Producto> CrearAsync(Producto producto)
    {
        await EnsureInitializedAsync();
        var respuesta = await _supabase.From<Producto>().Insert(producto);
        return respuesta.Models.First();
    }

    // Actualizar
    public async Task ActualizarAsync(Producto producto)
    {
        await EnsureInitializedAsync();
        await _supabase.From<Producto>().Update(producto);
    }

    // Borrar
    public async Task BorrarAsync(int id)
    {
        await EnsureInitializedAsync();
        await _supabase.From<Producto>().Where(p => p.Id == id).Delete();
    }

    // Subir imagen al bucket "productos"
    public async Task<string> SubirImagenAsync(byte[] bytes, string nombreArchivo)
    {
        await EnsureInitializedAsync();

        var nombreFinal = $"{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}_{nombreArchivo}";

        await _supabase.Storage
            .From("productos")
            .Upload(bytes, nombreFinal);

        return _supabase.Storage.From("productos").GetPublicUrl(nombreFinal);
    }
}