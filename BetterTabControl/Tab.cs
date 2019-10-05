using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace BetterTabs
{
    [TemplatePart(Name = "CloseButton", Type = typeof(ButtonBase))]
    [TemplatePart(Name = "TitleContent", Type = typeof(ContentControl))]
    [TemplatePart(Name = "TabBackground", Type = typeof(Panel))]
    public class Tab : HeaderedContentControl, INotifyPropertyChanged, IComparer<Tab>
    {
        static Tab()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Tab), new
            FrameworkPropertyMetadata(typeof(Tab)));
        }
        public static readonly DependencyProperty TabTitleProperty = DependencyProperty.Register(
            "TabTitle",
            typeof(object),
            typeof(Tab),
            new FrameworkPropertyMetadata("Untitled", FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnTabTitlePropertyChanged))
            );
        public static readonly DependencyProperty TabContentProperty = DependencyProperty.Register(
            "TabContent",
            typeof(object),
            typeof(Tab),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnTabContentPropertyChanged))
            );
        public static readonly DependencyProperty IDProperty = DependencyProperty.Register(
            "ID",
            typeof(Guid),
            typeof(Tab),
            new FrameworkPropertyMetadata(Guid.Empty)
            );
        public static readonly DependencyProperty DisplayIndexProperty = DependencyProperty.Register(
            "DisplayIndex",
            typeof(int),
            typeof(Tab),
            new FrameworkPropertyMetadata(0, new PropertyChangedCallback(OnDisplayIndexPropertyChanged))
            );
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
            "IsSelected",
            typeof(bool),
            typeof(Tab),
            new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnIsSelectedPropertyChanged))
            );
        public static readonly DependencyProperty IsPressedProperty = DependencyProperty.Register(
            "IsPressed",
            typeof(bool),
            typeof(Tab),
            new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnIsPressedPropertyChanged))
            );
        public static readonly DependencyProperty IsDraggingProperty = DependencyProperty.Register(
            "IsDragging",
            typeof(bool),
            typeof(Tab),
            new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnIsDraggingPropertyChanged))
            );
        public static readonly DependencyProperty ParentTabControlProperty = DependencyProperty.Register(
            "ParentTabControl",
            typeof(BetterTabControl),
            typeof(Tab),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnParentTabControlPropertyChanged))
            );
        public static readonly DependencyProperty TabContentTemplateProperty = DependencyProperty.Register(
            "TabContentTemplate",
            typeof(DataTemplate),
            typeof(Tab),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnTabContentTemplatePropertyChanged))
            );
        public static readonly DependencyProperty MouseOverBackgroundProperty = DependencyProperty.Register(
            "MouseOverBackground",
            typeof(Brush),
            typeof(Tab),
            new FrameworkPropertyMetadata(SystemColors.HighlightBrush, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnMouseOverBackgroundPropertyChanged))
            );

        public static readonly DependencyProperty MouseOverForegroundProperty = DependencyProperty.Register(
            "MouseOverForeground",
            typeof(Brush),
            typeof(Tab),
            new FrameworkPropertyMetadata(SystemColors.HighlightTextBrush, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnMouseOverForegroundPropertyChanged))
            );
        public static readonly DependencyProperty SelectedBackgroundProperty = DependencyProperty.Register(
            "SelectedBackground",
            typeof(Brush),
            typeof(Tab),
            new FrameworkPropertyMetadata(SystemColors.HighlightBrush, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnSelectedBackgroundPropertyChanged))
            );

        public static readonly DependencyProperty SelectedForegroundProperty = DependencyProperty.Register(
            "SelectedForeground",
            typeof(Brush),
            typeof(Tab),
            new FrameworkPropertyMetadata(Brushes.White, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnSelectedForegroundPropertyChanged))
            );

#pragma warning disable S1450 // Private fields only used as local variables in methods should become local variables. left for future use
        private ButtonBase CloseButton;
