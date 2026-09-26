using Microsoft.JSInterop;

namespace PerfumeCatalog.Client.Services;

public class TemaService
{
    private readonly IJSRuntime _js;
    private const string StorageKey = "tema-preferido";

    public string TemaActual { get; private set; } = "light";
    public event Action? OnTemaCambiado;

    public TemaService(IJSRuntime js)
    {
        _js = js;
    }

    public async Task InicializarAsync()
    {
        try
        {
            // 1. Buscar tema guardado en localStorage
            var temaGuardado = await _js.InvokeAsync<string?>("localStorage.getItem", StorageKey);

            if (!string.IsNullOrEmpty(temaGuardado))
            {
                TemaActual = temaGuardado;
            }
            else
            {
                // 2. Si no hay, detectar preferencia del sistema operativo
                var prefiereOscuro = await _js.InvokeAsync<bool>(
                    "window.matchMedia('(prefers-color-scheme: dark)').matches");

                TemaActual = prefiereOscuro ? "dark" : "light";
            }

            await AplicarTemaAsync(TemaActual);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error inicializando tema: {ex.Message}");
        }
    }

    public async Task AlternarAsync()
    {
        TemaActual = TemaActual == "dark" ? "light" : "dark";
        await AplicarTemaAsync(TemaActual);
        OnTemaCambiado?.Invoke();
    }

    private async Task AplicarTemaAsync(string tema)
    {
        await _js.InvokeVoidAsync("document.documentElement.setAttribute", "data-theme", tema);
        await _js.InvokeVoidAsync("localStorage.setItem", StorageKey, tema);
    }
}