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
using Expression.Blend.SampleData.SampleDataSource;


namespace BetterTabControlTest
{
    /// <summary>
    /// Interaction logic for TestDataGrid.xaml
    /// </summary>
    public partial class TestDataGrid : UserControl
    {
        public TestDataGrid()
        {
            TestData = new SampleDataSource();
            InitializeComponent();
        }
        public SampleDataSource TestData { get; set; }
    }
}
