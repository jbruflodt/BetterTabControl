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
using System.Collections.ObjectModel;

namespace BetterTabs
{
    /// <summary>
    /// Interaction logic for AutoFilterDataGrid.xaml
    /// </summary>
    public partial class AutoFilterDataGrid : UserControl
    {
        private List<FilterValue> filterList;
        ObservableCollection<CheckBox> filterPopupContent;
        public static readonly DependencyProperty DataSourceProperty = DependencyProperty.Register(
            "DataSource",
            typeof(ListCollectionView),
            typeof(AutoFilterDataGrid),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnDataSourceChanged))
            );
        public ListCollectionView DataSource
        {
            get { return (ListCollectionView)GetValue(DataSourceProperty); }
            set { SetValue(DataSourceProperty, value); }
        }
        public Brush AlternatingRowBackgroundcolor
        {
            get { return dataGrid.AlternatingRowBackground; }
            set { dataGrid.AlternatingRowBackground = value; }
        }
        public bool IsReadOnly
        {
            get { return dataGrid.IsReadOnly; }
            set { dataGrid.IsReadOnly = value; }
        }
        public bool CanUserAddRows
        {
            get { return dataGrid.CanUserAddRows; }
            set { dataGrid.CanUserAddRows = value; }
        }
        public bool CanUserDeleteRows
        {
            get { return dataGrid.CanUserDeleteRows; }
            set { dataGrid.CanUserDeleteRows = value; }
        }
        public bool CanUserReorderColumns
        {
            get { return dataGrid.CanUserReorderColumns; }
            set { dataGrid.CanUserReorderColumns = value; }
        }
        public bool CanUserResizeColumns
        {
            get { return dataGrid.CanUserResizeColumns; }
            set { dataGrid.CanUserResizeColumns = value; }
        }
        public bool CanUserResizeRows
        {
            get { return dataGrid.CanUserResizeRows; }
            set { dataGrid.CanUserResizeRows = value; }
        }
        public bool CanUserSortColumns
        {
            get { return dataGrid.CanUserSortColumns; }
            set { dataGrid.CanUserSortColumns = value; }
        }
        public ObservableCollection<CheckBox> FilterPopupContent
        {
            get
            {
                return filterPopupContent;
            }
        }
        public AutoFilterDataGrid()
        {
            filterPopupContent = new ObservableCollection<CheckBox>();
            CheckBox tempCheck = new CheckBox();
            tempCheck.Content = "All";
            tempCheck.FontWeight = FontWeights.Bold;
            tempCheck.IsThreeState = true;
            tempCheck.Checked += AllCheckBox_Checked;
            tempCheck.Unchecked += AllCheckBox_Unchecked;
            filterPopupContent.Add(tempCheck);
            filterList = new List<FilterValue>();
            InitializeComponent();
            this.Loaded += AutoFilterDataGridLoaded;
        }
        private static void OnDataSourceChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            AutoFilterDataGrid autoFilterDataGrid = sender as AutoFilterDataGrid;
            ((CollectionView)e.NewValue).Filter = new Predicate<object>(autoFilterDataGrid.Contains);
            autoFilterDataGrid.GenerateFilterList();
        }
        private bool Contains(object testObject)
        {
            bool filtered = false;
            foreach(FilterValue thisFilter in filterList)
            {
                string testObjectValue = testObject.GetType().GetProperty(thisFilter.PropertyName).GetMethod.Invoke(testObject, new object[] { }).ToString();
                foreach(string filterValue in thisFilter.FilteredValues)
                {
                    if(filterValue == testObjectValue)
                    {
                        filtered = true;
                        return !filtered;
                    }
                }
            }
            return !filtered;
        }
        private void AutoFilterDataGridLoaded(object sender, RoutedEventArgs e)
        {
            GenerateFilterList();
        }
        private void GenerateFilterList()
        {
            filterList = new List<FilterValue>();
            foreach (DataGridBoundColumn thisColumn in dataGrid.Columns)
            {
                filterList.Add(new FilterValue(((Binding)thisColumn.Binding).Path.Path.ToString(), new List<string>()));
            }
        }
        private void AllCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            for (int x = 1; x < filterPopupContent.Count; x++)
            {
                CheckBox thisCheck = filterPopupContent[x];
                thisCheck.IsChecked = true;
            }
        }

        private void AllCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            for (int x = 1; x < filterPopupContent.Count; x++)
            {
                CheckBox thisCheck = filterPopupContent[x];
                thisCheck.IsChecked = false;
            }
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            bool all = true;
            for (int x = 1; x < filterPopupContent.Count; x++)
            {
                CheckBox thisCheck = filterPopupContent[x];
                if (thisCheck.IsChecked == false)
                {
                    all = false;
                }
            }
            filterPopupContent[0].IsChecked = all ? true : new bool?();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            bool all = true;
            for(int x = 1; x < filterPopupContent.Count; x++)
            {
                CheckBox thisCheck = filterPopupContent[x];
                if (thisCheck.IsChecked != false)
                {
                    all = false;
                }
            }
            filterPopupContent[0].IsChecked = all ? false : new bool?();
        }
        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            Button filterButton = (Button)sender;
            int columnIndex = ((DataGridColumnHeader)filterButton.TemplatedParent).DisplayIndex;
            FilterValue thisColumnFilter = new FilterValue();
            foreach (FilterValue thisFilter in filterList)
            {
                if (thisFilter.PropertyName == ((Binding)((DataGridBoundColumn)this.dataGrid.Columns[columnIndex]).Binding).Path.Path.ToString())
                    thisColumnFilter = thisFilter;
            }
            List<string> columnValues = new List<string>();
            Popup filterPopup = (Popup)filterButton.Tag;
            for (int x = 1; x < filterPopupContent.Count; x = 1)
            {
                filterPopupContent.RemoveAt(x);
            }
            if (this.dataGrid.HasItems)
            {
                PropertyPath propertyPath = ((Binding)((DataGridBoundColumn)this.dataGrid.Columns[columnIndex]).Binding).Path;
                Type itemsType = this.dataGrid.Items[0].GetType();
                foreach (object item in ((ListCollectionView)this.dataGrid.ItemsSource).SourceCollection)
                {
                    if (item.GetType().ToString() != "MS.Internal.NamedObject")
                    {
                        string thisValue = itemsType.GetProperty(propertyPath.Path).GetMethod.Invoke(item, new object[] { }).ToString();
                        if (!columnValues.Contains(thisValue))
                            columnValues.Add(thisValue);
                    }
                }
                columnValues.Sort();
                foreach (string thisValue in columnValues)
                {
                    CheckBox tempCheck = new CheckBox();
                    tempCheck.Content = thisValue;
                    if (!thisColumnFilter.FilteredValues.Contains(thisValue))
                    {
                        tempCheck.IsChecked = true;
                    }
                    tempCheck.Checked += CheckBox_Checked;
                    tempCheck.Unchecked += CheckBox_Unchecked;
                    filterPopupContent.Add(tempCheck);
                }
            }
            filterPopup.IsOpen = true;
            if (filterPopupContent.Count > 1)
            {
                bool? all = true;
                for (int x = 1; x < filterPopupContent.Count; x++)
                {
                    CheckBox thisCheck = filterPopupContent[x];
                    if (thisCheck.IsChecked == false && all.HasValue && all.Value == true)
                    {
                        all = false;
                    }
                    if(thisCheck.IsChecked == true && all.HasValue && all.Value == false)
                    {
                        all = new bool?();
                    }
                }
                filterPopupContent[0].IsChecked = all;
            }
            else
            {
                filterPopupContent[0].IsChecked = true;
            }
        }

        private void FilterPopup_Closed(object sender, EventArgs e)
        {
            Popup filterPopup = (Popup)sender;
            int columnIndex = ((DataGridColumnHeader)filterPopup.TemplatedParent).DisplayIndex;
            FilterValue thisColumnFilter = new FilterValue();
            foreach (FilterValue thisFilter in filterList)
            {
                if (thisFilter.PropertyName == ((Binding)((DataGridBoundColumn)this.dataGrid.Columns[columnIndex]).Binding).Path.Path.ToString())
                    thisColumnFilter = thisFilter;
            }
            thisColumnFilter.FilteredValues = new List<string>();
            for (int x = 1; x < FilterPopupContent.Count; x++)
            {
                CheckBox tempCheck = FilterPopupContent[x];
                if (!tempCheck.IsChecked.HasValue || !tempCheck.IsChecked.Value)
                    thisColumnFilter.FilteredValues.Add(tempCheck.Content.ToString());

            }
            DataSource.Refresh();
        }
    }
    public class FilterValue
    {
        string propertyName;
        List<string> filteredValues;

        public FilterValue()
        {
            PropertyName = "";
            FilteredValues = new List<string>();
        }

        public FilterValue(string propertyName, List<string> permittedValues)
        {
            this.PropertyName = propertyName;
            this.FilteredValues = permittedValues;
        }

        public string PropertyName { get => propertyName; set => propertyName = value; }
        public List<string> FilteredValues { get => filteredValues; set => filteredValues = value; }
    }
    public class ListCollectionViewIListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            else if (!(value is ListCollectionView) && !(value is System.Collections.IList))
                throw new ArgumentException("value must be of type System.Windows.Data.ListCollectionView or System.Collections.IList", "value");
            else if (!targetType.IsAssignableFrom(typeof(System.Collections.IList)) && !targetType.IsAssignableFrom(typeof(ListCollectionView)))
                throw new ArgumentException("targetType must be assignable from System.Windows.Data.ListCollectionView or System.Collections.IList", "targetType");
            else
            {
                if(value is System.Collections.IList)
                {
                    if (targetType.IsAssignableFrom(typeof(ListCollectionView)))
                        return new ListCollectionView(value as System.Collections.IList);
                    else
                        return value as System.Collections.IList;
                }
                else
                {
                    return value as ListCollectionView;
                }

            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            else if (!(value is ListCollectionView) && !(value is System.Collections.IList))
                throw new ArgumentException("value must be of type System.Windows.Data.ListCollectionView or System.Collections.IList", "value");
            else if (!targetType.IsAssignableFrom(typeof(System.Collections.IList)) && !targetType.IsAssignableFrom(typeof(ListCollectionView)))
                throw new ArgumentException("targetType must be assignable from System.Windows.Data.ListCollectionView or System.Collections.IList", "targetType");
            else
            {
                if (value is System.Collections.IList)
                {
                    if (targetType.IsAssignableFrom(typeof(ListCollectionView)))
                        return new ListCollectionView(value as System.Collections.IList);
                    else
                        return value as System.Collections.IList;
                }
                else
                {
                    return value as ListCollectionView;
                }

            }
        }
    }
}
