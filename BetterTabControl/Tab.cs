using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Media;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BetterTabs
{
    [Serializable()]
    [TemplatePart(Name = "CloseButton", Type = typeof(System.Windows.Controls.Primitives.ButtonBase))]
    [TemplatePart(Name = "TitleContent", Type = typeof(ContentPresenter))]
    public class Tab : HeaderedContentControl, INotifyPropertyChanged, IComparer<Tab>
    {
        public static readonly DependencyProperty TabTitleProperty = DependencyProperty.Register(
            "TabTitle",
            typeof(object),
            typeof(Tab),
            new FrameworkPropertyMetadata("Untitled", FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(OnTabTitleChanged))
            );
        public static readonly DependencyProperty TabContentProperty = DependencyProperty.Register(
            "TabContent",
            typeof(object),
            typeof(Tab),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnTabContentChanged))
            );
        public static readonly DependencyProperty IDProperty = DependencyProperty.RegisterAttached(
            "ID",
            typeof(Guid),
            typeof(Tab),
            new FrameworkPropertyMetadata(Guid.Empty, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnIDChanged))
            );
        public static readonly DependencyProperty DisplayIndexProperty = DependencyProperty.RegisterAttached(
            "DisplayIndex",
            typeof(int),
            typeof(Tab),
            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnDisplayIndexChanged))
            );
        public static readonly DependencyProperty SelectedProperty = DependencyProperty.RegisterAttached(
            "Selected",
            typeof(bool),
            typeof(Tab),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnSelectedChanged))
            );
        public static readonly DependencyProperty IsPressedProperty = DependencyProperty.RegisterAttached(
            "Pressed",
            typeof(bool),
            typeof(Tab),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender)
            );
        private object tabTitle;
        private UIElement tabContent;
        private Guid id;
        private int displayIndex;
        private int previousIndex;
        private bool selected;
        private TabCollection tabCollection;
        private bool dragging;


        public object TabTitle
        {
            get { return (object)GetValue(TabTitleProperty); }
            set { SetValue(TabTitleProperty, value); }
        }
        public UIElement TabContent
        {
            get { return (Control)GetValue(TabContentProperty); }
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
        public TabCollection TabCollection
        {
            get
            {
                return tabCollection;
            }
        }
        public bool Dragging
        {
            get
            {
                return dragging;
            }
            set
            {
                dragging = value;
                NotifyPropertyChanged("Dragging");
            }
        }

        internal int PreviousIndex { get => previousIndex; set => previousIndex = value; }

        public event CancelableTabEventHandler CloseButtonClicked;
        public event CancelableTabEventHandler TabSelected;
        public event EventHandler TabClosed;

        public Tab(string tabTitle, Control tabContent) : base()
        {
            this.TabTitle = tabTitle;
            this.TabContent = tabContent;
            this.SetID(Guid.NewGuid());
            previousIndex = -1;
        }
        public Tab() : base()
        {
            this.TabTitle = "Untitled";
            this.TabContent = null;
            this.SetID(Guid.NewGuid());
            previousIndex = -1;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler SelectChanged;

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
                SelectChanged?.Invoke(this, new EventArgs());
            }
        }
        internal void SetTabCollection(TabCollection tabCollection)
        {
            this.tabCollection = tabCollection;
            NotifyPropertyChanged("TabCollection");
        }
        private void SetID(Guid newID)
        {
            SetValue(IDProperty, newID);
        }
        internal void SetPressed(bool pressed)
        {
            SetValue(IsPressedProperty, pressed);
        }
        private static void OnTabTitleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Tab localTab = (Tab)sender;
            localTab.tabTitle = (string)e.NewValue;
        }
        private static void OnTabContentChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Tab localTab = (Tab)sender;
            localTab.tabContent = (UIElement)e.NewValue;
        }
        private static void OnIDChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Tab localTab = (Tab)sender;
            localTab.id = (Guid)e.NewValue;
        }
        private static void OnDisplayIndexChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Tab localTab = (Tab)sender;
            localTab.displayIndex = (int)e.NewValue;
        }
        private static void OnSelectedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Tab localTab = (Tab)sender;
            localTab.selected = (bool)e.NewValue;
        }
        internal void OnCloseButtonClick(CancelableTabEventArgs e)
        {
            CloseButtonClicked?.Invoke(this, e);
        }
        internal void OnSelected(CancelableTabEventArgs e)
        {
            TabSelected?.Invoke(this, e);
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

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
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