#pragma warning restore S1450 // Private fields only used as local variables in methods should become local variables. left for future use
        private protected ContentControl TitleContent;
        private Panel TabBackground;
        private bool settingFromParent;
        private bool backgroundSetManually;
        private bool foregroundSetManually;
        private bool mouseOverBackgroundSetManually;
        private bool mouseOverForegroundSetManually;
        private bool selectedBackgroundSetManually;
        private bool selectedForegroundSetManually;
        public object TabTitle
        {
            get { return GetValue(TabTitleProperty); }
            set { SetValue(TabTitleProperty, value); }
        }
        public object TabContent
        {
            get { return GetValue(TabContentProperty); }
            set { SetValue(TabContentProperty, value); }
        }
        public DataTemplate TabContentTemplate
        {
            get { return (DataTemplate)GetValue(TabContentTemplateProperty); }
            set { SetValue(TabContentTemplateProperty, value); }
        }
        public Guid ID
        {
            get { return (Guid)GetValue(IDProperty); }
        }
        public int DisplayIndex
        {
            get { return (int)GetValue(DisplayIndexProperty); }
            set
            {
                PreviousIndex = (int)GetValue(DisplayIndexProperty);
                SetValue(DisplayIndexProperty, value);
            }
        }
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
        }
        public bool IsPressed
        {
            get { return (bool)GetValue(IsPressedProperty); }
        }
        public BetterTabControl ParentTabControl
        {
            get { return (BetterTabControl)GetValue(ParentTabControlProperty); }
        }
        public bool IsDragging
        {
            get { return (bool)GetValue(IsDraggingProperty); }
        }
        public Brush MouseOverBackground
        {
            get { return (Brush)GetValue(MouseOverBackgroundProperty); }
            set { SetValue(MouseOverBackgroundProperty, value); }
        }
        public Brush MouseOverForeground
        {
            get { return (Brush)GetValue(MouseOverForegroundProperty); }
            set { SetValue(MouseOverForegroundProperty, value); }
        }
        public Brush SelectedBackground
        {
            get { return (Brush)GetValue(SelectedBackgroundProperty); }
            set { SetValue(SelectedBackgroundProperty, value); }
        }
        public Brush SelectedForeground
        {
            get { return (Brush)GetValue(SelectedForegroundProperty); }
            set { SetValue(SelectedForegroundProperty, value); }
        }
        internal int PreviousIndex { get; set; }

        public event CancelEventHandler TabClosing;
        public event DependencyPropertyChangedEventHandler DisplayIndexchanged;
        public event DependencyPropertyChangedEventHandler TabContentChanged;
        public event DependencyPropertyChangedEventHandler TabContentTemplateChanged;
        public event DependencyPropertyChangedEventHandler ParentTabControlChanged;
        public event EventHandler TabClosed;
        public event DependencyPropertyChangedEventHandler TabTitleChanged;
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler TabDeselected;
        public event EventHandler TabSelected;

        public Tab(string tabTitle, Control tabContent) : base()
        {
            TabTitle = tabTitle;
            TabContent = tabContent;
            SetID(Guid.NewGuid());
            PreviousIndex = -1;
        }
        public Tab() : this("Untitled", null)
        {
        }
        private static void OnMouseOverBackgroundPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Tab tab = sender as Tab;
            if(!tab.settingFromParent)
            {
                tab.mouseOverBackgroundSetManually = true;
            }
            tab.OnPropertyChanged(e);
        }
        private static void OnMouseOverForegroundPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Tab tab = sender as Tab;
            if (!tab.settingFromParent)
            {
                tab.mouseOverForegroundSetManually = true;
            }
            tab.OnPropertyChanged(e);
        }
        private static void OnSelectedBackgroundPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Tab tab = sender as Tab;
            if (!tab.settingFromParent)
            {
                tab.selectedBackgroundSetManually = true;
            }
            tab.OnPropertyChanged(e);
        }
        private static void OnSelectedForegroundPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Tab tab = sender as Tab;
            if (!tab.settingFromParent)
            {
                tab.selectedForegroundSetManually = true;
            }
            tab.OnPropertyChanged(e);
        }
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (!settingFromParent)
            {
                if (e.Property == Tab.BackgroundProperty)
                {
                    backgroundSetManually = true;
                }
                else if (e.Property == Tab.ForegroundProperty)
                {
                    foregroundSetManually = true;
                }
                else if (e.Property == Tab.MouseOverBackgroundProperty)
                {
                    mouseOverBackgroundSetManually = true;
                }
                else if (e.Property == Tab.MouseOverForegroundProperty)
                {
                    mouseOverForegroundSetManually = true;
                }
                else if (e.Property == Tab.SelectedBackgroundProperty)
                {
                    selectedBackgroundSetManually = true;
                }
                else if (e.Property == Tab.SelectedForegroundProperty)
                {
                    selectedForegroundSetManually = true;
                }
            }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        internal void SetSelected(bool value)
        {
            SetPressed(false);
            if ((bool)GetValue(IsSelectedProperty) != value)
            {
                SetValue(IsSelectedProperty, value);
            }
        }
        protected internal void SetParentTabControl(BetterTabControl parentTabControl)
        {
            SetValue(ParentTabControlProperty, parentTabControl);
            settingFromParent = true;
            if(!selectedBackgroundSetManually && parentTabControl != null)
                SelectedBackground = parentTabControl.SelectedTabBackgroundColor;
            if(!selectedForegroundSetManually && parentTabControl != null)
                SelectedForeground = parentTabControl.SelectedTabTextColor;
            if(!mouseOverBackgroundSetManually && parentTabControl != null)
                MouseOverBackground = parentTabControl.MouseOverTabBackgroundColor;
            if(mouseOverForegroundSetManually && parentTabControl != null)
                MouseOverForeground = parentTabControl.MouseOverTabTextColor;
            if(!backgroundSetManually && parentTabControl != null)
                Background = parentTabControl.TabBackgroundColor;
            if(!foregroundSetManually && parentTabControl != null)
                Foreground = parentTabControl.TabTextColor;
        }
        private void SetID(Guid newID)
        {
            SetValue(IDProperty, newID);
            NotifyPropertyChanged(nameof(ID));
        }
        protected internal void SetPressed(bool pressed)
        {
            SetValue(IsPressedProperty, pressed);
        }
        protected internal void SetDragging(bool pressed)
        {
            SetValue(IsDraggingProperty, pressed);
        }
        private static void OnTabTitlePropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Tab localTab = (Tab)sender;
            localTab.OnTabTitleChanged(e.OldValue, e.NewValue);
        }
        private static void OnTabContentPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Tab localTab = (Tab)sender;
            localTab.OnTabContentChanged(e.OldValue, e.NewValue);
        }
        private static void OnTabContentTemplatePropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Tab localTab = (Tab)sender;
            localTab.OnTabContentTemplateChanged((DataTemplate)e.OldValue, (DataTemplate)e.NewValue);
        }
        private static void OnParentTabControlPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Tab localTab = (Tab)sender;
            localTab.OnParentTabControlChanged((BetterTabControl)e.OldValue, (BetterTabControl)e.NewValue);
        }
        private static void OnDisplayIndexPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Tab localTab = (Tab)sender;
            localTab.OnDisplayIndexChanged((int)e.OldValue, (int)e.NewValue);
        }
        private static void OnIsSelectedPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Tab localTab = (Tab)sender;
            localTab.OnIsSelectedChanged((bool)e.OldValue, (bool)e.NewValue);
        }
        private static void OnIsDraggingPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Tab localTab = (Tab)sender;
            localTab.OnIsDraggingChanged((bool)e.OldValue, (bool)e.NewValue);
        }
        private static void OnIsPressedPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Tab localTab = (Tab)sender;
            localTab.OnIsPressedChanged((bool)e.OldValue, (bool)e.NewValue);
        }
        protected void OnTabTitleChanged(object oldValue, object newValue)
        {
            TabTitleChanged?.Invoke(this, new DependencyPropertyChangedEventArgs(TabTitleProperty, oldValue, newValue));
            NotifyPropertyChanged(nameof(TabTitle));
        }
        protected void OnTabContentChanged(object oldValue, object newValue)
        {
            TabContentChanged?.Invoke(this, new DependencyPropertyChangedEventArgs(TabContentProperty, oldValue, newValue));
            NotifyPropertyChanged(nameof(TabContent));
        }
        protected void OnTabContentTemplateChanged(DataTemplate oldValue, DataTemplate newValue)
        {
            TabContentTemplateChanged?.Invoke(this, new DependencyPropertyChangedEventArgs(TabContentTemplateProperty, oldValue, newValue));
            NotifyPropertyChanged(nameof(TabContentTemplate));
        }
        protected void OnParentTabControlChanged(BetterTabControl oldValue, BetterTabControl newValue)
        {
            ParentTabControlChanged?.Invoke(this, new DependencyPropertyChangedEventArgs(ParentTabControlProperty, oldValue, newValue));
            NotifyPropertyChanged(nameof(ParentTabControl));
        }
        protected void OnDisplayIndexChanged(int oldValue, int newValue)
        {
            DisplayIndexchanged?.Invoke(this, new DependencyPropertyChangedEventArgs(DisplayIndexProperty, oldValue, newValue));
            NotifyPropertyChanged(nameof(DisplayIndex));
        }
        
        protected void OnIsDraggingChanged(bool oldValue, bool newValue)
        {
            NotifyPropertyChanged(nameof(IsDragging));
            if (TabBackground != null)
            {
                TabBackground.IsHitTestVisible = !newValue;
            }
        }
        protected virtual void OnIsSelectedChanged(bool oldValue, bool newValue)
        {
            if (newValue)
                TabSelected?.Invoke(this, new EventArgs());
            else
                TabDeselected?.Invoke(this, new EventArgs());
            NotifyPropertyChanged(nameof(IsSelected));
        }
        protected virtual void OnIsPressedChanged(bool oldValue, bool newValue)
        {
            NotifyPropertyChanged(nameof(IsPressed));
        }
        protected internal void OnTabClosing(CancelEventArgs e)
        {
            TabClosing?.Invoke(this, e);
        }
        public int Compare(Tab x, Tab y)
        {
            if (x == null)
                throw new ArgumentNullException(nameof(x));
            if (y == null)
                throw new ArgumentNullException(nameof(y));
            if (x.DisplayIndex == y.DisplayIndex && x.PreviousIndex == y.PreviousIndex)
            {
                return 0;
            }
            else if (x.DisplayIndex == y.DisplayIndex && x.PreviousIndex > y.PreviousIndex)
            {
                return -1;
            }
            else if (x.DisplayIndex == y.DisplayIndex && x.PreviousIndex < y.PreviousIndex)
            {
                return 1;
            }
            else
            {
                return x.DisplayIndex - y.DisplayIndex;
            }
        }
        public void Close()
        {
            TabClosed?.Invoke(this, new EventArgs());
        }
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e is null)
                throw new ArgumentNullException(nameof(e));
            base.OnPreviewKeyDown(e);
            if (e.Key == Key.Space)
                SetPressed(true);
        }
        protected override void OnPreviewKeyUp(KeyEventArgs e)
        {
            if (e is null)
                throw new ArgumentNullException(nameof(e));
            base.OnPreviewKeyUp(e);
            if (e.Key == Key.Space && IsPressed && !IsSelected)
            {
                SetPressed(false);
                SetSelected(true);
            }
        }
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            if (e is null)
                throw new ArgumentNullException(nameof(e));
            base.OnLostFocus(e);
            if (IsPressed)
                SetPressed(false);
        }
        public override void OnApplyTemplate()
        {
            CloseButton = GetTemplateChild("CloseButton") as ButtonBase;
            TitleContent = GetTemplateChild("TitleContent") as ContentControl;
            TabBackground = GetTemplateChild("TabBackground") as Panel;
            if (CloseButton != null)
            {
                CloseButton.Click += CloseButton_Click;
            }
            if (TabBackground != null)
            {
                TabBackground.PreviewDragOver += TabBackground_PreviewDragOver;
                TabBackground.PreviewMouseLeftButtonDown += TabBackground_PreviewMouseLeftButtonDown;
                TabBackground.PreviewMouseLeftButtonUp += TabBackground_PreviewMouseLeftButtonUp;
                TabBackground.MouseLeave += TabBackground_MouseLeave;
            }
        }
        private void TabBackground_MouseLeave(object sender, MouseEventArgs e)
        {
            if (ParentTabControl != null)
            {
                ParentTabControl.TabBackground_MouseLeave(this, e);
            }
        }

        private void TabBackground_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (ParentTabControl != null)
            {
                ParentTabControl.TabBackground_PreviewMouseLeftButtonUp(this, e);
            }
        }

        private void TabBackground_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ParentTabControl != null)
            {
                ParentTabControl.TabBackground_PreviewMouseLeftButtonDown(this, e);
            }
        }

        private void TabBackground_PreviewDragOver(object sender, DragEventArgs e)
        {
            if (ParentTabControl != null)
            {
                ParentTabControl.TabBackground_PreviewDragOver(this, e);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (ParentTabControl != null)
            {
                ParentTabControl.CloseButton_Click(this, e);
            }
        }
    }
    public class TabComparer : IComparer<Tab>
    {
        public int Compare(Tab x, Tab y)
        {
            if (x == null)
                throw new ArgumentNullException(nameof(x));
            if (y == null)
                throw new ArgumentNullException(nameof(y));
            if (x.DisplayIndex == y.DisplayIndex && x.PreviousIndex == y.PreviousIndex)
            {
                return 0;
            }
            else if (x.DisplayIndex == y.DisplayIndex && x.PreviousIndex > y.PreviousIndex)
            {
                return -1;
            }
            else if (x.DisplayIndex == y.DisplayIndex && x.PreviousIndex < y.PreviousIndex)
            {
                return 1;
            }
            else
            {
                return x.DisplayIndex - y.DisplayIndex;
            }
        }
    }
}
