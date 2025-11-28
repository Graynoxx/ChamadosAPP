using Chamados.Mobile.Views;
using Microsoft.Maui.Controls;

namespace Chamados.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Registra as rotas para navegação Shell
            Routing.RegisterRoute(nameof(ListaChamadosPage), typeof(ListaChamadosPage));
            Routing.RegisterRoute(nameof(NovoChamadoPage), typeof(NovoChamadoPage));
            Routing.RegisterRoute(nameof(DetalhesChamadoPage), typeof(DetalhesChamadoPage));

            // Define a página inicial como a LoginPage
            MainPage = new NavigationPage(new LoginPage());
            
            // Em um cenário real com Shell, seria:
            // MainPage = new AppShell();
            // Mas para simplificar a navegação inicial sem um AppShell complexo:
            // MainPage = new NavigationPage(new LoginPage());
        }
    }
}
