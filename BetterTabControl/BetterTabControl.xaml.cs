using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;

namespace BetterTabs
{
    /// <summary>
    /// Interaction logic for BetterTabs.xaml
    /// </summary>
    public partial class BetterTabControl : UserControl
    {
        public BetterTabControl()
        {
            InitializeComponent();
        }
    }
    public class ChangeColorBrightness : IValueConverter
    {
        private Color ChangeColor(Color color, float factor)
        {
            return Color.FromArgb(color.A,
                (byte)(color.R * factor), (byte)(color.G * factor), (byte)(color.B * factor));
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(!parameter.GetType().GetTypeInfo().IsAssignableFrom(typeof(float)))
                throw new ArgumentException("parameter must be assignable from float", "parameter");
            Color color = Colors.Transparent;
            if (targetType == null)
            {
                throw new ArgumentNullException("targetType");
            }
            if (!targetType.GetTypeInfo().IsAssignableFrom(typeof(Color)) && !targetType.GetTypeInfo().IsAssignableFrom(typeof(SolidColorBrush)))
            {
                throw new ArgumentException("targetType must be assignable from Color or SolidColorBrush", "targetType");
            }
            if (value == null)
            {
                if (targetType.GetTypeInfo().IsAssignableFrom(typeof(Color)))
                    return color;
                else
                    return new SolidColorBrush(color);
            }
            if (!typeof(Color).GetTypeInfo().IsAssignableFrom(value.GetType()) && !typeof(SolidColorBrush).GetTypeInfo().IsAssignableFrom(value.GetType()))
            {
                throw new ArgumentException("value must be assignable to Color or SolidColorBrush", "value");
            }
            if (typeof(Color).GetTypeInfo().IsAssignableFrom(value.GetType()))
                color = (Color)value;
            else
                color = ((SolidColorBrush)value).Color;
            if (targetType.GetTypeInfo().IsAssignableFrom(typeof(Color)))
                return ChangeColor(color, (float)parameter);
            else
                return new SolidColorBrush(ChangeColor(color, (float)parameter));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
