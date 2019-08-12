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

namespace BetterTabControlTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //Tabs.AddNewTab();
            //Tabs.AddNewTab();
            //Tabs.Tabs[0].TabContent = new Button()
            //{
            //    Content = "Tab1"
            //};
            //Tabs.Tabs[0].TabTitle = "";
            //Tabs.Tabs[1].TabContent = new Button()
            //{
            //    Content = "Tab2"
            //};
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
