using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Rework.Models;

namespace Rework.ViewModels
{
    public class EnrollViewModel : BaseViewModel
    {
        public ICommand EnrollCommand { get; set; }

        #region Children fields
        private string _childrenName;
        private string _nickName;
        private DateTime _birthDate;
        private DateTime _enrollDate;
        private bool _sex;


        public String ChildrenName
        {
            get
            {
                return _childrenName;
            }
            set
            {
                if (value != "")
                    _childrenName = value;
                OnPropertyChange("ChildrenName");
            }
        }
        public String NickName
        {
            get
            {
                return _nickName;
            }
            set
            {
                if (value != "")
                    _nickName = value;
                OnPropertyChange("NickName");
            }
        }
        public String BirthDate
        {
            get
            {
                return Convert.ToString(_birthDate);
            }
            set
            {
                if (value != "")
                    _birthDate = Convert.ToDateTime(value);
                OnPropertyChange("BirthDate");
            }
        }
        public String EnrollDate
        {
            get
            {
                return Convert.ToString(_enrollDate);
            }
            set
            {
                if (value != "")
                    _enrollDate = Convert.ToDateTime(value);
                OnPropertyChange("EnrollDate");
            }
        }
        public String Sex
        {
            get
            {
                return (_sex == true) ? "Male" : "Female";
            }
            set
            {
                if (value != "")
                    _sex = (value == "Male") ? true : false;
                OnPropertyChange("Sex");
            }
        }

        #endregion

        #region Parent fields

        private string _motherName;
        private string _fatherName;
        private string _address;
        private string _phoneNumber;

        public String MotherName
        {
            get
            {
                return _motherName;
            }
            set
            {
                if (value != "")
                    _motherName = value;
                OnPropertyChange("MotherName");
            }
        }
        public String FatherName
        {
            get
            {
                return _fatherName;
            }
            set
            {
                if (value != "")
                    _fatherName = value;
                OnPropertyChange("FatherName");
            }
        }
        public String Address
        {
            get
            {
                return _address;
            }
            set
            {
                if (value != "")
                    _address = value;
                OnPropertyChange("Addresss");
            }
        }
        public String PhoneNumber
        {
            get
            {
                return this._phoneNumber;
            }
            set
            {
                if (value != "")
                    _phoneNumber = value;
                OnPropertyChange("PhoneNumber");
            }
        }
        #endregion 


        public EnrollViewModel()
        {
            EnrollCommand = new RelayCommand<UserControl>((p) => { return true; },
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
                    parent addingParent = new parent();
                    addingParent.Mothername = this._motherName;
                    addingParent.FatherName = this._fatherName;
                    addingParent.address = this._address;
                    addingParent.phonenumber = this._phoneNumber;

                    child addingChild = new child();
                    addingChild.name = this._childrenName;
                    addingChild.nickname = this._nickName;
                    addingChild.sex = this._sex;
                    addingChild.birthdate = this._birthDate;
                    addingChild.enrolldate = DateTime.Now;

                    if(ChildrenName == "" || NickName == "" || BirthDate == "1/1/0001 12:00:00 AM" || MotherName == "" || FatherName == "" || Address == "" || PhoneNumber == "")
                    {
                        await CurrentWindow.ShowMessageAsync("Hello!", "Please fill in every blanks.", MessageDialogStyle.Affirmative, mySettings);
                        return;
                    }

                    var controller = await MainViewModel.Ins.dialogCoordinator.ShowProgressAsync(MainViewModel.Ins, "Processing", "Progressing all the things, please wait.");
                    controller.SetIndeterminate();

                    await Task.Delay(2000);

                    if (DataProvider.Ins.DB.parents.Where(x => x.FatherName == addingParent.FatherName && x.Mothername == addingParent.Mothername && x.phonenumber == addingParent.phonenumber && x.address == addingParent.address).Count() == 0)
                    {
                        DataProvider.Ins.DB.parents.Add(addingParent);
                        await DataProvider.Ins.DB.SaveChangesAsync();
                    }

                    await controller.CloseAsync();

                    addingChild.id_parent = DataProvider.Ins.DB.parents.Where(x => x.FatherName == addingParent.FatherName && x.Mothername == addingParent.Mothername && x.phonenumber == addingParent.phonenumber && x.address == addingParent.address).ToArray()[0].id;

                    if (DataProvider.Ins.DB.children.Where(x => x.name == addingChild.name && x.id_parent == addingChild.id_parent && x.birthdate == addingChild.birthdate && x.sex == addingChild.sex && x.nickname == addingChild.nickname).Count() > 0)
                    {
                        await CurrentWindow.ShowMessageAsync("Hello!", "This child has already enrolled at school.", MessageDialogStyle.Affirmative, mySettings);
                        return;
                    }
                    else
                    {
                        DataProvider.Ins.DB.children.Add(addingChild);
                        await DataProvider.Ins.DB.SaveChangesAsync();
                        await CurrentWindow.ShowMessageAsync("Hello!", "Enrolled Successfully.", MessageDialogStyle.Affirmative, mySettings);
                    }
                });
        }

        private async void RunProgess()
        {
            
            

        }


        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}
