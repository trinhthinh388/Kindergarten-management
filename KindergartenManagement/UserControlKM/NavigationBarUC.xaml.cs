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
            this.DataContext = new NavigationBarVM();
        }

        public void OpenLoginWindow()
        {
            LogInWindow logInWindow = new LogInWindow();
            logInWindow.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenLoginWindow();
        }
    }
}
