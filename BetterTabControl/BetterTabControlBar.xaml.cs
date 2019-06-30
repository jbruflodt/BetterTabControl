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
    public partial class BetterTabControlBar : UserControl
    {
        public static readonly DependencyProperty NewTabDisplayTextProperty = DependencyProperty.RegisterAttached(
            "NewTabDisplayText",
            typeof(string),
            typeof(BetterTabControlBar),
            new FrameworkPropertyMetadata("+", FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnNewTabDisplayTextChanged))
            );
        public static readonly DependencyProperty RibbonColorProperty = DependencyProperty.RegisterAttached(
            "RibbonColor",
            typeof(Color),
            typeof(BetterTabControlBar),
            new FrameworkPropertyMetadata(SystemColors.HighlightColor, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnRibbonColorChanged))
            );
        public static readonly DependencyProperty TabsPropertty = DependencyProperty.RegisterAttached(
            "Tabs",
            typeof(SortedSet<Tab>),
            typeof(BetterTabControlBar),
            new FrameworkPropertyMetadata(new SortedSet<Tab>(), FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender)
            );
        public static readonly DependencyProperty BarBackgroundColorProperty = DependencyProperty.RegisterAttached(
            "BarBackgroundColor",
            typeof(Color),
            typeof(BetterTabControlBar),
            new FrameworkPropertyMetadata(SystemColors.MenuBarColor, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnBarBackgroundColorChanged))
            );
        public static readonly DependencyProperty TabBackgroundColorProperty = DependencyProperty.RegisterAttached(
            "TabBackgroundColor",
            typeof(Color),
            typeof(BetterTabControlBar),
            new FrameworkPropertyMetadata(SystemColors.MenuBarColor, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnTabBackgroundColorChanged))
            );
        public static readonly DependencyProperty TabTextColorProperty = DependencyProperty.RegisterAttached(
            "TabTextColor",
            typeof(Color),
            typeof(BetterTabControlBar),
            new FrameworkPropertyMetadata(SystemColors.ControlTextColor, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnTabTextColorChanged))
            );
        public static readonly DependencyProperty SelectedTabBackgroundColorProperty = DependencyProperty.RegisterAttached(
            "SelectedTabBackgroundColo",
            typeof(Color),
            typeof(BetterTabControlBar),
            new FrameworkPropertyMetadata(SystemColors.ControlDarkDarkColor, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnSelectedTabBackgroundColorChanged))
            );
        public static readonly DependencyProperty SelectedTabTextColorProperty = DependencyProperty.RegisterAttached(
            "SelectedTabTextColor",
            typeof(Color),
            typeof(BetterTabControlBar),
            new FrameworkPropertyMetadata(Colors.White, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnSelectedTabTextColorChanged))
            );
        public static readonly DependencyProperty MouseOverTabBackgroundColorProperty = DependencyProperty.RegisterAttached(
            "MouseOverTabBackgroundColor",
            typeof(Color),
            typeof(BetterTabControlBar),
            new FrameworkPropertyMetadata(SystemColors.HighlightColor, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnMouseOverTabBackgroundColorChanged))
            );
        public static readonly DependencyProperty MouseOverTabTextColorProperty = DependencyProperty.RegisterAttached(
            "MouseOverTabTextColor",
            typeof(Color),
            typeof(BetterTabControlBar),
            new FrameworkPropertyMetadata(SystemColors.HighlightTextColor, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnMouseOverTabTextColorChanged))
            );
        public static readonly DependencyProperty MouseOverCloseTabTextColorProperty = DependencyProperty.RegisterAttached(
            "MouseOverCloseTabTextColor",
            typeof(Color),
            typeof(BetterTabControlBar),
            new FrameworkPropertyMetadata(Colors.Red, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnMouseOverCloseTabTextColorChanged))
            );
        public BetterTabControlBar()
        {
            InitializeComponent();
        }
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
        public List<Tab> Tabs
        {
            get
            {
                return ((SortedSet<Tab>)GetValue(TabsPropertty)).ToList();
            }
            set
            {
                SetValue(TabsPropertty, new SortedSet<Tab>(value));
            }
        }
        private static void OnNewTabDisplayTextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BetterTabControlBar tabControl = (BetterTabControlBar)sender;
            tabControl.Resources["NewTabDisplayText"] = e.NewValue;
        }
        private static void OnRibbonColorChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BetterTabControlBar tabControl = (BetterTabControlBar)sender;
            tabControl.Resources["RibbonColor"] = e.NewValue;
        }
        private static void OnBarBackgroundColorChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BetterTabControlBar tabControl = (BetterTabControlBar)sender;
            tabControl.Resources["BarBackgroundColor"] = e.NewValue;
        }
        private static void OnTabBackgroundColorChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BetterTabControlBar tabControl = (BetterTabControlBar)sender;
            tabControl.Resources["TabBackgroundColor"] = e.NewValue;
        }
        private static void OnTabTextColorChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BetterTabControlBar tabControl = (BetterTabControlBar)sender;
            tabControl.Resources["TabTextColor"] = e.NewValue;
        }
        private static void OnSelectedTabBackgroundColorChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BetterTabControlBar tabControl = (BetterTabControlBar)sender;
            tabControl.Resources["SelectedTabBackgroundColor"] = e.NewValue;
        }
        private static void OnSelectedTabTextColorChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BetterTabControlBar tabControl = (BetterTabControlBar)sender;
            tabControl.Resources["SelectedTabTextColor"] = e.NewValue;
        }
        private static void OnMouseOverTabBackgroundColorChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BetterTabControlBar tabControl = (BetterTabControlBar)sender;
            tabControl.Resources["MouseOverTabBackgroundColor"] = e.NewValue;
        }
        private static void OnMouseOverTabTextColorChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BetterTabControlBar tabControl = (BetterTabControlBar)sender;
            tabControl.Resources["MouseOverTabTextColor"] = e.NewValue;
        }
        private static void OnMouseOverCloseTabTextColorChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BetterTabControlBar tabControl = (BetterTabControlBar)sender;
            tabControl.Resources["MouseOverCloseTabTextColor"] = e.NewValue;
        }
    }
    public class BrushColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color color = Colors.Transparent;
            if(targetType == null)
            {
                throw new ArgumentNullException("targetType");
            }
            if(!targetType.GetTypeInfo().IsAssignableFrom(typeof(Color)) && !targetType.GetTypeInfo().IsAssignableFrom(typeof(SolidColorBrush)))
            {
                throw new ArgumentException("targetType must be assignable from Color or SolidColorBrush", "targetType");
            }
            if(value == null)
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
                return color;
            else
                return new SolidColorBrush(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
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
                return color;
            else
                return new SolidColorBrush(color);
        }
    }
}
