using System;
using System.Collections.Generic;
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
using System.Windows.Controls.Primitives;

namespace BetterTabs
{
    /// <summary>
    /// Interaction logic for FilterPopup.xaml
    /// </summary>
    public partial class FilterPopup : Popup
    {
        public FilterPopup()
        {
            InitializeComponent();
        }
        public void ClearValues()
        {
            for (int x = 1; x < this.FilterList.Items.Count; x++)
            {
                this.FilterList.Items.RemoveAt(x);
            }
        }
        public void Add(string value)
        {
            CheckBox tempCheck = new CheckBox();
            tempCheck.Content = value;
            this.FilterList.Items.Add(tempCheck);
        }

        private void AllCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            foreach(object item in FilterList.Items)
            {
                CheckBox thisCheck = (CheckBox)item;
                thisCheck.IsChecked = true;
            }
        }

        private void AllCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (object item in FilterList.Items)
            {
                CheckBox thisCheck = (CheckBox)item;
                thisCheck.IsChecked = false;
            }
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            bool all = true;
            foreach (object item in FilterList.Items)
            {
                CheckBox thisCheck = (CheckBox)item;
                if (thisCheck.IsChecked == false)
                {
                    all = false;
                }
                AllCheckBox.IsChecked = all ? true : new bool?();
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            bool all = true;
            foreach (object item in FilterList.Items)
            {
                CheckBox thisCheck = (CheckBox)item;
                if(thisCheck.IsChecked != false)
                {
                    all = false;
                }
                AllCheckBox.IsChecked = all ? false : new bool?();
            }
        }
    }
}
