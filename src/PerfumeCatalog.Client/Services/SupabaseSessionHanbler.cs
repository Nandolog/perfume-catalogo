using Microsoft.JSInterop;
using Supabase.Gotrue;
using Supabase.Gotrue.Interfaces;

namespace PerfumeCatalog.Client.Services;

public class SupabaseSessionHandler : IGotrueSessionPersistence<Session>
{
    private const string Key = "supabase.session";
    private static IJSRuntime? _js;

    // Se llama una vez al arrancar la app
    public static void Initialize(IJSRuntime js)
    {
        _js = js;
    }

    public void SaveSession(Session session)
    {
        if (_js is null) return;
        try
        {
            var json = System.Text.Json.JsonSerializer.Serialize(session);
            // Nota: InvokeVoidAsync es async, pero no esperamos el resultado.
            // Confiamos en que el navegador lo ejecutará igual.
            _ = _js.InvokeVoidAsync("localStorageSync.set", Key, json);
        }
        catch { /* ignorar si falla */ }
    }

    public void DestroySession()
    {
        if (_js is null) return;
        try
        {
            _ = _js.InvokeVoidAsync("localStorageSync.remove", Key);
        }
        catch { /* ignorar */ }
    }

    public Session? LoadSession()
    {
        // No puede ser sincrónico en WASM sin bloquear.
        // Devolvemos null y dejamos que AutoRefreshToken haga su trabajo
        // una vez que tenemos el refresh_token guardado.
        return null;
    }
}