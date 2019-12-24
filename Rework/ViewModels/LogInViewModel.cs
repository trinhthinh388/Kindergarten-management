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
using System.Threading;
using System.Windows.Threading;

namespace Rework.ViewModels
{
    public class LogInViewModel: BaseViewModel
    {
        private List<user> listUsers;
        private string username;
        private string password;
        private bool isValid;

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
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; },(p) =>
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

                await Task.Factory.StartNew(() => AuthUser(username, password, CurrentWindow, mySettings));
                

            });
        }


        private async void AuthUser(string username, string password, MetroWindow CurrentWindow, MetroDialogSettings mySettings)
        {

            int logInUser = DataProvider.Ins.DB.users.Where(x => x.username.Equals(username) && x.password.Equals(password)).ToArray().Count();
            
            if (logInUser == 0)
            {
                await Application.Current.Dispatcher.Invoke(async () =>
                {
                    await CurrentWindow.ShowMessageAsync("Hello!", "Wrong username or password.", MessageDialogStyle.Affirmative, mySettings);
                });
            }
            else if(logInUser == 1)
            {
                int idUser = DataProvider.Ins.DB.users.Where(x => x.username.Equals(username) && x.password.Equals(password)).ToArray()[0].id;
                await Task.Factory.StartNew(() => { Console.WriteLine("Load Username"); MainViewModel.Ins.LoadUserName(idUser); });
                await Task.Factory.StartNew( () => { Console.WriteLine("Load data"); SettingViewModel.LoadData(); });
                await Application.Current.Dispatcher.Invoke(async () =>
                {
                    Console.WriteLine("Loged in");
                    await CurrentWindow.ShowMessageAsync("Hello!", "Log in successfully.", MessageDialogStyle.Affirmative, mySettings);
                    isLogin = true;
                });
                
            }
            else
            {
                await Application.Current.Dispatcher.Invoke(async () =>
                {
                    await CurrentWindow.ShowMessageAsync("Hello!", "Wrong username or password.", MessageDialogStyle.Affirmative, mySettings);
                });
            }
        }

    }
}
