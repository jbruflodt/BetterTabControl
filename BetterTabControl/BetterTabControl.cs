﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace BetterTabs
{
    public delegate void AddTabEventHandler(object sender, AddTabEventArgs e);

    public delegate void SelectedTabChangedEventHandler(object sender, SelectedTabChangedEventArgs e);

    public delegate void SelectedTabChangingEventHandler(object sender, SelectedTabChangingEventArgs e);

    public static class WpfExtensions
    {
        public static DependencyObject FindLogicalAncestor(this DependencyObject dp, Type ancestorType)
        {
            if (ancestorType == null)
                throw new ArgumentNullException(nameof(ancestorType));
            else if (!typeof(DependencyObject).IsAssignableFrom(ancestorType))
                throw new ArgumentException(Properties.Resources.ResourceManager.GetString("AssignableFromDependencyObjectExceptionMessage", CultureInfo.CurrentUICulture), nameof(ancestorType));
            else
            {
                DependencyObject testFind = LogicalTreeHelper.GetParent(dp);
                if (testFind == null)
                    return null;
                else if (testFind.GetType() == ancestorType)
                    return testFind;
                else
                    return testFind.FindLogicalAncestor(ancestorType);
            }
        }

        public static DependencyObject FindVisualAncestor(this DependencyObject dp, Type ancestorType)
        {
            if (ancestorType == null)
                throw new ArgumentNullException(nameof(ancestorType));
            else if (!typeof(DependencyObject).IsAssignableFrom(ancestorType))
                throw new ArgumentException(Properties.Resources.ResourceManager.GetString("AssignableFromDependencyObjectExceptionMessage", CultureInfo.CurrentUICulture), nameof(ancestorType));
            else
            {
                DependencyObject testFind = VisualTreeHelper.GetParent(dp);
                if (testFind == null)
                    return null;
                else if (testFind.GetType() == ancestorType)
                    return testFind;
                else
                    return testFind.FindVisualAncestor(ancestorType);
            }
        }
    }

    public class AddTabEventArgs : EventArgs
    {
        public Tab NewTab { get; set; }

        public AddTabEventArgs(Tab newTab) : base()
        {
            NewTab = newTab;
        }
    }

    [TemplatePart(Name = "TabBar", Type = typeof(Panel))]
    [TemplatePart(Name = "TabsPresenter", Type = typeof(BetterTabsPresenter))]
    [TemplatePart(Name = "NewTabButton", Type = typeof(ButtonBase))]
    [TemplatePart(Name = "CurrentContent", Type = typeof(ContentPresenter))]
    public partial class BetterTabControl : Control, INotifyPropertyChanged
    {
        public static readonly DependencyProperty BarBackgroundColorProperty = DependencyProperty.RegisterAttached(
            "BarBackgroundColor",
            typeof(Brush),
            typeof(BetterTabControl),
            new FrameworkPropertyMetadata(SystemColors.MenuBarBrush, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender)
            );

        public static readonly DependencyProperty CloseButtonContentProperty = DependencyProperty.RegisterAttached(
            "CloseButtonContent",
            typeof(object),
            typeof(BetterTabControl),
            new FrameworkPropertyMetadata("X", FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender)
            );

        public static readonly DependencyProperty TabsProperty = DependencyProperty.Register(
            "Tabs",
            typeof(TabCollection),
            typeof(BetterTabControl),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender)
            );

        public static readonly DependencyProperty MouseOverCloseTabTextColorProperty = DependencyProperty.RegisterAttached(
            "MouseOverCloseTabTextColor",
            typeof(Brush),
            typeof(BetterTabControl),
            new FrameworkPropertyMetadata(Brushes.Red, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender)
            );

        public static readonly DependencyProperty MouseOverTabBackgroundColorProperty = DependencyProperty.RegisterAttached(
            "MouseOverTabBackgroundColor",
            typeof(Brush),
            typeof(BetterTabControl),
            new FrameworkPropertyMetadata(SystemColors.HighlightBrush, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender)
            );

        public static readonly DependencyProperty MouseOverTabTextColorProperty = DependencyProperty.RegisterAttached(
            "MouseOverTabTextColor",
            typeof(Brush),
            typeof(BetterTabControl),
            new FrameworkPropertyMetadata(SystemColors.HighlightTextBrush, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender)
            );

        public static readonly DependencyProperty NewTabDisplayTextProperty = DependencyProperty.Register(
            "NewTabDisplayText",
            typeof(string),
            typeof(BetterTabControl),
            new FrameworkPropertyMetadata("+", FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender)
            );

        public static readonly DependencyProperty RibbonColorProperty = DependencyProperty.RegisterAttached(
            "RibbonColor",
            typeof(Brush),
            typeof(BetterTabControl),
            new FrameworkPropertyMetadata(SystemColors.HighlightBrush, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender)
            );

        public static readonly DependencyProperty SelectedTabBackgroundColorProperty = DependencyProperty.RegisterAttached(
            "SelectedTabBackgroundColor",
            typeof(Brush),
            typeof(BetterTabControl),
            new FrameworkPropertyMetadata(SystemColors.HighlightBrush, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender)
            );

        public static readonly DependencyProperty SelectedTabTextColorProperty = DependencyProperty.RegisterAttached(
            "SelectedTabTextColor",
            typeof(Brush),
            typeof(BetterTabControl),
            new FrameworkPropertyMetadata(Brushes.White, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender)
            );

        public static readonly DependencyProperty TabBackgroundColorProperty = DependencyProperty.RegisterAttached(
            "TabBackgroundColor",
            typeof(Brush),
            typeof(BetterTabControl),
            new FrameworkPropertyMetadata(SystemColors.MenuBarBrush, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender)
            );

        public static readonly DependencyProperty TabStyleProperty = DependencyProperty.Register(
            "TabStyle",
            typeof(Style),
            typeof(BetterTabControl),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender)
            );

        public static readonly DependencyProperty TabTextColorProperty = DependencyProperty.RegisterAttached(
            "TabTextColor",
            typeof(Brush),
            typeof(BetterTabControl),
            new FrameworkPropertyMetadata(SystemColors.ControlTextBrush, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender)
            );
        public static readonly DependencyProperty SelectedTabProperty = DependencyProperty.Register(
            "SelectedTab",
            typeof(Tab),
            typeof(BetterTabControl),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender)
            );
        public static readonly DependencyProperty SelectedContentProperty = DependencyProperty.Register(
            "SelectedContent",
            typeof(object),
            typeof(BetterTabControl),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsArrange)
            );
        public static readonly DependencyProperty SelectedContentTemplateProperty = DependencyProperty.Register(
            "SelectedContentTemplate",
            typeof(DataTemplate),
            typeof(BetterTabControl),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.AffectsRender)
            );

        public static readonly RoutedUICommand NextTabCommand = new RoutedUICommand("Next Tab", "NextTab", typeof(BetterTabControl), new InputGestureCollection(new InputGestureCollection { new KeyGesture(Key.Tab, ModifierKeys.Control) }));

        public static readonly RoutedUICommand PreviousTabCommand = new RoutedUICommand("Previous Tab", "PreviousTab", typeof(BetterTabControl), new InputGestureCollection(new InputGestureCollection { new KeyGesture(Key.Tab, ModifierKeys.Control | ModifierKeys.Shift) }));

        private protected ContentPresenter CurrentContent;

        private bool doingDragDrop;

        private Tab draggedTab;

        private Point? dragStart;

        private List<DependencyObject> hitResults;

        private bool indexing;

        private protected ButtonBase NewTabButton;

        private protected Panel TabBar;

        private protected UIElement TabBarFiller;

        private protected BetterTabsPresenter tabsPresenter;

        private protected Guid id;

        public Brush BarBackgroundColor
        {
            get { return (Brush)GetValue(BarBackgroundColorProperty); }
            set { SetValue(BarBackgroundColorProperty, value); }
        }

        public object CloseButtonContent
        {
            get { return (string)GetValue(CloseButtonContentProperty); }
            set { SetValue(CloseButtonContentProperty, value); }
        }

        public Brush MouseOverCloseTabTextColor
        {
            get { return (Brush)GetValue(MouseOverCloseTabTextColorProperty); }
            set { SetValue(MouseOverCloseTabTextColorProperty, value); }
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

        public object SelectedContent
        {
            get { return GetValue(SelectedContentProperty); }
        }

        public DataTemplate SelectedContentTemplate
        {
            get { return (DataTemplate)GetValue(SelectedContentTemplateProperty); }
        }

        public Tab SelectedTab
        {
            get { return (Tab)GetValue(SelectedTabProperty); }
            set
            {
                ChangeSelectedTab(value);
            }
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

        public Brush TabBackgroundColor
        {
            get { return (Brush)GetValue(TabBackgroundColorProperty); }
            set { SetValue(TabBackgroundColorProperty, value); }
        }
        public int SelectedIndex
        {
            get
            {
                return Tabs.IndexOf(SelectedTab);
            }
        }

        public TabCollection Tabs
        {
            get
            {
                return (TabCollection)GetValue(TabsProperty);
            }
        }

        public Style TabStyle
        {
            get { return (Style)GetValue(TabStyleProperty); }
            set { SetValue(TabStyleProperty, value); }
        }

        public Brush TabTextColor
        {
            get { return (Brush)GetValue(TabTextColorProperty); }
            set { SetValue(TabTextColorProperty, value); }
        }
        public BetterTabsPresenter TabsPresenter
        {
            get
            {
                return tabsPresenter;
            }
        }

        public event AddTabEventHandler AddedNewTab;

        public event EventHandler AllTabsClosed;

        public event PropertyChangedEventHandler PropertyChanged;

        public event SelectedTabChangedEventHandler SelectedTabChanged;

        public event SelectedTabChangingEventHandler SelectedTabChanging;

        static BetterTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BetterTabControl), new
            FrameworkPropertyMetadata(typeof(BetterTabControl)));
        }
        public BetterTabControl()
        {
            id = Guid.NewGuid();
            SetValue(TabsProperty, new TabCollection(this));
            Tabs.CollectionChanged += TabsCollectionChanged;
            Loaded += BetterTabControl_Loaded;
            this.SetBinding(SelectedContentProperty, new Binding() { Source = this, Path = new PropertyPathConverter().ConvertFromString("SelectedTab.TabContent") as PropertyPath });
            this.SetBinding(SelectedContentTemplateProperty, new Binding() { Source = this, Path = new PropertyPathConverter().ConvertFromString("SelectedTab.TabContentTemplate") as PropertyPath });
        }
        public void AddNewTab()
        {
            Tab addedTab = new Tab();
            Tabs.Add(addedTab);
            ChangeSelectedTab(addedTab);
        }
        protected internal virtual void OnTabAdded(AddTabEventArgs e)
        {
            AddedNewTab?.Invoke(this, e);
        }
        public override void OnApplyTemplate()
        {
            TabBar = GetTemplateChild("TabBar") as Panel;
            TabBarFiller = GetTemplateChild("TabBarFiller") as UIElement;
            tabsPresenter = GetTemplateChild("TabsPresenter") as BetterTabsPresenter;
            OnPropertyChanged(nameof(TabsPresenter));
            NewTabButton = GetTemplateChild("NewTabButton") as ButtonBase;
            CurrentContent = GetTemplateChild("CurrentContent") as ContentPresenter;
            if (TabsPresenter != null)
            {
                TabsPresenter.PreviewMouseMove += TabsPanel_PreviewMouseMove;
            }
            if (NewTabButton != null)
            {
                NewTabButton.PreviewDragOver += NewTab_PreviewDragOver;
                NewTabButton.Click += NewTab_Click;
            }
            if (TabBarFiller != null)
            {
                TabBarFiller.PreviewDragOver += Filler_PreviewDragOver;
            }
        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            draggedTab = null;
            dragStart = null;
            base.OnPreviewMouseLeftButtonUp(e);
        }
        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            if (e is null)
                throw new ArgumentNullException(nameof(e));
            if (e.LeftButton == MouseButtonState.Released)
            {
                draggedTab = null;
                dragStart = null;
            }
            base.OnPreviewMouseMove(e);
        }

        internal void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Tab thisTab = (Tab)sender;
            CancelEventArgs eventArgs = new CancelEventArgs();
            thisTab.OnTabClosing(eventArgs);
            if (!eventArgs.Cancel)
            {
                thisTab.Close();
                Tabs.Remove(thisTab);
                if (Tabs.Count <= 0)
                {
                    if (AllTabsClosed != null)
                        AllTabsClosed(this, new EventArgs());
                    else
                        AddNewTab();
                }
            }
        }

        internal void Filler_PreviewDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Tab)))
            {
                Tab localDraggedTab = (Tab)e.Data.GetData(typeof(Tab));
                if (Tabs.IndexOf(localDraggedTab) != Tabs.Count - 1)
                {
                    Tabs.Move(Tabs.IndexOf(localDraggedTab), Tabs.Count - 1);
                    UpdateLayout();
                }
                e.Handled = true;
            }
        }

        internal void NewTab_PreviewDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Tab)))
            {
                Tab localDraggedTab = (Tab)e.Data.GetData(typeof(Tab));
                if (Tabs.IndexOf(localDraggedTab) != Tabs.Count - 1)
                {
                    Tabs.Move(Tabs.IndexOf(localDraggedTab), Tabs.Count - 1);
                    UpdateLayout();
                }
                e.Handled = true;
            }
        }

        internal void TabBackground_MouseLeave(object sender, MouseEventArgs e)
        {
            Tab thisTab;
            if (sender.GetType().ToString() == "MS.Internal.NamedObject")
            {
                thisTab = draggedTab;
            }
            else
                thisTab = (Tab)sender;
            thisTab.SetPressed(false);
        }

        internal void TabBackground_PreviewDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Tab)))
            {
                Tab thisTab = (Tab)sender;
                Tab localDraggedTab = (Tab)e.Data.GetData(typeof(Tab));
                if (thisTab.ID != localDraggedTab.ID)
                {
                    Tabs.Move(Tabs.IndexOf(localDraggedTab), Tabs.IndexOf(thisTab));
                    SelectedTab = localDraggedTab;
                    UpdateLayout();
                }
                e.Handled = true;
            }
        }
        protected override void OnPreviewDragOver(DragEventArgs e)
        {
            if (e is null)
                throw new ArgumentNullException(nameof(e));
            DependencyObject visualHit = VisualTreeHelper.HitTest(this as Visual, e.GetPosition(this as IInputElement)).VisualHit;
            DependencyObject visualAncestor = visualHit?.FindVisualAncestor(typeof(Button));
            if (visualAncestor == null)
                visualAncestor = visualHit?.FindVisualAncestor(typeof(Tab));
            if (visualAncestor == null && e.Data.GetDataPresent(typeof(Tab)))
            {
                Tab localDraggedTab = (Tab)e.Data.GetData(typeof(Tab));
                if (Tabs.IndexOf(localDraggedTab) != Tabs.Count - 1)
                {
                    Tabs.Move(Tabs.IndexOf(localDraggedTab), Tabs.Count - 1);
                    SelectedTab = localDraggedTab;
                    UpdateLayout();
                }
                e.Handled = true;
            }
            base.OnPreviewDragOver(e);
        }
        internal void TabBackground_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DependencyObject visualHit = VisualTreeHelper.HitTest(sender as Visual, e.GetPosition(sender as IInputElement)).VisualHit;
            DependencyObject visualAncestor = visualHit?.FindVisualAncestor(typeof(Button));
            if (visualAncestor == null || (visualAncestor as Button).Name != "CloseButton")
            {
                Tab thisTab = (Tab)sender;
                if (!thisTab.IsSelected)
                {
                    foreach (Tab tempTab in Tabs)
                    {
                        if (tempTab.IsSelected)
                            tempTab.SetSelected(false);
                    }
                    ChangeSelectedTab(thisTab);
                }
                draggedTab = thisTab;
                dragStart = e.GetPosition(null);
                thisTab.SetPressed(true);
            }
        }

        internal void TabBackground_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Tab thisTab = (Tab)sender;
            thisTab.SetPressed(false);
        }

        internal void TabsPanel_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (dragStart != null && DragDistance(dragStart.Value, e.GetPosition(null)) > SystemParameters.MinimumHorizontalDragDistance && draggedTab != null && !doingDragDrop)
            {
                doingDragDrop = true;
                draggedTab.SetDragging(true);
                Tabs.Remove(draggedTab);
                DragDropEffects dragResult = DragDrop.DoDragDrop(this, draggedTab, DragDropEffects.Move);
                doingDragDrop = false;
                if (dragResult == DragDropEffects.None)
                {
                    if (!Tabs.Contains(draggedTab))
                        Tabs.Add(draggedTab);
                }
                else
                {
                    for (int x = 0; x < Tabs.Count; x++)
                    {
                        Tabs[x].DisplayIndex = x;
                    }
                }
                SelectedTab = draggedTab;
                draggedTab.SetDragging(false);
                draggedTab = null;
            }
        }

        protected internal void ChangeSelectedTab(Tab tab)
        {
            if (tab is null)
                throw new ArgumentNullException(nameof(tab));
            if (Tabs.Contains(tab))
            {
                Tab oldSelection = SelectedTab;
                if (!OnSelectedTabChanging(oldSelection, tab))
                {
                    ClearSelected();
                    tab.SetSelected(true);
                    SetValue(SelectedTabProperty, tab);
                    OnSelectedTabChanged(oldSelection, tab);
                    NotifySelectedChanged();
                    tab.BringIntoView();
                }
            }
            else
            {
                throw new ArgumentException(Properties.Resources.ResourceManager.GetString("TabNotInControlException", CultureInfo.CurrentUICulture), nameof(tab));
            }
        }

        protected internal void ChangeSelectedTab(int index)
        {
            ChangeSelectedTab(Tabs[index]);
        }

        protected override void OnPreviewDragEnter(DragEventArgs e)
        {
            if (e is null)
                throw new ArgumentNullException(nameof(e));
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
            if (e is null)
                throw new ArgumentNullException(nameof(e));
            base.OnPreviewDragLeave(e);
            if (e.Data.GetDataPresent(typeof(Tab)))
            {
                hitResults = new List<DependencyObject>();
                VisualTreeHelper.HitTest(this, null, HitTestResultCallback, new PointHitTestParameters(e.GetPosition(this)));
                bool validLeave = true;
                if (hitResults.Count == 0 && e.OriginalSource is DependencyObject)
                {
                    hitResults.Add(e.OriginalSource as DependencyObject);
                }
                foreach (DependencyObject thisHit in hitResults)
                {
                    DependencyObject testHit = VisualTreeHelper.GetParent(thisHit);
                    while (testHit != null)
                    {
                        if (testHit is BetterTabControl && (testHit as BetterTabControl).id == id && (testHit as BetterTabControl).id == id)
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
            if (e is null)
                throw new ArgumentNullException(nameof(e));
            base.OnPreviewDrop(e);
            if (e.Data.GetDataPresent(typeof(Tab)))
            {
                e.Effects = DragDropEffects.Move;
                e.Handled = true;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnSelectedTabChanged(Tab oldSelection, Tab newSelection)
        {
            SelectedTabChanged?.Invoke(this, new SelectedTabChangedEventArgs(oldSelection, newSelection));
        }

        protected virtual bool OnSelectedTabChanging(Tab oldSelection, Tab newSelection)
        {
            SelectedTabChangingEventArgs eventArgs = new SelectedTabChangingEventArgs(oldSelection, newSelection);
            SelectedTabChanging?.Invoke(this, eventArgs);
            return eventArgs.Cancel;
        }
        private void BetterTabControl_Loaded(object sender, RoutedEventArgs e)
        {
            Window tempWindow = Window.GetWindow(this);
            if (tempWindow != null)
                tempWindow.Closing += BetterTabControl_Closing;
            if (Tabs.Count <= 0)
                AddNewTab();
            if (SelectedTab == null)
                ChangeSelectedTab(0);
        }

        private void BetterTabControl_Closing(object sender, CancelEventArgs e)
        {
            bool cancel = false;
            foreach (Tab thisTab in Tabs)
            {
                CancelEventArgs eventArgs = new CancelEventArgs();
                thisTab.OnTabClosing(eventArgs);
                if (eventArgs.Cancel)
                    cancel = true;
            }
            e.Cancel = cancel;
        }

        private void ClearSelected()
        {
            foreach (Tab tempTab in Tabs)
            {
                if (tempTab.IsSelected)
                    tempTab.SetSelected(false);
            }
        }

        private double DragDistance(Point start, Point end)
        {
            return Math.Sqrt(Math.Pow((end.X - start.X), 2) + Math.Pow((end.Y - start.Y), 2));
        }

        private HitTestResultBehavior HitTestResultCallback(HitTestResult result)
        {
            hitResults.Add(result.VisualHit);
            return HitTestResultBehavior.Continue;
        }

        private void NewTab_Click(object sender, RoutedEventArgs e)
        {
            AddNewTab();
            ScrollBar.LineRightCommand.CanExecute(null, TabsPresenter);
        }

        private void NotifySelectedChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedIndex)));
        }
        private void ReindexTabs()
        {
            if (!indexing)
            {
                indexing = true;
                for (int x = 0; x < Tabs.Count; x++)
                {
                    Tabs[x].DisplayIndex = x;
                }
            }
        }

        private void TabsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ReindexTabs();
            if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems.Count > 0)
            {
                foreach (Tab thisTab in e.OldItems)
                {
                    if (thisTab.IsSelected)
                    {
                        thisTab.SetSelected(false);
                        if (thisTab.DisplayIndex > 0 && Tabs.Count > thisTab.DisplayIndex - 1)
                            ChangeSelectedTab(Tabs[thisTab.DisplayIndex - 1]);
                        else if (thisTab.DisplayIndex == 0 && Tabs.Count > 0)
                            ChangeSelectedTab(Tabs[0]);
                        else if (Tabs.Count > 0)
                            ChangeSelectedTab(Tabs[Tabs.Count - 1]);
                    }
                }
            }
        }

        private void TabsItemChanged(object sender, PropertyChangedEventArgs e)
        {
            ReindexTabs();
        }
        private T GetChild<T>(DependencyObject obj) where T : DependencyObject
        {
            DependencyObject child = null;
            for (Int32 i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child.GetType() == typeof(T))
                {
                    break;
                }
                else if (child != null)
                {
                    child = GetChild<T>(child);
                    if (child != null && child.GetType() == typeof(T))
                    {
                        break;
                    }
                }
            }
            return child as T;
        }
    }
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
        public static Color ChangeColor(Color color, float correctionFactor)
        {
            float red = color.R;
            float green = color.G;
            float blue = color.B;

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
            if (parameter is null)
                throw new ArgumentNullException(nameof(parameter));
            if (!parameter.GetType().GetTypeInfo().IsAssignableFrom(typeof(float)) && !float.TryParse(parameter.ToString(), out changeValue))
                throw new ArgumentException(Properties.Resources.ResourceManager.GetString("AssignableFromFloatExceptionMessage", culture), nameof(parameter));
            Color color = Colors.Transparent;
            if (targetType == null)
            {
                throw new ArgumentNullException(nameof(targetType));
            }
            if (!targetType.GetTypeInfo().IsAssignableFrom(typeof(Color)) && !targetType.GetTypeInfo().IsAssignableFrom(typeof(SolidColorBrush)))
            {
                if (targetType.GetTypeInfo().IsInstanceOfType(value))
                {
                    return value;
                }
                throw new ArgumentException(Properties.Resources.ResourceManager.GetString("AssignableFromColorSolidColorBrushExceptionMessage", culture), nameof(targetType));
            }
            if (value == null)
            {
                if (targetType.GetTypeInfo().IsAssignableFrom(typeof(Color)))
                    return color;
                else
                    return new SolidColorBrush(color);
            }
            if (!typeof(Color).GetTypeInfo().IsInstanceOfType(value) && !typeof(SolidColorBrush).GetTypeInfo().IsInstanceOfType(value))
            {
                if (targetType.GetTypeInfo().IsInstanceOfType(value))
                {
                    return value;
                }
                throw new ArgumentException(Properties.Resources.ResourceManager.GetString("AssignableFromColorSolidColorBrushExceptionMessage", culture), nameof(value));
            }
            if (typeof(Color).GetTypeInfo().IsInstanceOfType(value))
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
            if (parameter is null)
                throw new ArgumentNullException(nameof(parameter));
            if (!parameter.GetType().GetTypeInfo().IsAssignableFrom(typeof(float)) && !float.TryParse(parameter.ToString(), out changeValue))
                throw new ArgumentException(Properties.Resources.ResourceManager.GetString("AssignableFromFloatExceptionMessage", culture), nameof(parameter));
            Color color = Colors.Transparent;
            if (targetType == null)
            {
                throw new ArgumentNullException(nameof(targetType));
            }
            if (!targetType.GetTypeInfo().IsAssignableFrom(typeof(Color)) && !targetType.GetTypeInfo().IsAssignableFrom(typeof(SolidColorBrush)))
            {
                if (targetType.GetTypeInfo().IsInstanceOfType(value))
                {
                    return value;
                }
                throw new ArgumentException(Properties.Resources.ResourceManager.GetString("AssignableFromColorSolidColorBrushExceptionMessage", culture), nameof(targetType));
            }
            if (value == null)
            {
                if (targetType.GetTypeInfo().IsAssignableFrom(typeof(Color)))
                    return color;
                else
                    return new SolidColorBrush(color);
            }
            if (!typeof(Color).GetTypeInfo().IsInstanceOfType(value) && !typeof(SolidColorBrush).GetTypeInfo().IsInstanceOfType(value))
            {
                if (targetType.GetTypeInfo().IsInstanceOfType(value))
                {
                    return value;
                }
                throw new ArgumentException(Properties.Resources.ResourceManager.GetString("AssignableFromColorSolidColorBrushExceptionMessage", culture), nameof(value));
            }
            if (typeof(Color).GetTypeInfo().IsInstanceOfType(value))
                color = (Color)value;
            else
                color = ((SolidColorBrush)value).Color;
            if (targetType.GetTypeInfo().IsAssignableFrom(typeof(Color)))
                return ChangeColor(color, changeValue * -1);
            else
                return new SolidColorBrush(ChangeColor(color, changeValue * -1));
        }
    }

    public class SelectedTabChangedEventArgs : EventArgs
    {
        public Tab NewSelection { get; set; }

        public Tab OldSelection { get; set; }

        public SelectedTabChangedEventArgs(Tab oldSelection, Tab newSelection) : base()
        {
            OldSelection = oldSelection;
            NewSelection = newSelection;
        }
    }

    public class SelectedTabChangingEventArgs : CancelEventArgs
    {
        public Tab NewSelection { get; set; }

        public Tab OldSelection { get; set; }

        public SelectedTabChangingEventArgs(Tab oldSelection, Tab newSelection) : this(oldSelection, newSelection, false)
        {

        }

        public SelectedTabChangingEventArgs(Tab oldSelection, Tab newSelection, bool cancel) : base(cancel)
        {
            OldSelection = oldSelection;
            NewSelection = newSelection;
        }
    }
    public sealed class TabCollection : ObservableCollection<Tab>
    {
        private readonly BetterTabControl parentTabControl;
        private Tab selectedTab;

        public Tab SelectedTab { get => selectedTab; }

        public TabCollection() : base()
        {
        }

        public TabCollection(BetterTabControl parentTabControl) : base()
        {
            this.parentTabControl = parentTabControl;
        }
        public TabCollection(BetterTabControl parentTabControl, List<Tab> list) : base(list)
        {
            this.parentTabControl = parentTabControl;
        }

        public TabCollection(BetterTabControl parentTabControl, IEnumerable<Tab> collection) : base(collection)
        {
            this.parentTabControl = parentTabControl;
        }
        protected override void ClearItems()
        {
            foreach (Tab thisTab in this)
            {
                thisTab.SetParentTabControl(null);
                thisTab.TabSelected -= Item_TabSelected;
                thisTab.SetSelected(false);
            }
            base.ClearItems();
        }
        public Tab GetTabFromContent(object tabContent)
        {
            if (tabContent == null)
                throw new ArgumentNullException(nameof(tabContent));
            else
            {
                return this.First((thisTab) =>
                    {
                        return ReferenceEquals(tabContent, thisTab.TabContent);
                    });
            }
        }

        protected override void InsertItem(int index, Tab item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (item.IsSelected)
                item.SetSelected(false);
            if (item.ParentTabControl == null || item.ParentTabControl == parentTabControl)
            {
                if (!Contains(item))
                {
                    item.SetParentTabControl(parentTabControl);
                    item.Style = parentTabControl.TabStyle;
                    item.TabSelected += Item_TabSelected;
                    base.InsertItem(index, item);
                    parentTabControl.OnTabAdded(new AddTabEventArgs(item));
                }
                else
                    throw new ArgumentException(Properties.Resources.ResourceManager.GetString("TabInCollectionException", CultureInfo.CurrentUICulture), nameof(item));
            }
            else
                throw new ArgumentException(Properties.Resources.ResourceManager.GetString("TabInCollectionException", CultureInfo.CurrentUICulture), nameof(item));
        }

        private void Item_TabSelected(object sender, EventArgs e)
        {
            if (SelectedTab != null)
            {
                foreach (Tab thisTab in this)
                {
                    if (!ReferenceEquals(thisTab, (sender as Tab)) && thisTab.IsSelected)
                        thisTab.SetSelected(false);
                }
            }
            selectedTab = (sender as Tab);
        }

        protected override void RemoveItem(int index)
        {
            this[index].TabSelected -= Item_TabSelected;
            this[index].SetParentTabControl(null);
            base.RemoveItem(index);
        }
    }
    internal class TabPresenterWidthCalculator : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 3 || !(values[0] is double) || !(values[1] is double))
                return double.PositiveInfinity;
            else if (!(values[2] is double))
                return (double)values[0] - (double)values[1];
            else
                return (double)values[0] - ((double)values[1] + (double)values[2]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    internal class ScrollButtonsVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    internal class UniversalValueConverter : IValueConverter
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Not sure why this is even being raised")]
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // obtain the conveter for the target type
            TypeConverter converter = TypeDescriptor.GetConverter(targetType);

            try
            {
                // determine if the supplied value is of a suitable type
                if (converter.CanConvertFrom(value.GetType()))
                {
                    // return the converted value
                    return converter.ConvertFrom(value);
                }
                else
                {
                    // try to convert from the string representation
                    return converter.ConvertFrom(value.ToString());
                }
            }
            catch (NotSupportedException)
            {
                return value;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}