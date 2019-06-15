using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Rework.Models;
using Rework.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            this.DataContext = new EnrollViewModel();
        }

        private void HamburgerMenuControl_OnItemInvoked(object sender, HamburgerMenuItemInvokedEventArgs e)
        {
            HamburgerMenuControl.Content = e.InvokedItem;
        }


        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var CurrentWindow = Application.Current.MainWindow as MetroWindow;
            var MySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Ok",
                ColorScheme = CurrentWindow.MetroDialogOptions.ColorScheme
            };
            Button cButton = sender as Button;
            if(cButton.Uid == "1")
            {
                if (ChildrenTxtB.Text == "" || NickNameTxtB.Text == "" || BirthDatePicker.Text == "")
                {
                    await CurrentWindow.ShowMessageAsync("Hello!", "Please fill in every blanks.", MessageDialogStyle.Affirmative, MySettings);
                    return;
                }
                HamburgerMenuControl.SelectedIndex = 1;
                HamburgerMenuControl.Content = ParentSection;
            }
            else if(cButton.Uid == "2")
            {
                string regexString = "^\\+?\\d{1,3}?[- .]?\\(?(?:\\d{2,3})\\)?[- .]?\\d\\d\\d[- .]?\\d\\d\\d\\d$";
                if (Mothertxb.Text == "" || Fathertxb.Text == "" || Phonetxb.Text == "" || Addresstxb.Text == "")
                {
                    await CurrentWindow.ShowMessageAsync("Hello!", "Please fill in every blanks.", MessageDialogStyle.Affirmative, MySettings);
                    return;
                }
                if(!Regex.IsMatch(Phonetxb.Text, regexString) || Phonetxb.Text.Length < 10)
                {
                    await CurrentWindow.ShowMessageAsync("Hello!", "Your phone number is not valid", MessageDialogStyle.Affirmative, MySettings);
                    return;
                }
                HamburgerMenuControl.SelectedIndex = 2;
                HamburgerMenuControl.Content = FinishSection;
            }
        }
    }
}
