using Microsoft.Extensions.Logging;
using Chamados.Core.Services;
using Chamados.Core.ViewModels;
using Chamados.Mobile.Views;
using Chamados.Core.Data;
using Chamados.Core.Models;

namespace Chamados.Mobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            
            // Configuração de Injeção de Dependência
            builder.Services.AddSingleton<ApiService>(s => 
                new ApiService("http://localhost:5000/api/")); // URL base da Web API
            
            // Registro dos ViewModels
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<ListaChamadosViewModel>();
            builder.Services.AddTransient<NovoChamadoViewModel>();
            builder.Services.AddTransient<DetalhesChamadoViewModel>();

            // Registro das Páginas (Views)
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<ListaChamadosPage>();
            builder.Services.AddTransient<NovoChamadoPage>();
            builder.Services.AddTransient<DetalhesChamadoPage>();

            return builder.Build();
        }
    }
}
