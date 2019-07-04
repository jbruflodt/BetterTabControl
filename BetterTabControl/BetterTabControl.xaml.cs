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
        private Tab draggedTab;
        private bool doingDragDrop;
        private Point? dragStart;
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
        public SolidColorBrush RibbonColor
        {
            get { return (SolidColorBrush)GetValue(RibbonColorProperty); }
            set { SetValue(RibbonColorProperty, value); }
        }
        public SolidColorBrush BarBackgroundColor
        {
            get { return (SolidColorBrush)GetValue(BarBackgroundColorProperty); }
            set { SetValue(BarBackgroundColorProperty, value); }
        }
        public SolidColorBrush TabBackgroundColor
        {
            get { return (SolidColorBrush)GetValue(TabBackgroundColorProperty); }
            set { SetValue(TabBackgroundColorProperty, value); }
        }
        public SolidColorBrush TabTextColor
        {
            get { return (SolidColorBrush)GetValue(TabTextColorProperty); }
            set { SetValue(TabTextColorProperty, value); }
        }
        public SolidColorBrush SelectedTabBackgroundColor
        {
            get { return (SolidColorBrush)GetValue(SelectedTabBackgroundColorProperty); }
            set { SetValue(SelectedTabBackgroundColorProperty, value); }
        }
        public SolidColorBrush SelectedTabTextColor
        {
            get { return (SolidColorBrush)GetValue(SelectedTabTextColorProperty); }
            set { SetValue(SelectedTabTextColorProperty, value); }
        }
        public SolidColorBrush MouseOverTabBackgroundColor
        {
            get { return (SolidColorBrush)GetValue(MouseOverTabBackgroundColorProperty); }
            set { SetValue(MouseOverTabBackgroundColorProperty, value); }
        }
        public SolidColorBrush MouseOverTabTextColor
        {
            get { return (SolidColorBrush)GetValue(MouseOverTabTextColorProperty); }
            set { SetValue(MouseOverTabTextColorProperty, value); }
        }
        public SolidColorBrush MouseOverCloseTabTextColor
        {
            get { return (SolidColorBrush)GetValue(MouseOverCloseTabTextColorProperty); }
            set { SetValue(MouseOverCloseTabTextColorProperty, value); }
        }
        public Tab SelectedTab
        {
            get
            {
                foreach (Tab tempTab in Tabs)
                {
                    if (tempTab.Selected)
                        return tempTab;
                }
                return null;
            }
        }
        public int SelectedIndex
        {
            get
            {
                for(int x = 0; x < Tabs.Count; x++)
                {
                    Tab tempTab = Tabs[x];
                    if (tempTab.Selected)
                        return x;
                }
                return -1;
            }
        }
        public Control SelectedContent
        {
            get
            {
                foreach (Tab tempTab in Tabs)
                {
                    if (tempTab.Selected)
                        return tempTab.TabContent;
                }
                return null;
            }
        }
        public Type DefaultContentType
        {
            get
            {
                return defaultContentType;
            }
            set
            {
                if (!value.GetTypeInfo().IsSubclassOf(typeof(Control)) && value != typeof(Control))
                {
                    throw new ArgumentException("DefaultContentType must be of type System.Windows.Controls.Control or derived from it");
                }
                else
                {
                    if (value.GetConstructor(new Type[] { }) == null)
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
                SetValue(TabsPropertty, value);
            }
        }
        public event EventHandler NewTabClick;
        public event EventHandler AllTabsClosed;
        public BetterTabControl()
        {
            InitializeComponent();
            Tabs.CollectionChanged += TabsCollectionChanged;
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
            ReindexTabs();
            if(e.Action == NotifyCollectionChangedAction.Remove)
            {
                if(e.OldItems.Count > 0)
                {
                    foreach(Tab thisTab in e.OldItems)
                    {
                        if(thisTab.Selected)
                        {
                            thisTab.SetSelected(false);
                            if (thisTab.DisplayIndex > 0 && Tabs.Count > thisTab.DisplayIndex - 1)
                                Tabs[thisTab.DisplayIndex - 1].SetSelected(true);
                            else if (thisTab.DisplayIndex == 0 && Tabs.Count > 0)
                                Tabs[0].SetSelected(true);
                            else if (Tabs.Count > 0)
                                Tabs[Tabs.Count - 1].SetSelected(true);
                        }
                    }
                }
            }
        }
        private void TabsItemChanged(object sender, PropertyChangedEventArgs e)
        {
            ReindexTabs();
        }
        private void ReindexTabs()
        {
            if (!doingDragDrop)
            {
                Tabs.OrderBy((thisTab) => thisTab, new TabComparer());
                for (int x = 0; x < Tabs.Count; x++)
                {
                    Tabs[x].DisplayIndex = x;
                }
            }
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
        public void ClearSelected()
        {
            foreach (Tab tempTab in Tabs)
            {
                if (tempTab.Selected)
                    tempTab.SetSelected(false);
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Tab thisTab = (Tab)((FrameworkElement)sender).DataContext;
            CancelableTabEventArgs eventArgs = new CancelableTabEventArgs();
            thisTab.OnCloseButtonClick(eventArgs);
            if (!eventArgs.Cancel)
            {
                thisTab.Close();
                this.Tabs.Remove(thisTab);
                if (Tabs.Count <= 0)
                {
                    if (AllTabsClosed != null)
                        AllTabsClosed(this, new EventArgs());
                    else
                        AddNewTab();
                }
            }
        }

        private void TabBackground_MouseEnter(object sender, MouseEventArgs e)
        {
            Tab thisTab = (Tab)((FrameworkElement)sender).DataContext;
            if (draggedTab != null)
            {
                int draggedIndex = draggedTab.DisplayIndex;
                doingDragDrop = false;
                draggedTab.DisplayIndex = thisTab.DisplayIndex;
                doingDragDrop = true;
                thisTab.DisplayIndex = draggedIndex;
            }
        }

        private void BetterTabControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Tabs.Count <= 0)
                AddNewTab();
        }

        private void TabButton_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Button senderButton = (Button)sender;
            Tab thisTab = (Tab)senderButton.DataContext;
            if (senderButton.IsEnabled)
            {
                CancelableTabEventArgs eventArgs = new CancelableTabEventArgs();
                thisTab.OnSelected(eventArgs);
                if (!eventArgs.Cancel)
                {
                    if (!thisTab.Selected)
                    {
                        foreach (Tab tempTab in Tabs)
                        {
                            if (tempTab.Selected)
                                tempTab.SetSelected(false);
                        }
                        thisTab.SetSelected(true);
                    }
                    draggedTab = thisTab;
                    dragStart = e.GetPosition(null);
                }
            }
        }

        private void BetterTabControl_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            draggedTab = null;
            dragStart = null;
        }

        private void BetterTabControl_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                draggedTab = null;
                dragStart = null;
            }
        }

        private double DragDistance(Point start, Point end)
        {
            return Math.Sqrt(Math.Pow((end.X - start.X), 2) + Math.Pow((end.Y - start.Y), 2));
        }
        private void TabsPanel_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            IInputElement inputElement = tabsPanel.InputHitTest(e.GetPosition(tabsPanel));
            if (dragStart != null && DragDistance(dragStart.Value, e.GetPosition(null)) > SystemParameters.MinimumHorizontalDragDistance)
            {
                if (draggedTab != null)
                {
                    doingDragDrop = true;
                    Tabs.Remove(draggedTab);
                    DragDropEffects dragResult = DragDrop.DoDragDrop(this, draggedTab, DragDropEffects.Move);
                    doingDragDrop = false;
                    if (dragResult == DragDropEffects.None)
                    {
                        Tabs.Add(draggedTab);
                    }
                    else
                    {
                        Tabs.OrderBy((thisTab) => thisTab, new TabComparer());
                        for (int x = 0; x < Tabs.Count; x++)
                        {
                            Tabs[x].DisplayIndex = x;
                        }
                    }
                    draggedTab = null;
                }
            }
        }

        private void TabBackground_PreviewDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Tab)))
            {
                Tab thisTab = (Tab)((FrameworkElement)sender).DataContext;
                Tab localDraggedTab = (Tab)e.Data.GetData(typeof(Tab));
                localDraggedTab.DisplayIndex = thisTab.DisplayIndex;
                ClearSelected();
                localDraggedTab.SetSelected(true);
                if (!Tabs.Contains(localDraggedTab))
                    Tabs.Add(localDraggedTab);
                e.Effects = DragDropEffects.Move;
            }
        }

        private void TabsGrid_PreviewDrop(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
        }

        private void TabsGrid_PreviewDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Tab)))
            {
                IInputElement hitElement = tabsGrid.InputHitTest(e.GetPosition(tabsGrid));
                if (hitElement == newTab || hitElement == tabsGrid)
                {
                    Tab localDraggedTab = (Tab)e.Data.GetData(typeof(Tab));
                    localDraggedTab.DisplayIndex = Tabs.Count - 1;
                    ClearSelected();
                    localDraggedTab.SetSelected(true);
                    if(!Tabs.Contains(localDraggedTab))
                        Tabs.Add(localDraggedTab);
                    e.Effects = DragDropEffects.Move;
                }
            }
        }

        private void TabsGrid_PreviewDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Tab)))
            {
                e.Effects = DragDropEffects.Move;
            }
        }

        private void TabsGrid_PreviewDragLeave(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Tab)))
            {
                Tab localDraggedTab = (Tab)e.Data.GetData(typeof(Tab));
                if (Tabs.Contains(localDraggedTab))
                {
                    Tabs.Remove(localDraggedTab);
                }
            }
        }
    }
    public class CancelableTabEventArgs : EventArgs
    {
        private bool cancel;

        public CancelableTabEventArgs()
        {
            cancel = false;
        }

        public bool Cancel { get => cancel; set => cancel = value; }
    }
    public delegate void CancelableTabEventHandler(object sender, CancelableTabEventArgs e);
    public class ChangeColorBrightness : IValueConverter
    {
        /// <summary>
        /// Creates color with corrected brightness.
        /// </summary>
        /// <param name="color">Color to correct.</param>
        /// <param name="correctionFactor">The brightness correction factor. Must be between -1 and 1. 
        /// Negative values produce darker colors.</param>
        /// <returns>
        /// Corrected <see cref="Color"/> structure.
        /// </returns>
        public Color ChangeColor(Color color, float correctionFactor)
        {
            float red = (float)color.R;
            float green = (float)color.G;
            float blue = (float)color.B;

            if (correctionFactor < 0)
            {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }
            else
            {
                red = (255 - red) * correctionFactor + red;
                green = (255 - green) * correctionFactor + green;
                blue = (255 - blue) * correctionFactor + blue;
            }

            return Color.FromArgb(color.A, (byte)red, (byte)green, (byte)blue);
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
