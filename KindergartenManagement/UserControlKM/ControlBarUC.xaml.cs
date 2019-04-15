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
    /// Interaction logic for ControlBarUC.xaml
    /// </summary>
    public partial class ControlBarUC : UserControl
    {
        public ControlBarVM ViewModel { get; set; }
        public ControlBarUC()
        {
            InitializeComponent();
            this.DataContext = ViewModel = new ControlBarVM();
        }

        private void MaximizeBtn_Click(object sender, RoutedEventArgs e)
        {
            maximizeBtn.Visibility = Visibility.Collapsed;
            restoreBtn.Visibility = Visibility.Visible;
        }

        private void RestoreBtn_Click(object sender, RoutedEventArgs e)
        {
            maximizeBtn.Visibility = Visibility.Visible;
            restoreBtn.Visibility = Visibility.Collapsed;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
