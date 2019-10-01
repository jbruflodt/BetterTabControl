using System.Windows;
using System.Windows.Controls;

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
            for (int x = 0; x < 11; x++)
            {
                Tabs.AddNewTab();
                Tabs.SelectedTab.TabTitle = "tab" + x.ToString();
                Tabs.SelectedTab.TabContent = new Button()
                {
                    Content = "tab" + x.ToString()
                };
            }
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
