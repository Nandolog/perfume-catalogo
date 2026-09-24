using PerfumeCatalog.Client.Models;
using Supabase;

namespace PerfumeCatalog.Client.Services;

public class ProductoService
{
    private readonly SupabaseProvider _provider;

    public ProductoService(SupabaseProvider provider)
    {
        _provider = provider;
    }

    public async Task<List<Producto>> ObtenerProductosAsync()
    {
        var client = await _provider.GetClientAsync();
        var respuesta = await client
            .From<Producto>()
            .Where(p => p.Disponible == true)
            .Get();
        return respuesta.Models;
    }

    public async Task<List<Producto>> ObtenerTodosAsync()
    {
        var client = await _provider.GetClientAsync();
        var respuesta = await client.From<Producto>().Get();
        return respuesta.Models;
    }

    public async Task<Producto> CrearAsync(Producto producto)
    {
        var client = await _provider.GetClientAsync();
        var respuesta = await client.From<Producto>().Insert(producto);
        return respuesta.Models.First();
    }

    public async Task ActualizarAsync(Producto producto)
    {
        var client = await _provider.GetClientAsync();
        await client.From<Producto>().Update(producto);
    }

    public async Task BorrarAsync(int id)
    {
        var client = await _provider.GetClientAsync();
        await client.From<Producto>().Where(p => p.Id == id).Delete();
    }

    public async Task<string> SubirImagenAsync(byte[] bytes, string nombreArchivo)
    {
        var client = await _provider.GetClientAsync();
        var nombreFinal = $"{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}_{nombreArchivo}";
        await client.Storage.From("productos").Upload(bytes, nombreFinal);
        return client.Storage.From("productos").GetPublicUrl(nombreFinal);
    }

    
}