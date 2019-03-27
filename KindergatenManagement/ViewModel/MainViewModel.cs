using KindergatenManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KindergatenManagement.ViewModel
{
    public class MainViewModel: BaseViewModel
    {
        #region properties
        public bool IsLogIn { get; set; }
        private string _Username;
        private string _Password;
        public string Username { get => _Username; set { _Username = value; OnPropertyChanged(); } }
        public string Password { get => _Username; set { _Password = value; OnPropertyChanged(); } }
        #endregion

        #region LogInCommand
        public ICommand LogInCommand { get; set; }
        public ICommand FillPasswordCommand { get; set; }
        #endregion

        public  MainViewModel()
        {
            IsLogIn = false;
            Username = "";
            Password = "";

            LogInCommand = new RelayCommand<Window>(p => { return true; }, p => { LogIn(p); });
            FillPasswordCommand = new RelayCommand<PasswordBox>(p => { return p == null ? false : true; }, p =>
            {
                this.Password = p.Password;
            });
        }


        void LogIn(Window p)
        {
            if (IsLogIn)
                return;
            var _password = MD5Hash(Base64Encode(this.Password));
            bool isValid = DataProvider.Ins.db.USERS.Where(x => x.UserName == this.Username && x.UserPassword == _password).Count() > 0 ? true : false;

            if (isValid)
            {
                IsLogIn = true;
                p.Close();
            }
            else
            {
                MessageBox.Show("Incorrect username or password!");
            }
        }


        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }



        private static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }

    }

}
