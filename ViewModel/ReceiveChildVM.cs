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
    public class ReceiveChildVM: BaseViewModel
    {

        public string ChildName { get; set; }
        public string NickName { get; set; }
        public string BirthDate { get; set; }
        public DateTime EnrollDate { get; set; }
        public string Sex { get; set; }
        public string ParentName { get; set; }
        public string  Address { get; set; }
        public string  PhoneNumber { get; set; }

        public ICommand AddCommand { get; set; }

        public ReceiveChildVM()
        {
            AddCommand = new RelayCommand<UserControl>((p)=> { return checkEmpty(); }, (p)=>
            {
                string commit = "Child Name: " + ChildName + "\n" + "Birthdate: " + BirthDate + "\n" + "Nickname: " + NickName + "\n" + "Parent Name: " + ParentName + "\n" + "Address: " + Address + "\n" + "Phone Number: " + PhoneNumber;
                MessageBox.Show(commit, "Check the information", MessageBoxButton.OKCancel);
            });
        }

        FrameworkElement GetWindowParent(UserControl p)
        {
            FrameworkElement parent = p;

            while (parent.Parent != null)
            {
                parent = parent.Parent as FrameworkElement;
            }

            return parent;
        }

        private bool checkEmpty()
        {
            if (ChildName != "" && ParentName != "" && BirthDate != "" && Address != "" && PhoneNumber != "" && BirthDate != null && Sex != null)
                return true;
            return false;
        }
    }
}
