using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.ComponentModel;

namespace BetterTabs
{
    public class Tab : INotifyPropertyChanged, IComparer<Tab>
    {
        private string tabTitle;
        private Control tabContent;
        private Guid id;
        private int displayIndex;
        private bool hasMoved;
        private bool selected;

        public string TabTitle
        {
            get
            {
                return tabTitle;
            }
            set
            {
                tabTitle = value;
                NotifyPropertyChanged("TabTitle");
            }
        }
        public Control TabContent
        {
            get
            {
                return tabContent;
            }
            set
            {
                tabContent = value;
                NotifyPropertyChanged("TabContent");
            }
        }
        public Guid ID
        {
            get
            {
                return id;
            }
        }
        public int DisplayIndex
        {
            get
            {
                return displayIndex;
            }
            set
            {
                displayIndex = value;
                NotifyPropertyChanged("DisplayIndex");
            }
        }
        public bool Selected
        {
            get
            {
                return selected;
            }
            set
            {
                selected = value;
                NotifyPropertyChanged("Selected");
            }
        }

        internal bool HasMoved { get => hasMoved; set => hasMoved = value; }

        public Tab(string tabTitle, Control tabContent)
        {
            this.tabTitle = tabTitle;
            this.tabContent = tabContent;
            id = Guid.NewGuid();
        }
        public Tab()
        {
            this.tabTitle = "Untitled";
            this.tabContent = null;
            id = Guid.NewGuid();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int Compare(Tab x, Tab y)
        {
            if (x == null)
                throw new ArgumentNullException("x");
            if (y == null)
                throw new ArgumentNullException("y");
            if (x.DisplayIndex == y.DisplayIndex && ((!x.HasMoved && !y.HasMoved) || (x.HasMoved && y.HasMoved)))
            {
                if(x.HasMoved && y.HasMoved)
                {
                    x.HasMoved = false;
                    y.HasMoved = false;
                }
                return 0;
            }
            else if (x.DisplayIndex == y.DisplayIndex && x.HasMoved)
            {
                x.HasMoved = false;
                return -1;
            }
            else if (x.DisplayIndex == y.DisplayIndex && y.HasMoved)
            {
                y.HasMoved = false;
                return 1;
            }
            else
            {
                return x.DisplayIndex - y.DisplayIndex;
            }
        }
    }
}
