using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Rework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Rework.ViewModels
{
    class EditChildrenViewModel:BaseViewModel
    {
        private child Child;
        private parent Parent;
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

        public ICommand SaveCommand { get; set; }

        private EditChildren Window;

        public EditChildrenViewModel()
        {
            SaveCommand = new RelayCommand<object>((p)=> { return true; },
                async (p)=> 
                {
                    var mySettings = new MetroDialogSettings()
                    {
                        AffirmativeButtonText = "Ok",
                        NegativeButtonText = "Go away!",
                        FirstAuxiliaryButtonText = "Cancel",
                        ColorScheme = Window.MetroDialogOptions.ColorScheme
                    };
                    Child.name = this._childrenName;
                    Child.sex = this._sex;
                    Child.birthdate = this._birthDate;
                    Child.nickname = this._nickName;
                    DataProvider.Ins.DB.SaveChanges();
                    ManageChildrenViewModel.Ins.LoadData();
                    await Window.ShowMessageAsync("Hello!", "Saved changes successfully.", MessageDialogStyle.Affirmative, mySettings);
                    Window.Close();
                });

            foreach(Window window in Application.Current.Windows)
            {
                if(window.Name == "EditChildrenWindow")
                {
                    this.Window = window as EditChildren;
                    break;
                }
            }

            Child = DataProvider.Ins.DB.children.Where(x => x.id == Window.id).ToArray()[0];
            this._childrenName = Child.name;
            this._nickName = Child.nickname;
            this._birthDate = Child.birthdate;
            this._sex = Child.sex;
        }
    }
}
