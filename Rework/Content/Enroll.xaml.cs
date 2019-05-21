using MahApps.Metro.Controls;
using Rework.ViewModels;
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

namespace Rework.Content
{
    /// <summary>
    /// Interaction logic for Enroll.xaml
    /// </summary>
    public partial class Enroll : UserControl
    {
        public Enroll()
        {
            InitializeComponent();
        }

        private void HamburgerMenuControl_OnItemInvoked(object sender, HamburgerMenuItemInvokedEventArgs e)
        {
            HamburgerMenuControl.Content = e.InvokedItem;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button cButton = sender as Button;
            if(cButton.Uid == "1")
            {
                HamburgerMenuControl.SelectedIndex = 1;
                HamburgerMenuControl.Content = ParentSection;
            }
            else if(cButton.Uid == "2")
            {
                HamburgerMenuControl.SelectedIndex = 2;
                HamburgerMenuControl.Content = FinishSection;
            }
        }
    }
}
