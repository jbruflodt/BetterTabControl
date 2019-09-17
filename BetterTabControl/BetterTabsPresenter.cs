using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace BetterTabs
{
    [TemplatePart(Name = "TabScroller", Type = typeof(ScrollViewer))]
    [TemplatePart(Name = "ItemsPresenter", Type = typeof(ItemsPresenter))]
    public class BetterTabsPresenter : ItemsControl, INotifyPropertyChanged
    {
        static BetterTabsPresenter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BetterTabsPresenter), new
            FrameworkPropertyMetadata(typeof(BetterTabsPresenter)));
        }
        private ScrollViewer tabScroller;
        private ItemsPresenter itemsPresenter;
        public ScrollViewer TabScroller
        {
            get
            {
                return tabScroller;
            }
        }
        public ItemsPresenter ItemsPresenter
        {
            get
            {
                return itemsPresenter;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public BetterTabsPresenter()
        {
            CommandBindings.Add(new System.Windows.Input.CommandBinding(ScrollBar.LineRightCommand, OnCanExecuteLineRightCommand, OnCanExecuteLineRightCommand));
            CommandBindings.Add(new System.Windows.Input.CommandBinding(ScrollBar.LineLeftCommand, OnCanExecuteLineLeftCommand, OnCanExecuteLineLeftCommand));
        }
        private void NotifyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        protected virtual void OnCanExecuteLineRightCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = TabScroller.ScrollableWidth - TabScroller.HorizontalOffset > 0;
            e.Handled = true;
        }
        protected virtual void OnCanExecuteLineRightCommand(object sender, ExecutedRoutedEventArgs e)
        {
            TabScroller.LineRight();
            e.Handled = true;
        }
        protected virtual void OnCanExecuteLineLeftCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = TabScroller.HorizontalOffset > 0;
            e.Handled = true;
        }
        protected virtual void OnCanExecuteLineLeftCommand(object sender, ExecutedRoutedEventArgs e)
        {
            TabScroller.LineLeft();
            e.Handled = true;
        }
        public override void OnApplyTemplate()
        {
            tabScroller = GetTemplateChild("TabScroller") as ScrollViewer;
            NotifyPropertyChanged("TabScroller");
            itemsPresenter = GetTemplateChild("ItemsPresenter") as ItemsPresenter;
            NotifyPropertyChanged("ItemsPresenter");
            base.OnApplyTemplate();
        }
    }
}
