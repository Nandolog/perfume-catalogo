using PerfumeCatalog.Client.Models;

namespace PerfumeCatalog.Client.Services;

public class CategoriaService
{
    private readonly SupabaseProvider _provider;

    public CategoriaService(SupabaseProvider provider)
    {
        _provider = provider;
    }

    public async Task<List<Categoria>> ObtenerTodasAsync()
    {
        var client = await _provider.GetClientAsync();
        var respuesta = await client
            .From<Categoria>()
            .Where(c => c.Activa == true)
            .Get();
        return respuesta.Models.OrderBy(c => c.Orden).ToList();
    }

    public async Task<List<Categoria>> ObtenerTodasAdminAsync()
    {
        var client = await _provider.GetClientAsync();
        var respuesta = await client.From<Categoria>().Get();
        return respuesta.Models.OrderBy(c => c.Orden).ToList();
    }

    public async Task<Categoria> CrearAsync(Categoria categoria)
    {
        var client = await _provider.GetClientAsync();
        var respuesta = await client.From<Categoria>().Insert(categoria);
        return respuesta.Models.First();
    }

    public async Task ActualizarAsync(Categoria categoria)
    {
        var client = await _provider.GetClientAsync();
        await client.From<Categoria>().Update(categoria);
    }

    public async Task BorrarAsync(int id)
    {
        var client = await _provider.GetClientAsync();
        await client.From<Categoria>().Where(c => c.Id == id).Delete();
    }
}