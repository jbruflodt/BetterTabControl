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
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace BetterTabs
{
    /// <summary>
    /// Interaction logic for BetterTabs.xaml
    /// </summary>
    public partial class BetterTabControl : UserControl
    {
        private Type defaultContentType;
        public static readonly DependencyProperty NewTabDisplayTextProperty = DependencyProperty.RegisterAttached(
            "NewTabDisplayText",
            typeof(string),
            typeof(BetterTabControl),
            new FrameworkPropertyMetadata("+", FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnNewTabDisplayTextChanged))
            );
        public static readonly DependencyProperty RibbonColorProperty = DependencyProperty.RegisterAttached(
            "RibbonColor",
            typeof(SolidColorBrush),
            typeof(BetterTabControl),
            new FrameworkPropertyMetadata(SystemColors.HighlightBrush, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnRibbonColorChanged))
            );
        public static readonly DependencyProperty TabsPropertty = DependencyProperty.RegisterAttached(
            "Tabs",
            typeof(ObservableCollection<Tab>),
            typeof(BetterTabControl),
            new FrameworkPropertyMetadata(new ObservableCollection<Tab>(), FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender)
            );
        public static readonly DependencyProperty BarBackgroundColorProperty = DependencyProperty.RegisterAttached(
            "BarBackgroundColor",
            typeof(SolidColorBrush),
            typeof(BetterTabControl),
            new FrameworkPropertyMetadata(SystemColors.MenuBarBrush, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnBarBackgroundColorChanged))
            );
        public static readonly DependencyProperty TabBackgroundColorProperty = DependencyProperty.RegisterAttached(
            "TabBackgroundColor",
            typeof(SolidColorBrush),
            typeof(BetterTabControl),
            new FrameworkPropertyMetadata(SystemColors.MenuBarBrush, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnTabBackgroundColorChanged))
            );
        public static readonly DependencyProperty TabTextColorProperty = DependencyProperty.RegisterAttached(
            "TabTextColor",
            typeof(SolidColorBrush),
            typeof(BetterTabControl),
            new FrameworkPropertyMetadata(SystemColors.ControlTextBrush, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnTabTextColorChanged))
            );
        public static readonly DependencyProperty SelectedTabBackgroundColorProperty = DependencyProperty.RegisterAttached(
            "SelectedTabBackgroundColor",
            typeof(SolidColorBrush),
            typeof(BetterTabControl),
            new FrameworkPropertyMetadata(SystemColors.ControlDarkDarkBrush, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnSelectedTabBackgroundColorChanged))
            );
        public static readonly DependencyProperty SelectedTabTextColorProperty = DependencyProperty.RegisterAttached(
            "SelectedTabTextColor",
            typeof(SolidColorBrush),
            typeof(BetterTabControl),
            new FrameworkPropertyMetadata(Brushes.White, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnSelectedTabTextColorChanged))
            );
        public static readonly DependencyProperty MouseOverTabBackgroundColorProperty = DependencyProperty.RegisterAttached(
            "MouseOverTabBackgroundColor",
            typeof(SolidColorBrush),
            typeof(BetterTabControl),
            new FrameworkPropertyMetadata(SystemColors.HighlightBrush, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnMouseOverTabBackgroundColorChanged))
            );
        public static readonly DependencyProperty MouseOverTabTextColorProperty = DependencyProperty.RegisterAttached(
            "MouseOverTabTextColor",
            typeof(SolidColorBrush),
            typeof(BetterTabControl),
            new FrameworkPropertyMetadata(SystemColors.HighlightTextBrush, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnMouseOverTabTextColorChanged))
            );
        public static readonly DependencyProperty MouseOverCloseTabTextColorProperty = DependencyProperty.RegisterAttached(
            "MouseOverCloseTabTextColor",
            typeof(SolidColorBrush),
            typeof(BetterTabControl),
            new FrameworkPropertyMetadata(Brushes.Red, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnMouseOverCloseTabTextColorChanged))
            );
        public string NewTabDisplayText
        {
            get { return (string)GetValue(NewTabDisplayTextProperty); }
            set { SetValue(NewTabDisplayTextProperty, value); }
        }
        public Color RibbonColor
        {
            get { return (Color)GetValue(RibbonColorProperty); }
            set { SetValue(RibbonColorProperty, value); }
        }
        public Color BarBackgroundColor
        {
            get { return (Color)GetValue(BarBackgroundColorProperty); }
            set { SetValue(BarBackgroundColorProperty, value); }
        }
        public Color TabBackgroundColor
        {
            get { return (Color)GetValue(TabBackgroundColorProperty); }
            set { SetValue(TabBackgroundColorProperty, value); }
        }
        public Color TabTextColor
        {
            get { return (Color)GetValue(TabTextColorProperty); }
            set { SetValue(TabTextColorProperty, value); }
        }
        public Color SelectedTabBackgroundColor
        {
            get { return (Color)GetValue(SelectedTabBackgroundColorProperty); }
            set { SetValue(SelectedTabBackgroundColorProperty, value); }
        }
        public Color SelectedTabTextColor
        {
            get { return (Color)GetValue(SelectedTabTextColorProperty); }
            set { SetValue(SelectedTabTextColorProperty, value); }
        }
        public Color MouseOverTabBackgroundColor
        {
            get { return (Color)GetValue(MouseOverTabBackgroundColorProperty); }
            set { SetValue(MouseOverTabBackgroundColorProperty, value); }
        }
        public Color MouseOverTabTextColor
        {
            get { return (Color)GetValue(MouseOverTabTextColorProperty); }
            set { SetValue(MouseOverTabTextColorProperty, value); }
        }
        public Color MouseOverCloseTabTextColor
        {
            get { return (Color)GetValue(MouseOverCloseTabTextColorProperty); }
            set { SetValue(MouseOverCloseTabTextColorProperty, value); }
        }
        public Type DefaultContentType
        {
            get
            {
                return defaultContentType;
            }
            set
            {
                if(!value.GetTypeInfo().IsSubclassOf(typeof(Control)) && value != typeof(Control))
                {
                    throw new ArgumentException("DefaultContentType must be of type System.Windows.Controls.Control or derived from it");
                }
                else
                {
                    if(value.GetConstructor(new Type[] { }) == null)
                        throw new ArgumentException("DefaultContentType must be of a type that has a parameterless constructor");
                    else
                        defaultContentType = value;
                }
            }
        }
        public ObservableCollection<Tab> Tabs
        {
            get
            {
                return (ObservableCollection<Tab>)GetValue(TabsPropertty);
            }
            set
            {
                SetValue(TabsPropertty,value);
            }
        }
        public event EventHandler NewTabClick;
        public BetterTabControl()
        {
            InitializeComponent();
        }
        private static void OnNewTabDisplayTextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BetterTabControl tabControl = (BetterTabControl)sender;
            tabControl.Resources["NewTabDisplayText"] = e.NewValue;
        }
        private static void OnRibbonColorChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BetterTabControl tabControl = (BetterTabControl)sender;
            tabControl.Resources["RibbonColor"] = e.NewValue;
        }
        private static void OnBarBackgroundColorChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BetterTabControl tabControl = (BetterTabControl)sender;
            tabControl.Resources["BarBackgroundColor"] = e.NewValue;
        }
        private static void OnTabBackgroundColorChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BetterTabControl tabControl = (BetterTabControl)sender;
            tabControl.Resources["TabBackgroundColor"] = e.NewValue;
        }
        private static void OnTabTextColorChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BetterTabControl tabControl = (BetterTabControl)sender;
            tabControl.Resources["TabTextColor"] = e.NewValue;
        }
        private static void OnSelectedTabBackgroundColorChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BetterTabControl tabControl = (BetterTabControl)sender;
            tabControl.Resources["SelectedTabBackgroundColor"] = e.NewValue;
        }
        private static void OnSelectedTabTextColorChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BetterTabControl tabControl = (BetterTabControl)sender;
            tabControl.Resources["SelectedTabTextColor"] = e.NewValue;
        }
        private static void OnMouseOverTabBackgroundColorChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BetterTabControl tabControl = (BetterTabControl)sender;
            tabControl.Resources["MouseOverTabBackgroundColor"] = e.NewValue;
        }
        private static void OnMouseOverTabTextColorChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BetterTabControl tabControl = (BetterTabControl)sender;
            tabControl.Resources["MouseOverTabTextColor"] = e.NewValue;
        }
        private static void OnMouseOverCloseTabTextColorChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BetterTabControl tabControl = (BetterTabControl)sender;
            tabControl.Resources["MouseOverCloseTabTextColor"] = e.NewValue;
        }
        private void TabsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Tabs.OrderBy((thisTab) => thisTab, new TabComparer());
        }
        private void TabsItemChanged(object sender, PropertyChangedEventArgs e)
        {
            Tabs.OrderBy((thisTab) => thisTab, new TabComparer());
        }

        private void NewTab_Click(object sender, RoutedEventArgs e)
        {
            AddNewTab();
            NewTabClick?.Invoke(this, new EventArgs());
        }
        public void AddNewTab()
        {
            Tab addedTab = new Tab();
            if (DefaultContentType != null)
                addedTab.TabContent = (Control)DefaultContentType.GetConstructor(new Type[] { }).Invoke(new object[] { });
            Tabs.Add(addedTab);
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
            float changeValue = 0;
            if(!parameter.GetType().GetTypeInfo().IsAssignableFrom(typeof(float)) && !float.TryParse(parameter.ToString(), out changeValue))
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
                return ChangeColor(color, changeValue);
            else
                return new SolidColorBrush(ChangeColor(color, changeValue));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            float changeValue = 0;
            if (!parameter.GetType().GetTypeInfo().IsAssignableFrom(typeof(float)) && !float.TryParse(parameter.ToString(), out changeValue))
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
                return ChangeColor(color, changeValue * -1);
            else
                return new SolidColorBrush(ChangeColor(color, changeValue * -1));
        }
    }
}
