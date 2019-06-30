﻿using System;
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

namespace BetterTabs
{
    /// <summary>
    /// Interaction logic for NewTab.xaml
    /// </summary>
    public partial class NewTab : UserControl
    {
        public NewTab()
        {
            InitializeComponent();
        }
        public string DisplayText
        {
            get { return (string)GetValue(BetterTabControlBar.NewTabDisplayTextProperty); }
            set { SetValue(BetterTabControlBar.NewTabDisplayTextProperty, value); }
        }
    }
}