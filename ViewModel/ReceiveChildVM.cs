using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;

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
            AddCommand = new RelayCommand<UserControl>((p)=> { return true; }, (p)=>
            {

                if (ChildName == null || ParentName == null || Address == null || PhoneNumber == null || BirthDate == null || Sex == null || NickName == null || ChildName == "" || ParentName == "" || Address == "" || PhoneNumber == "" || BirthDate == "" || Sex == "" || NickName == "")
                {
                    MessageBox.Show("All the blanks must be filled.");
                    return;
                }
                string phonePattern = @"[0-9]";
                string commit = "Child Name: " + ChildName + "\n" + "Birthdate: " + BirthDate + "\n" + "Nickname: " + NickName + "\n" + "Parent Name: " + ParentName + "\n" + "Address: " + Address + "\n" + "Phone Number: " + PhoneNumber;
                MessageBoxResult mr = MessageBox.Show(commit, "Check the information", MessageBoxButton.YesNo,MessageBoxImage.Question);
                if(mr == MessageBoxResult.Yes)
                {
                    Match match = Regex.Match(PhoneNumber, phonePattern);
                    if (match.Success == false)
                    {
                        MessageBox.Show("Phone number must be a string of number only!");
                        return;
                    }
                    parent Parent = new parent(ParentName, Address, PhoneNumber);

                    this.EnrollDate = DateTime.Now;
                    DateTime birthDate = Convert.ToDateTime(this.BirthDate);
                    bool sex = (this.Sex == "Male") ? true : false;



                    if (Model.DataProvider.Ins.DB.parents.Where(x => x.name == ParentName && x.address == Address && x.phone_number == PhoneNumber).Count() == 0)
                    {
                        Model.DataProvider.Ins.DB.parents.Add(Parent);
                        Model.DataProvider.Ins.DB.SaveChanges();
                    }

                    int idParent = Model.DataProvider.Ins.DB.parents.Where(x => x.name == ParentName && x.address == Address && x.phone_number == PhoneNumber).ToArray()[0].id;

                    if (Model.DataProvider.Ins.DB.children.Where(x => x.name == ChildName && x.nickname == NickName && x.birthdate == birthDate && x.sex == sex && x.id_parent == idParent).Count() > 0)
                    {
                        MessageBox.Show("This child is already enrolled at school");
                        return;
                    }

                    child Child = Model.DataProvider.Ins.DB.children.Add(new child(ChildName, NickName, birthDate, EnrollDate, sex, idParent));

                    Model.DataProvider.Ins.DB.children.Add(Child);

                    Model.DataProvider.Ins.DB.SaveChanges();

                    MessageBox.Show("Enroll succeed", "", MessageBoxButton.OK, MessageBoxImage.None);
                    DashBoardVM.Ins.UpdateDashboard();
                }
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
            
            return false;
        }
    }
}
