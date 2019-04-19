using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ViewModel
{
    public class LogInVM: BaseViewModel
    {

        #region Commands
        public ICommand ExitCommand { get; set; }
        public ICommand LogInCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        #endregion
        private static bool _isLogIn;
        public bool IsLogIn {
            get
            {
                return _isLogIn;
            }

            set
            {
                _isLogIn = value;
            }
        }
        public string Username { get { return _Username; } set { _Username = value; OnPropertyChange(); } }
        public string Password { get { return _Password; } set { _Password = value; OnPropertyChange(); } }


        private string _Username = "";
        private string _Password = "";

        public LogInVM()
        {
            ExitCommand = new RelayCommand<Window>((p)=> { return p == null ? false : true; }, (p)=>
            {
                p.Close();
            });

            LogInCommand = new RelayCommand<Window>((p) =>
            {
                return !IsLogIn;
            },
            (p) =>
            {
                LogIn(p);
            }
            );

            PasswordChangedCommand = new RelayCommand<PasswordBox>((p)=> { return p != null; }, (p)=>
            {
                this.Password = p.Password;
            });

            var a = DataProvider.Ins.DB.users.ToList();
        }

        private void LogIn(Window p)
        {
            if (p == null)
                return;
            _Password = EncodePassword(_Password);
            int validCount = Model.DataProvider.Ins.DB.users.Where(x => x.username == _Username && x.password == _Password).Count();
            if (validCount > 0)
            {
                IsLogIn = true;
                string name = Model.DataProvider.Ins.DB.users.Where(x => x.username == _Username && x.password == _Password).ToArray()[0].teachers.ToArray()[0].name.ToString();
                NavigationBarVM.Ins.ChangedText(name);
                p.Close();
            }
            else
            {
                IsLogIn = false;
                MessageBox.Show("Username or password is wrong!!");
            }

        }

        private static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        private static string EncodePassword(string password)
        {
            byte[] data = System.Text.ASCIIEncoding.ASCII.GetBytes(password);
            string result = System.Convert.ToBase64String(data);
            result = CreateMD5(result);
            return result;
        }
    }
}
