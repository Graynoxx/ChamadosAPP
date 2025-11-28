using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Chamados.Core.ViewModels
{
    // Este conversor é necessário para mapear o Status do Chamado para uma cor no XAML
    // Ele deve estar no Core.ViewModels ou ser referenciado corretamente.
    // Colocando-o no Core.ViewModels para simplificar a referência no XAML do MAUI.
    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string status)
            {
                return status.ToLowerInvariant() switch
                {
                    "aberto" => Color.FromArgb("#007bff"), // Azul
                    "em andamento" => Color.FromArgb("#ffc107"), // Amarelo
                    "resolvido" => Color.FromArgb("#28a745"), // Verde
                    "fechado" => Color.FromArgb("#6c757d"), // Cinza
                    _ => Color.FromArgb("#6c757d"), // Cor padrão
                };
            }
            return Color.FromArgb("#6c757d");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
