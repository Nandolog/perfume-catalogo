using System.Net.Http.Json;
using PerfumeCatalog.Client.Models;

namespace PerfumeCatalog.Client.Services;
public class ProductoService
{
    private readonly HttpClient _http;
    public ProductoService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<Producto>> ObtenerProductosAsync()
    {
        var productos = await _http.GetFromJsonAsync<List<Producto>>("productos.json");
        return productos ?? new List<Producto>();
    }
}