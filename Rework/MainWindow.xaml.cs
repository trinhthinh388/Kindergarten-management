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
using MahApps.Metro.Controls.Dialogs;

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
            this.DataContext = MainViewModel.Ins;
            MainViewModel.Ins.dialogCoordinator = DialogCoordinator.Instance;
            
        }

        private async void HomeButtonClick(object sender, RoutedEventArgs e)
        {
            MainTabControl.SelectedIndex = 0;
        }

        private void MainTabControl_LayoutUpdated(object sender, EventArgs e)
        {
            if (LogInViewModel.isLogin && MainTabControl.SelectedIndex == 0)
            {
                MainTabControl.SelectedIndex = 1;
                this.MainWD.ShowTitleBar = true;
            }

            if(MainTabControl.SelectedIndex == 0)
            {
                this.MainWD.ShowTitleBar = false;
            }

            if (MainTabControl.SelectedIndex > 1)
            {
                this.HomeButton.Visibility = Visibility.Visible;
            }
            else
            {
                this.HomeButton.Visibility = Visibility.Collapsed;
            }
        }
    }
}
