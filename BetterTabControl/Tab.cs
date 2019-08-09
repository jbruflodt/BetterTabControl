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
    [Serializable()]
    [TemplatePart(Name = "CloseButton", Type = typeof(ButtonBase))]
    [TemplatePart(Name = "TitleContent", Type = typeof(ContentPresenter))]
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
            new FrameworkPropertyMetadata("Untitled", FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(OnTabTitlePropertyChanged))
            );
        public static readonly DependencyProperty TabContentProperty = DependencyProperty.Register(
            "TabContent",
            typeof(object),
            typeof(Tab),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnTabContentPropertyChanged))
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
        public static readonly DependencyProperty SelectedProperty = DependencyProperty.Register(
            "Selected",
            typeof(bool),
            typeof(Tab),
            new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnSelectedPropertyChanged))
            );
        public static readonly DependencyProperty IsPressedProperty = DependencyProperty.Register(
            "Pressed",
            typeof(bool),
            typeof(Tab),
            new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnIsPressedPropertyChanged))
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
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnTabContentTemplatePropertyChanged))
            );
        private ButtonBase CloseButton;
        private ContentPresenter TitleContent;
        private Panel TabBackground;
        private int previousIndex;
        private bool dragging;


        public object TabTitle
        {
            get { return (object)GetValue(TabTitleProperty); }
            set { SetValue(TabTitleProperty, value); }
        }
        public object TabContent
        {
            get { return (object)GetValue(TabContentProperty); }
            set { SetValue(TabContentProperty, value); }
        }
        public DataTemplate TabContentTemplate
        {
            get { return (DataTemplate)GetValue(TabContentTemplateProperty); }
            set { SetValue(TabContentProperty, value); }
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
                previousIndex = (int)GetValue(DisplayIndexProperty);
                SetValue(DisplayIndexProperty, value);
            }
        }
        public bool Selected
        {
            get { return (bool)GetValue(SelectedProperty); }
        }
        public bool IsPressed
        {
            get { return (bool)GetValue(IsPressedProperty); }
        }
        public BetterTabControl ParentTabControl
        {
            get { return (BetterTabControl)GetValue(ParentTabControlProperty); } 
        }
        public bool Dragging
        {
            get
            {
                return dragging;
            }
            set
            {
                bool oldValue = dragging;
                dragging = value;
                OnDraggingChanged(oldValue, value);
            }
        }

        internal int PreviousIndex { get => previousIndex; set => previousIndex = value; }

        public event CancelableTabEventHandler CloseButtonClicked;
        public event EventHandler DisplayIndexchanged;
        public event EventHandler TabContentChanged;
        public event EventHandler TabContentTemplateChanged;
        public event EventHandler ParentTabControlChanged;
        public event EventHandler TabClosed;
        public event EventHandler TabTitleChanged;
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler TabDeselected;
        public event EventHandler TabSelected;

        public Tab(string tabTitle, Control tabContent) : base()
        {
            this.TabTitle = tabTitle;
            this.TabContent = tabContent;
            this.SetID(Guid.NewGuid());
            previousIndex = -1;
        }
        public Tab() : this("Untitled", null)
        {
        }
        

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        internal void SetSelected(bool value)
        {
            SetPressed(false);
            if ((bool)GetValue(SelectedProperty) != value)
            {
                SetValue(SelectedProperty, value);
            }
        }
        protected internal void SetParentTabControl(BetterTabControl parentTabControl)
        {
            SetValue(ParentTabControlProperty, parentTabControl);
        }
        private void SetID(Guid newID)
        {
            SetValue(IDProperty, newID);
            NotifyPropertyChanged("ID");
        }
        protected internal void SetPressed(bool pressed)
        {
            SetValue(IsPressedProperty, pressed);
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
        private static void OnSelectedPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Tab localTab = (Tab)sender;
            localTab.OnSelectedChanged((bool)e.OldValue, (bool)e.NewValue);
        }
        private static void OnIsPressedPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Tab localTab = (Tab)sender;
            localTab.OnIsPressedChanged((bool)e.OldValue, (bool)e.NewValue);
        }
        protected void OnTabTitleChanged(object oldValue, object newValue)
        {
            TabTitleChanged?.Invoke(this, new EventArgs());
            NotifyPropertyChanged("TabTitle");
        }
        protected void OnTabContentChanged(object oldValue, object newValue)
        {
            TabContentChanged?.Invoke(this, new EventArgs());
            NotifyPropertyChanged("TabContent");
        }
        protected void OnTabContentTemplateChanged(DataTemplate oldValue, DataTemplate newValue)
        {
            TabContentTemplateChanged?.Invoke(this, new EventArgs());
            NotifyPropertyChanged("TabContentTemplate");
        }
        protected void OnParentTabControlChanged(BetterTabControl oldValue, BetterTabControl newValue)
        {
            ParentTabControlChanged?.Invoke(this, new EventArgs());
            NotifyPropertyChanged("ParentTabControl");
        }
        protected void OnDisplayIndexChanged(int oldValue, int newValue)
        {
            DisplayIndexchanged?.Invoke(this, new EventArgs());
            NotifyPropertyChanged("DisplayIndex");
        }
        protected void OnDraggingChanged(bool oldValue, bool newValue)
        {
            NotifyPropertyChanged("Dragging");
            if(TabBackground != null)
            {
                TabBackground.IsHitTestVisible = !newValue;
            }
        }
        protected virtual void OnSelectedChanged(bool oldValue, bool newValue)
        {
            NotifyPropertyChanged("Selected");
        }
        protected virtual void OnIsPressedChanged(bool oldValue, bool newValue)
        {
            NotifyPropertyChanged("IsPressed");
        }
        protected internal void OnCloseButtonClick(CancelableTabEventArgs e)
        {
            CloseButtonClicked?.Invoke(this, e);
        }
        
        public int Compare(Tab x, Tab y)
        {
            if (x == null)
                throw new ArgumentNullException("x");
            if (y == null)
                throw new ArgumentNullException("y");
            if (x.DisplayIndex == y.DisplayIndex && x.previousIndex == y.previousIndex)
            {
                return 0;
            }
            else if (x.DisplayIndex == y.DisplayIndex && x.previousIndex > y.previousIndex)
            {
                return -1;
            }
            else if (x.DisplayIndex == y.DisplayIndex && x.previousIndex < y.previousIndex)
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
            base.OnPreviewKeyDown(e);
            if (e.Key == Key.Space)
                SetPressed(true);
        }
        protected override void OnPreviewKeyUp(KeyEventArgs e)
        {
            base.OnPreviewKeyUp(e);
            if (e.Key == Key.Space && this.IsPressed && !this.Selected)
            {
                SetPressed(false);
                SetSelected(true);
            }
        }
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            if (this.IsPressed)
                SetPressed(false);
        }
        public override void OnApplyTemplate()
        {
            CloseButton = GetTemplateChild("CloseButton") as ButtonBase;
            TitleContent = GetTemplateChild("TitleContent") as ContentPresenter;
            TabBackground = GetTemplateChild("TabBackground") as Panel;
            if (CloseButton != null)
            {
                CloseButton.Click += CloseButton_Click;
            }
            if (TabBackground != null)
            {
                TabBackground.MouseEnter += TabBackground_MouseEnter;
                TabBackground.PreviewDragOver += TabBackground_PreviewDragOver;
                TabBackground.PreviewMouseLeftButtonDown += TabBackground_PreviewMouseLeftButtonDown;
                TabBackground.PreviewMouseLeftButtonUp += TabBackground_PreviewMouseLeftButtonUp;
                TabBackground.MouseLeave += TabBackground_MouseLeave;
            }
        }
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
        }
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
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

        private void TabBackground_MouseEnter(object sender, MouseEventArgs e)
        {
            if (ParentTabControl != null)
            {
                ParentTabControl.TabBackground_MouseEnter(this, e);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if(ParentTabControl != null)
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
                throw new ArgumentNullException("x");
            if (y == null)
                throw new ArgumentNullException("y");
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
