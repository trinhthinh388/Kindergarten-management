using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Rework.Models;

namespace Rework.ViewModels
{
    public class EnrollViewModel : BaseViewModel
    {
        public ICommand EnrollCommand { get; set; }
        private static ObservableCollection<string> _classes;

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
        private string _className;

        public string MotherName
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
        public string FatherName
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
        public string Address
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
        public string PhoneNumber
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

        public string ClassName
        {
            get
            {
                return _className;
            }
            set
            {
                _className = value;
                OnPropertyChange("ClassName");
            }
        }
        #endregion 


        public EnrollViewModel()
        {
            LoadClasses();
            EnrollCommand = new RelayCommand<UserControl>((p) => { return true; },
                async (p) =>
                {
                    var CurrentWindow = Application.Current.MainWindow as MetroWindow;
                    var mySettings = new MetroDialogSettings()
                    {
                        AffirmativeButtonText = "Ok",
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

                    if (ChildrenName == null || NickName == null || MotherName == null || FatherName == null || Address == null || PhoneNumber == null || _className == null)
                    {
                        await CurrentWindow.ShowMessageAsync("Hello!", "Please fill in every blanks.", MessageDialogStyle.Affirmative, mySettings);
                        return;
                    }

                    if (_className == "")
                    {
                        await CurrentWindow.ShowMessageAsync("Hello!", "Please fill in every blanks.", MessageDialogStyle.Affirmative, mySettings);
                        return;
                    }

                    await Task.Factory.StartNew(()=>CheckingAndAdding(addingParent, addingChild, CurrentWindow, mySettings));
                });
        }


        private async void CheckingAndAdding(parent addingParent, child addingChild, MetroWindow CurrentWindow, MetroDialogSettings mySettings)
        {

            var controller = await MainViewModel.Ins.dialogCoordinator.ShowProgressAsync(MainViewModel.Ins, "Processing", "Proceessing all the things, please wait.");
            controller.SetIndeterminate();
            if (DataProvider.Ins.DB.parents.Where(x => x.FatherName == addingParent.FatherName && x.Mothername == addingParent.Mothername && x.phonenumber == addingParent.phonenumber && x.address == addingParent.address).Count() == 0)
            {
                DataProvider.Ins.DB.parents.Add(addingParent);
                DataProvider.Ins.DB.SaveChanges();
            }

            addingChild.id_parent = DataProvider.Ins.DB.parents.Where(x => x.FatherName == addingParent.FatherName && x.Mothername == addingParent.Mothername && x.phonenumber == addingParent.phonenumber && x.address == addingParent.address).ToArray()[0].id;

           

            if (DataProvider.Ins.DB.children.Where(x => x.name == addingChild.name && x.id_parent == addingChild.id_parent && x.birthdate == addingChild.birthdate && x.sex == addingChild.sex && x.nickname == addingChild.nickname).Count() > 0)
            {
                await Application.Current.Dispatcher.Invoke(async () =>
                {
                    await controller.CloseAsync();
                    await CurrentWindow.ShowMessageAsync("Hello!", "This child has already enrolled at school.", MessageDialogStyle.Affirmative, mySettings);
                });
                return;
            }
            else
            {
                addingChild.id_class = DataProvider.Ins.DB.classes.Where(x => x.name == _className).ToArray()[0].id;
                DataProvider.Ins.DB.children.Add(addingChild);
                DataProvider.Ins.DB.SaveChanges();
                await Application.Current.Dispatcher.Invoke(async () =>
                {
                    await controller.CloseAsync();
                    await CurrentWindow.ShowMessageAsync("Hello!", "Enrolled Successfully.", MessageDialogStyle.Affirmative, mySettings);
                    ManageChildrenViewModel.Ins.LoadData();
                    LoadClasses();
                });
            }
        }

        public static void LoadClasses()
        {
            if(_classes == null)
            {
                _classes = new ObservableCollection<string>();
            }
            else
            {
                _classes.Clear();
            }

            if (DataProvider.Ins.DB.regulations.Where(x => x.content == "Class size").Count() == 0)
                return;

            int classSize = DataProvider.Ins.DB.regulations.Where(x => x.content == "Class size").ToArray()[0].ValueInt;
            
            var classes = (from c in DataProvider.Ins.DB.classes
                           join d in DataProvider.Ins.DB.children on c.id equals d.id_class into sums
                           from sum in sums.DefaultIfEmpty()
                           group sum by new { c.id, c.name } into gr
                           select new
                           {
                               gr.Key.name,
                               Total = gr.Count(x => x != null)
                           }
                           ).ToList();
            foreach (var c in classes)
            {
                if (c.Total < classSize)
                    _classes.Add(c.name);
            }
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
