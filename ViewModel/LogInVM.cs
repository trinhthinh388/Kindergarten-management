using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ViewModel
{
    public class LogInVM: BaseViewModel
    {
        #region Commands
        public ICommand ExitCommand { get; set; }
        public ICommand LogInCommand { get; set; }
        #endregion
        public LogInVM()
        {
            ExitCommand = new RelayCommand<Window>((p)=> { return p == null ? false : true; }, (p)=>
            {
                p.Close();
            });
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
