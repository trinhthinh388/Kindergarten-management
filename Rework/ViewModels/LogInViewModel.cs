using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rework.Models;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;

namespace Rework.ViewModels
{
    public class LogInViewModel: BaseViewModel
    {
        private List<user> listUsers;
        private string username;
        private string password;
        private string name;

        public static bool isLogin;
        public ICommand LogInCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        public List<user> ListUsers
        {
            get
            {
                return this.listUsers;
            }
            set
            {
                this.listUsers = value;
            }
        }
        public string Username
        {
            get
            {
                return this.username;
            }
            set
            {
                this.username = value;
                OnPropertyChange("Username");
            }
        }

        

        public LogInViewModel()
        {
            isLogin = false;
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) =>
             {
                 password = p.Password;
             });
            LogInCommand = new RelayCommand<UserControl>((p) => { return true; }, 
                async (p) =>
            {
                var CurrentWindow = Application.Current.MainWindow as MetroWindow;
                var mySettings = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "Ok",
                    NegativeButtonText = "Go away!",
                    FirstAuxiliaryButtonText = "Cancel",
                    ColorScheme = CurrentWindow.MetroDialogOptions.ColorScheme
                };

                if(AuthUser(username, password))
                {
                    await CurrentWindow.ShowMessageAsync("Hello!", "Log in successfully.", MessageDialogStyle.Affirmative, mySettings);
                    isLogin = true;
                }
                else
                {
                    await CurrentWindow.ShowMessageAsync("Hello!", "Wrong username or password.", MessageDialogStyle.Affirmative, mySettings);
                }
            });
        }


        private bool AuthUser(string username, string password)
        {
            int logInUser = DataProvider.Ins.DB.users.Where(x => x.username == username && x.password == password).ToArray().Count();
            if(logInUser == 0)
            {
                return false;
            }
            else if(logInUser == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
