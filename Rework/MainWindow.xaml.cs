using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Rework.ViewModels;

namespace Rework
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }

        private void HomeButtonClick(object sender, RoutedEventArgs e)
        {
            MainTabControl.SelectedIndex = 0;
        }

        private void MainTabControl_LayoutUpdated(object sender, EventArgs e)
        {
            if(MainTabControl.SelectedIndex != 0)
            {
                this.HomeButton.Visibility = Visibility.Visible;
                this.TitlebarHeight = 50;
            }
            else
            {
                this.HomeButton.Visibility = Visibility.Collapsed;
            }
        }
    }
}
