using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Chamados.Web;
using Chamados.Core.Services;
using Chamados.Core.ViewModels;
using Chamados.Core;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configurar o HttpClient para a Web API
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Adicionar os serviços do Chamados.Core (ApiService e ViewModels)
// O connectionString é null aqui, pois o Blazor WebAssembly não acessa o DB diretamente.
builder.Services.AddChamadosCoreServices(null);

// O ApiService precisa do HttpClient configurado.
// Vamos substituir a injeção do ApiService para usar o HttpClient configurado acima.
builder.Services.AddScoped<ApiService>(sp =>
{
    var httpClient = sp.GetRequiredService<HttpClient>();
    // Definir a BaseAddress da API. Assumindo que a API estará na mesma origem, mas em /api
    // Em um ambiente de produção, esta URL seria configurada.
    httpClient.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress + "api/");
    return new ApiService(httpClient);
});

await builder.Build().RunAsync();
