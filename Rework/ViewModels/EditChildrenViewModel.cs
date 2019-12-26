using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Rework.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private static ObservableCollection<string> _conditions;
        private static ObservableCollection<string> _classes;

        public static ObservableCollection<string> Conditions
        {
            get
            {
                return _conditions;
            }
            set
            {
                _conditions = value;

            }
        }
        public static ObservableCollection<string> AvailableClasses
        {
            get
            {
                return _classes;
            }
            set
            {
                _classes = value;
            }
        }
        private child Child;
        private parent Parent;
        #region Children fields
        private string _childrenName;
        private string _nickName;
        private DateTime _birthDate;
        private DateTime _enrollDate;
        private bool _sex;
        private string selectedClass;
        private string selectedCondition;
        private string _imageURL;


        public string ImageURL
        {
            get
            {
                return this._imageURL;
            }
            set
            {
                this._imageURL = value;
                OnPropertyChange("ImageURL");
            }
        }
        public string SelectedClass
        {
            get
            {
                return selectedClass;
            }
            set
            {
                selectedClass = value;
            }
        }

        public string SelectedCondition
        {
            get
            {
                return selectedCondition;
            }
            set
            {
                selectedCondition = value;
            }
        }
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
        public ICommand UploadImageCommand { get; set; }

        private EditChildren Window;

        public EditChildrenViewModel()
        {
            LoadClasses();
            LoadConditions();
            UploadImageCommand = new RelayCommand<object>((p)=>true, (p)=>
            {
                var dlg = new OpenFileDialog();
                dlg.Title = "Choose profile picture";
                dlg.InitialDirectory = "";
                dlg.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
        "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
        "Portable Network Graphic (*.png)|*.png";
                dlg.Multiselect = false;
                if (dlg.ShowDialog() == true)
                {
                    this.ImageURL = dlg.FileName;
                }
            });
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
                    Child.imageUrl = this.ImageURL;
                    Child.id_class = DataProvider.Ins.DB.classes.Where(x => x.name == selectedClass).ToArray()[0].id;
                    if(DataProvider.Ins.DB.conditions.Where(x => x.name == selectedCondition).FirstOrDefault() != null)
                        Child.id_condition = DataProvider.Ins.DB.conditions.Where(x => x.name == selectedCondition).FirstOrDefault().id;
                    DataProvider.Ins.DB.SaveChanges();
                    ManageChildrenViewModel.Ins.LoadData();
                    EnrollViewModel.LoadClasses();
                    await Window.ShowMessageAsync("Hello!", "Save changes success.", MessageDialogStyle.Affirmative, mySettings);
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
            Child = DataProvider.Ins.DB.children.Where(x => x.id == Window.id).Join(
                    DataProvider.Ins.DB.parents,
                        d => d.id_parent,
                        f => f.id,
                        (d, f) => d
                    ).ToArray()[0];
            this.selectedClass = DataProvider.Ins.DB.classes.Where(x => x.id == Child.id_class).ToArray()[0].name;
            this._childrenName = Child.name;
            this._nickName = Child.nickname;
            this._birthDate = Child.birthdate;
            this._sex = Child.sex;
            this.ImageURL = Child.imageUrl;
            this._motherName = Child.parent.Mothername;
            this._fatherName = Child.parent.FatherName;
            if(DataProvider.Ins.DB.conditions.Where(x => x.id == Child.id_condition).Count() > 0)
                this.selectedCondition = DataProvider.Ins.DB.conditions.Where(x => x.id == Child.id_condition).ToArray()[0].name;
        }

        public static void LoadClasses()
        {
            if (_classes == null)
            {
                _classes = new ObservableCollection<string>();
            }
            else
            {
                _classes.Clear();
            }

            List<@class> classes = DataProvider.Ins.DB.classes.ToList();
            foreach (@class c in classes)
            {
                _classes.Add(c.name);
            }
        }

        public static void LoadConditions()
        {
            if(_conditions == null)
            {
                _conditions = new ObservableCollection<string>();
            }
            else
            {
                _conditions.Clear();
            }
            List<condition> conditions = DataProvider.Ins.DB.conditions.ToList();
            foreach(condition c in conditions)
            {
                Conditions.Add(c.name);
            }
        }
    }
}
