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
using System.Collections;

namespace BetterTabs
{
    /// <summary>
    /// Interaction logic for BetterTabControl.xaml
    /// </summary>
    public partial class BetterTabControl : UserControl, INotifyPropertyChanged
    {
        public static RoutedUICommand NextTabCommand = new RoutedUICommand("Next Tab", "NextTab", typeof(BetterTabControl), new InputGestureCollection(new InputGestureCollection { new KeyGesture(Key.Tab, ModifierKeys.Control) }));
        public static RoutedUICommand PreviousTabCommand = new RoutedUICommand("Previous Tab", "PreviousTab", typeof(BetterTabControl), new InputGestureCollection(new InputGestureCollection { new KeyGesture(Key.Tab, ModifierKeys.Control | ModifierKeys.Shift) }));
        private Type defaultContentType;
        private Tab draggedTab;
        private bool doingDragDrop;
        private Point? dragStart;
        private bool indexing;
        private List<DependencyObject> hitResults;
        public static readonly DependencyProperty NewTabDisplayTextProperty = DependencyProperty.Register(
            "NewTabDisplayText",
            typeof(string),
            typeof(BetterTabControl),
            new FrameworkPropertyMetadata("+", FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender)
            );
        public static readonly DependencyProperty RibbonColorProperty = DependencyProperty.Register(
            "RibbonColor",
            typeof(Brush),
            typeof(BetterTabControl),
            new FrameworkPropertyMetadata(SystemColors.HighlightBrush, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender)
            );
        public static readonly DependencyProperty TabsPropertty = DependencyProperty.Register(
            "Tabs",
            typeof(TabCollection),
            typeof(BetterTabControl),
            new FrameworkPropertyMetadata(new TabCollection(), FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender)
            );
        public static readonly DependencyProperty BarBackgroundColorProperty = DependencyProperty.Register(
            "BarBackgroundColor",
            typeof(Brush),
            typeof(BetterTabControl),
            new FrameworkPropertyMetadata(SystemColors.MenuBarBrush, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender)
            );
        public static readonly DependencyProperty TabBackgroundColorProperty = DependencyProperty.Register(
            "TabBackgroundColor",
            typeof(Brush),
            typeof(BetterTabControl),
            new FrameworkPropertyMetadata(SystemColors.MenuBarBrush, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender)
            );
        public static readonly DependencyProperty TabTextColorProperty = DependencyProperty.Register(
            "TabTextColor",
            typeof(Brush),
            typeof(BetterTabControl),
            new FrameworkPropertyMetadata(SystemColors.ControlTextBrush, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender)
            );
        public static readonly DependencyProperty SelectedTabBackgroundColorProperty = DependencyProperty.Register(
            "SelectedTabBackgroundColor",
            typeof(Brush),
            typeof(BetterTabControl),
            new FrameworkPropertyMetadata(SystemColors.HighlightBrush, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender)
            );
        public static readonly DependencyProperty SelectedTabTextColorProperty = DependencyProperty.Register(
            "SelectedTabTextColor",
            typeof(Brush),
            typeof(BetterTabControl),
            new FrameworkPropertyMetadata(Brushes.White, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender)
            );
        public static readonly DependencyProperty MouseOverTabBackgroundColorProperty = DependencyProperty.Register(
            "MouseOverTabBackgroundColor",
            typeof(Brush),
            typeof(BetterTabControl),
            new FrameworkPropertyMetadata(SystemColors.HighlightBrush, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender)
            );
        public static readonly DependencyProperty MouseOverTabTextColorProperty = DependencyProperty.Register(
            "MouseOverTabTextColor",
            typeof(Brush),
            typeof(BetterTabControl),
            new FrameworkPropertyMetadata(SystemColors.HighlightTextBrush, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender)
            );
        public static readonly DependencyProperty MouseOverCloseTabTextColorProperty = DependencyProperty.Register(
            "MouseOverCloseTabTextColor",
            typeof(Brush),
            typeof(BetterTabControl),
            new FrameworkPropertyMetadata(Brushes.Red, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender)
            );
        public static readonly DependencyProperty CloseButtonContentProperty = DependencyProperty.Register(
            "CloseButtonContent",
            typeof(object),
            typeof(BetterTabControl),
            new FrameworkPropertyMetadata("X", FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender)
            );
        public string NewTabDisplayText
        {
            get { return (string)GetValue(NewTabDisplayTextProperty); }
            set { SetValue(NewTabDisplayTextProperty, value); }
        }
        public Brush RibbonColor
        {
            get { return (Brush)GetValue(RibbonColorProperty); }
            set { SetValue(RibbonColorProperty, value); }
        }
        public Brush BarBackgroundColor
        {
            get { return (Brush)GetValue(BarBackgroundColorProperty); }
            set { SetValue(BarBackgroundColorProperty, value); }
        }
        public Brush TabBackgroundColor
        {
            get { return (Brush)GetValue(TabBackgroundColorProperty); }
            set { SetValue(TabBackgroundColorProperty, value); }
        }
        public Brush TabTextColor
        {
            get { return (Brush)GetValue(TabTextColorProperty); }
            set { SetValue(TabTextColorProperty, value); }
        }
        public Brush SelectedTabBackgroundColor
        {
            get { return (Brush)GetValue(SelectedTabBackgroundColorProperty); }
            set { SetValue(SelectedTabBackgroundColorProperty, value); }
        }
        public Brush SelectedTabTextColor
        {
            get { return (Brush)GetValue(SelectedTabTextColorProperty); }
            set { SetValue(SelectedTabTextColorProperty, value); }
        }
        public Brush MouseOverTabBackgroundColor
        {
            get { return (Brush)GetValue(MouseOverTabBackgroundColorProperty); }
            set { SetValue(MouseOverTabBackgroundColorProperty, value); }
        }
        public Brush MouseOverTabTextColor
        {
            get { return (Brush)GetValue(MouseOverTabTextColorProperty); }
            set { SetValue(MouseOverTabTextColorProperty, value); }
        }
        public Brush MouseOverCloseTabTextColor
        {
            get { return (Brush)GetValue(MouseOverCloseTabTextColorProperty); }
            set { SetValue(MouseOverCloseTabTextColorProperty, value); }
        }
        public object CloseButtonContent
        {
            get { return (string)GetValue(CloseButtonContentProperty); }
            set { SetValue(CloseButtonContentProperty, value); }
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
            set
            {
                try
                {
                    ChangeSelectedTab(value);
                }
                catch
                {
                    throw;
                }
            }
        }
        public int SelectedIndex
        {
            get
            {
                for (int x = 0; x < Tabs.Count; x++)
                {
                    Tab tempTab = Tabs[x];
                    if (tempTab.Selected)
                        return x;
                }
                return -1;
            }
            set
            {
                try
                {
                    ChangeSelectedTab(value);
                }
                catch
                {
                    throw;
                }
            }
        }
        public UIElement SelectedContent
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
        public TabCollection Tabs
        {
            get
            {
                return (TabCollection)GetValue(TabsPropertty);
            }
            set
            {
                SetValue(TabsPropertty, value);
            }
        }
        public event EventHandler AddingNewTab;
        public event EventHandler AddedNewTab;
        public event EventHandler AllTabsClosed;
        public event SelectedTabChangingEventHandler SelectedTabChanging;
        public event SelectedTabChangedEventHandler SelectedTabChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public BetterTabControl()
        {
            InitializeComponent();
            Tabs.CollectionChanged += TabsCollectionChanged;
        }
        protected virtual bool OnSelectedTabChanging(Tab oldSelection, Tab newSelection)
        {
            SelectedTabChangingEventArgs eventArgs = new SelectedTabChangingEventArgs(oldSelection, newSelection);
            SelectedTabChanging?.Invoke(this, eventArgs);
            return eventArgs.Cancel;
        }
        protected virtual void OnSelectedTabChanged(Tab oldSelection, Tab newSelection)
        {
            SelectedTabChanged?.Invoke(this, new SelectedTabChangedEventArgs(oldSelection, newSelection));
        }
        private void NotifySelectedChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedTab"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedContent"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedIndex"));
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void ChangeSelectedTab(Tab tab)
        {
            if (Tabs.Contains(tab))
            {
                Tab oldSelection = SelectedTab;
                if (!OnSelectedTabChanging(oldSelection, tab))
                {
                    ClearSelected();
                    tab.SetSelected(true);
                    OnSelectedTabChanged(oldSelection, tab);
                    NotifySelectedChanged();
                }
            }
            else
            {
                throw new ArgumentException("tab is not in this BetterTabControl");
            }
        }
        private void ChangeSelectedTab(int index)
        {
            ChangeSelectedTab(Tabs[index]);
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
            if (!indexing)
            {
                indexing = true;
                //Tabs.OrderBy((thisTab) => thisTab, new TabComparer());
                for (int x = 0; x < Tabs.Count; x++)
                {
                    Tabs[x].DisplayIndex = x;
                }
            }
        }
        private void NewTab_Click(object sender, RoutedEventArgs e)
        {
            AddNewTab();
        }
        public void AddNewTab()
        {
            AddingNewTab?.Invoke(this, new EventArgs());
            if (AddingNewTab == null)
            {
                Tab addedTab = new Tab();
                if (DefaultContentType != null)
                    addedTab.TabContent = (Control)DefaultContentType.GetConstructor(new Type[] { }).Invoke(new object[] { });
                Tabs.Add(addedTab);
            }
            AddedNewTab?.Invoke(this, new EventArgs());
        }
        private void ClearSelected()
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
            /*Tab thisTab = (Tab)((FrameworkElement)sender).DataContext;
            if (draggedTab != null)
            {
                int draggedIndex = draggedTab.DisplayIndex;
                doingDragDrop = false;
                draggedTab.DisplayIndex = thisTab.DisplayIndex;
                doingDragDrop = true;
                thisTab.DisplayIndex = draggedIndex;
            }*/
        }

