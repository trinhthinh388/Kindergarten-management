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
using ViewModel;

namespace KindergartenManagement.UserControlKM
{
    /// <summary>
    /// Interaction logic for NavigationBarUC.xaml
    /// </summary>
    public partial class NavigationBarUC : UserControl
    {
        public NavigationBarUC()
        {
            InitializeComponent();
            this.DataContext = NavigationBarVM.Ins;
        }

        public void OpenLoginWindow()
        {
            LogInWindow logInWindow = new LogInWindow();
            logInWindow.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LogInVM logInVM = new LogInVM();
            if (logInVM.IsLogIn == false)
            {
                OpenLoginWindow();
                if (logInVM.IsLogIn == true)
                {
                    logInButton.Visibility = Visibility.Collapsed;
                    logOutButton.Visibility = Visibility.Visible;
                }
            }
            else
            {
                MessageBoxResult mr = MessageBox.Show("Do you really want to log out?", "", MessageBoxButton.YesNo);
                if (mr == MessageBoxResult.No)
                    return;
                logInVM.IsLogIn = false;
                NavigationBarVM.Ins.ResetWelcomeText();
                logInButton.Visibility = Visibility.Visible;
                logOutButton.Visibility = Visibility.Collapsed;
            }
            
        }

        private void ManageChildren_MouseEnter(object sender, MouseEventArgs e)
        {
            ManageChildren.IsExpanded = true;
        }

        private void ManageChildren_MouseLeave(object sender, MouseEventArgs e)
        {
            ManageChildren.IsExpanded = false;
        }

        private void ManageClasses_MouseEnter(object sender, MouseEventArgs e)
        {
            ManageClasses.IsExpanded = true;
        }

        private void ManageClasses_MouseLeave(object sender, MouseEventArgs e)
        {
            ManageClasses.IsExpanded = false;
        }

        private void ManageSystem_MouseEnter(object sender, MouseEventArgs e)
        {
            ManageSystem.IsExpanded = true;
        }

        private void ManageSystem_MouseLeave(object sender, MouseEventArgs e)
        {
            ManageSystem.IsExpanded = false;
        }
    }
}
