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
using System.Windows.Controls.Primitives;
using System.Collections.Specialized;

namespace BetterTabs
{
    /// <summary>
    /// Interaction logic for BetterDataGrid.xaml
    /// </summary>
    public partial class BetterDataGrid : DataGrid, INotifyPropertyChanged
    {
        private bool autoFilter;
        private List<FilterValue> filterList;
        private int filterColumnIndex;
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

        public event PropertyChangedEventHandler PropertyChanged;

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
        public bool AutoFilterEnabled
        {
            get
            {
                if (this.ItemsSource != null)
                {
                    Type[] types = (from object thisItem in this.ItemsSource
                                    select thisItem.GetType()).ToArray();
                    if(types.Distinct().Count() == 1)
                        return autoFilter;
                }
                return false;
            }
            set
            {
                autoFilter = value;
                OnPropertyChanged("AutoFilter");
            }
        }
        public BetterDataGrid()
        {
            filterList = new List<FilterValue>();
            InitializeComponent();
        }
        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        private void FilterPopup_Opened(object sender, EventArgs e)
        {
            List<string> columnValues = new List<string>();
            FilterPopup filterPopup = (FilterPopup)sender;
            for(int x = 1; x < filterPopup.FilterList.Items.Count; x++)
            {
                filterPopup.FilterList.Items.RemoveAt(x);
            }
            if(this.HasItems)
            {
                PropertyPath propertyPath = ((Binding)((DataGridBoundColumn)this.Columns[filterColumnIndex]).Binding).Path;
                Type itemsType = this.Items[0].GetType();
                foreach(object item in this.Items)
                {
                    columnValues.Add(itemsType.GetProperty(propertyPath.Path).GetMethod.Invoke(item, new object[] { }).ToString());
                }
                columnValues.Sort();
                foreach(string thisValue in columnValues)
                {
                    filterPopup.Add(thisValue);
                }
            }
        }

        private void AllCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            foreach (object item in ((ItemsControl)((FrameworkElement)sender).Parent).Items)
            {
                CheckBox thisCheck = (CheckBox)item;
                thisCheck.IsChecked = true;
            }
        }

        private void AllCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (object item in ((ItemsControl)((FrameworkElement)sender).Parent).Items)
            {
                CheckBox thisCheck = (CheckBox)item;
                thisCheck.IsChecked = false;
            }
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            bool all = true;
            foreach (object item in ((ItemsControl)((FrameworkElement)sender).Parent).Items)
            {
                CheckBox thisCheck = (CheckBox)item;
                if (thisCheck.IsChecked == false)
                {
                    all = false;
                }
            }
            ((CheckBox)((ItemsControl)((FrameworkElement)sender).Parent).Items[0]).IsChecked = all ? true : new bool?();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            bool all = true;
            foreach (object item in ((ItemsControl)((FrameworkElement)sender).Parent).Items)
            {
                CheckBox thisCheck = (CheckBox)item;
                if (thisCheck.IsChecked != false)
                {
                    all = false;
                }
            }
            ((CheckBox)((ItemsControl)((FrameworkElement)sender).Parent).Items[0]).IsChecked = all ? false : new bool?();
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            Button filterButton = (Button)sender;
            filterColumnIndex = (int)filterButton.Tag;
            ((DataGridColumnHeader)filterButton.TemplatedParent).
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
            else if(!targetType.IsAssignableFrom(typeof(Path)))
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
            else if (!targetType.IsAssignableFrom(typeof(ListSortDirection?)))
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
    public class BoolVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool bValue = true;
            Visibility vValue = Visibility.Visible;
            object ret = new object();
            if (value == null)
                throw new ArgumentNullException("value");
            else if (value.GetType() != typeof(bool) && value.GetType() != typeof(Visibility))
                throw new ArgumentException("value must be of type bool or System.Windows.Visibility", "value");
            else if (!targetType.IsAssignableFrom(typeof(Visibility)) && !targetType.IsAssignableFrom(typeof(bool)))
                throw new ArgumentException("targetType must be assignable from bool or System.Windows.Visibility", "targetType");
            else
            {
                if (value.GetType() == typeof(bool))
                {
                    bValue = (bool)value;
                    if (targetType.IsAssignableFrom(typeof(Visibility)))
                    {
                        if (bValue)
                            return Visibility.Visible;
                        else
                            return Visibility.Collapsed;
                    }
                    else
                    {
                        return bValue;
                    }
                }
                else
                {
                    vValue = (Visibility)value;
                    if (targetType.IsAssignableFrom(typeof(bool)))
                    {
                        if (vValue == Visibility.Visible)
                            return true;
                        else
                            return false;
                    }
                    else
                    {
                        return vValue;
                    }
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool bValue = true;
            Visibility vValue = Visibility.Visible;
            object ret = new object();
            if (value == null)
                throw new ArgumentNullException("value");
            else if (value.GetType() != typeof(bool) && value.GetType() != typeof(Visibility))
                throw new ArgumentException("value must be of type bool or System.Windows.Visibility", "value");
            else if (!targetType.IsAssignableFrom(typeof(Visibility)) && !targetType.IsAssignableFrom(typeof(bool)))
                throw new ArgumentException("targetType must be assignable from bool or System.Windows.Visibility", "targetType");
            else
            {
                if (value.GetType() == typeof(bool))
                {
                    bValue = (bool)value;
                    if (targetType.IsAssignableFrom(typeof(Visibility)))
                    {
                        if (bValue)
                            return Visibility.Visible;
                        else
                            return Visibility.Collapsed;
                    }
                    else
                    {
                        return bValue;
                    }
                }
                else
                {
                    vValue = (Visibility)value;
                    if (targetType.IsAssignableFrom(typeof(bool)))
                    {
                        if (vValue == Visibility.Visible)
                            return true;
                        else
                            return false;
                    }
                    else
                    {
                        return vValue;
                    }
                }
            }
        }
    }
    public class FilterValue
    {
        string propertyName;
        List<string> permittedValues;

        public FilterValue()
        {
            PropertyName = "";
            PermittedValues = new List<string>();
        }

        public FilterValue(string propertyName, List<string> permittedValues)
        {
            this.PropertyName = propertyName;
            this.PermittedValues = permittedValues;
        }

        public string PropertyName { get => propertyName; set => propertyName = value; }
        public List<string> PermittedValues { get => permittedValues; set => permittedValues = value; }
    }
}
