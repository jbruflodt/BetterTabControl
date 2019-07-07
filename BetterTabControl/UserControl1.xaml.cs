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
using System.ComponentModel;


namespace BetterTabs
{
    /// <summary>
    /// Interaction logic for BetterDataGrid.xaml
    /// </summary>
    public partial class BetterDataGrid : DataGrid
    {
        private static Path ascendingArrow = new Path()
        {
            Data = Geometry.Parse("M0.50000001,5.0100004 L5.1155531,0.50000001 9.6670012,4.9473615 M5.1060004,8.7700007 C5.1060004,0.66589555 5.1060004,0.77006571 5.1060004,0.77006571")
            , HorizontalAlignment = HorizontalAlignment.Stretch, Height = 9.27, Margin = new Thickness(0), Stretch = Stretch.Fill, VerticalAlignment = VerticalAlignment.Stretch,
            Width = 10.167
        };
        private static Path descendingArrow = new Path()
        {
            Data = Geometry.Parse("M0.50000001,5.0100004 L5.1155531,0.50000001 9.6670012,4.9473615 M5.1060004,8.7700007 C5.1060004,0.66589555 5.1060004,0.77006571 5.1060004,0.77006571")
            ,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Height = 9.27,
            Margin = new Thickness(0),
            Stretch = Stretch.Fill,
            VerticalAlignment = VerticalAlignment.Stretch,
            Width = 10.167,
            RenderTransformOrigin = new Point(0.5,0.5)
        };
        public static Path AscendingArrow
        {
            get
            {
                return ascendingArrow;
            }
        }
        public static Path DescendingArrow
        {
            get
            {
                return descendingArrow;
            }
        }
        public BetterDataGrid()
        {
            InitializeComponent();
        }
    }
    public class SortDirectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ListSortDirection? listSortDirection = new ListSortDirection?();
            if (value != null)
                listSortDirection = (ListSortDirection?)value;
            else
                return null;
            if (value.GetType() != typeof(ListSortDirection?))
                throw new ArgumentException("value must be of type ListSortDirection? which is received from the SortDirection property of the column", "value");
            else if(targetType.IsAssignableFrom(typeof(Path)))
                throw new ArgumentException("targetType must be assignable from System.Windows.Shapes.Path", "targetType");
            else
            {
                
                if (listSortDirection == null || !listSortDirection.HasValue)
                    return null;
                else if (listSortDirection.Value == ListSortDirection.Ascending)
                    return BetterDataGrid.AscendingArrow;
                else if (listSortDirection.Value == ListSortDirection.Descending)
                    return BetterDataGrid.DescendingArrow;
                else
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            else if (value.GetType() != typeof(Path))
                throw new ArgumentException("value must be of type System.Windows.Shapes.Path", "value");
            else if (targetType.IsAssignableFrom(typeof(ListSortDirection?)))
                throw new ArgumentException("targetType must be assignable from System.ComponentModel.ListSortDirection", "targetType");
            else
            {
                if (value == null)
                    return new ListSortDirection?();
                else if ((Path)value == BetterDataGrid.AscendingArrow)
                    return new ListSortDirection?(ListSortDirection.Ascending);
                else if ((Path)value == BetterDataGrid.DescendingArrow)
                    return new ListSortDirection?(ListSortDirection.Descending);
                else
                    return new ListSortDirection?();
            }
        }
    }
}
