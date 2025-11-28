using Microsoft.Extensions.DependencyInjection;
using Chamados.Core.DataAccess;
using Chamados.Core.ViewModels;
using Chamados.Core.Services;

namespace Chamados.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddChamadosCoreServices(this IServiceCollection services, string? connectionString)
        {
            // Data Access
            services.AddSingleton(new ChamadoDAO(connectionString));
            services.AddSingleton(new UsuarioDAO(connectionString));

            // Services
            services.AddSingleton<ApiService>();

            // ViewModels
            services.AddTransient<LoginViewModel>();
            services.AddTransient<ListaChamadosViewModel>();
            services.AddTransient<NovoChamadoViewModel>();
            services.AddTransient<DetalhesChamadoViewModel>();

            return services;
        }
    }
}