        private void BetterTabControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Tabs.Count <= 0)
                AddNewTab();
            if (SelectedTab == null)
                ChangeSelectedTab(0);

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
                if (draggedTab != null && !doingDragDrop)
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
                if (thisTab.ID != localDraggedTab.ID)
                {
                    Tabs.Move(Tabs.IndexOf(localDraggedTab), Tabs.IndexOf(thisTab));
                    UpdateLayout();
                }
                e.Handled = true;
            }
        }


        private void NewTab_PreviewDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Tab)))
            {
                Tab localDraggedTab = (Tab)e.Data.GetData(typeof(Tab));
                Tabs.Move(Tabs.IndexOf(localDraggedTab), Tabs.Count - 1);
                UpdateLayout();
                e.Handled = true;
            }
        }

        private void BaseGrid_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            //Console.WriteLine(baseGrid.InputHitTest(e.GetPosition(baseGrid)));
        }

        HitTestResultBehavior HitTestResultCallback(HitTestResult result)
        {
            hitResults.Add(result.VisualHit);
            return HitTestResultBehavior.Continue;
        }

        private void Filler_PreviewDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Tab)))
            {
                Tab localDraggedTab = (Tab)e.Data.GetData(typeof(Tab));
                Tabs.Move(Tabs.IndexOf(localDraggedTab), Tabs.Count - 1);
                UpdateLayout();
                e.Handled = true;
            }
        }

        protected override void OnPreviewDragEnter(DragEventArgs e)
        {
            base.OnPreviewDragEnter(e);
            if (e.Data.GetDataPresent(typeof(Tab)))
            {
                Tab localDraggedTab = (Tab)e.Data.GetData(typeof(Tab));
                ClearSelected();
                localDraggedTab.SetSelected(true);
                if (!Tabs.Contains(localDraggedTab))
                    Tabs.Add(localDraggedTab);
                e.Handled = true;
            }
        }

        protected override void OnPreviewDragLeave(DragEventArgs e)
        {
            base.OnPreviewDragLeave(e);
            if (e.Data.GetDataPresent(typeof(Tab)))
            {
                hitResults = new List<DependencyObject>();
                VisualTreeHelper.HitTest(this, null, HitTestResultCallback, new PointHitTestParameters(e.GetPosition(this)));
                bool validLeave = true;
                foreach(DependencyObject thisHit in hitResults)
                {
                    DependencyObject testHit = VisualTreeHelper.GetParent(thisHit);
                    while(testHit != null)
                    {
                        if(testHit == this)
                        {
                            validLeave = false;
                            break;
                        }
                        testHit = VisualTreeHelper.GetParent(testHit);
                    }
                    if (!validLeave)
                        break;
                }
                if (validLeave)
                {
                    Tab localDraggedTab = (Tab)e.Data.GetData(typeof(Tab));
                    if (Tabs.Contains(localDraggedTab))
                    {
                        Tabs.Remove(localDraggedTab);
                    }

                    e.Handled = true;
                }

            }
        }

        protected override void OnPreviewDrop(DragEventArgs e)
        {
            base.OnPreviewDrop(e);
            if (e.Data.GetDataPresent(typeof(Tab)))
            {
                e.Effects = DragDropEffects.Move;
                e.Handled = true;
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
    public class SelectedTabChangingEventArgs : CancelableTabEventArgs
    {
        public SelectedTabChangingEventArgs(Tab oldSelection, Tab newSelection) : base()
        {
            OldSelection = oldSelection;
            NewSelection = newSelection;
        }

        public Tab OldSelection { get; set; }
        public Tab NewSelection { get; set; }
    }
    public delegate void SelectedTabChangingEventHandler(object sender, SelectedTabChangingEventArgs e);
    public class SelectedTabChangedEventArgs : EventArgs
    {
        public SelectedTabChangedEventArgs(Tab oldSelection, Tab newSelection) : base()
        {
            OldSelection = oldSelection;
            NewSelection = newSelection;
        }

        public Tab OldSelection { get; set; }
        public Tab NewSelection { get; set; }
    }
    public delegate void SelectedTabChangedEventHandler(object sender, SelectedTabChangedEventArgs e);
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
                if(targetType.GetTypeInfo().IsAssignableFrom(value.GetType()))
                {
                    return value;
                }
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
                if (targetType.GetTypeInfo().IsAssignableFrom(value.GetType()))
                {
                    return value;
                }
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
                if (targetType.GetTypeInfo().IsAssignableFrom(value.GetType()))
                {
                    return value;
                }
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
                if (targetType.GetTypeInfo().IsAssignableFrom(value.GetType()))
                {
                    return value;
                }
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
    public sealed class TabCollection : INotifyPropertyChanged, INotifyCollectionChanged, IList<Tab>, ICollection<Tab>, IEnumerable<Tab>
    {
        private ObservableCollection<Tab> collection;

        public TabCollection()
        {
            this.collection = new ObservableCollection<Tab>();
        }

        public TabCollection(ObservableCollection<Tab> tabs)
        {
            this.collection = new ObservableCollection<Tab>(tabs) ?? throw new ArgumentNullException(nameof(tabs));
        }

        public Tab this[int index] { get => ((IList<Tab>)collection)[index]; set => ((IList<Tab>)collection)[index] = value; }

        public int Count => ((IList<Tab>)collection).Count;

        public bool IsReadOnly => ((IList<Tab>)collection).IsReadOnly;

        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                ((INotifyPropertyChanged)collection).PropertyChanged += value;
            }

            remove
            {
                ((INotifyPropertyChanged)collection).PropertyChanged -= value;
            }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add
            {
                ((INotifyCollectionChanged)collection).CollectionChanged += value;
            }

            remove
            {
                ((INotifyCollectionChanged)collection).CollectionChanged -= value;
            }
        }

        public void Add(Tab item)
        {
            if(item.Selected)
                item.SetSelected(false);
            if (item.TabCollection == null || item.TabCollection == this)
            {
                if (!collection.Contains(item))
                    ((IList<Tab>)collection).Add(item);
                else
                    throw new ArgumentException("Tab is already in this collection");
            }
            else
                throw new ArgumentException("Tab is already in this collection");
        }

        public void Clear()
        {
            foreach(Tab thisTab in collection)
            {
                thisTab.SetTabCollection(null);
            }
            ((IList<Tab>)collection).Clear();
        }

        public bool Contains(Tab item)
        {
            return ((IList<Tab>)collection).Contains(item);
        }

        public void CopyTo(Tab[] array, int arrayIndex)
        {
            ((IList<Tab>)collection).CopyTo(array, arrayIndex);
        }

        public IEnumerator<Tab> GetEnumerator()
        {
            return ((IList<Tab>)collection).GetEnumerator();
        }

        public int IndexOf(Tab item)
        {
            return ((IList<Tab>)collection).IndexOf(item);
        }

        public void Insert(int index, Tab item)
        {
            if (item.Selected)
                item.SetSelected(false);
            if (item.TabCollection == null || item.TabCollection == this)
            {
                if (!collection.Contains(item))
                    ((IList<Tab>)collection).Insert(index, item);
                else
                    throw new ArgumentException("Tab is already in this collection");
            }
            else
                throw new ArgumentException("Tab is already in this collection");
        }

        public bool Remove(Tab item)
        {
            item.SetTabCollection(null);
            return ((IList<Tab>)collection).Remove(item);
        }

        public void RemoveAt(int index)
        {
            collection[index].SetTabCollection(null);
            ((IList<Tab>)collection).RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IList<Tab>)collection).GetEnumerator();
        }
        public void Move(int oldIndex, int newIndex)
        {
            collection.Move(oldIndex, newIndex);
        }
    }
}
