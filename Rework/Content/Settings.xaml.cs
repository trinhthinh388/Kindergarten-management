﻿using MahApps.Metro.Controls;
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
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        public Settings()
        {
            InitializeComponent();
            this.DataContext = SettingViewModel.Ins;
        }

        private void HamburgerMenuControl_OnItemInvoked(object sender, HamburgerMenuItemInvokedEventArgs e)
        {
            HamburgerMenuControl.Content = e.InvokedItem;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(this.newPassBox.Password == this.confirmPassBox.Password && this.newPassBox.Password != "")
            {
                SettingViewModel.Ins.NewPassword = this.newPassBox.Password;
                Console.WriteLine("match!");
                this.newPassBox.Password = this.confirmPassBox.Password = "";
            }
            else
            {
                SettingViewModel.Ins.NewPassword = "The password is not match!! shit!!";
            }
        }


    }
}
