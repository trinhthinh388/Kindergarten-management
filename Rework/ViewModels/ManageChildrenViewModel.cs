using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Rework.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Rework.ViewModels
{
    public class ManageChildrenViewModel: BaseViewModel
    {

        private static ManageChildrenViewModel _ins;
        public static ManageChildrenViewModel Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new ManageChildrenViewModel();
                return _ins;
            }
            set
            {
                _ins = value;
            }
        }
        public ICommand SearchCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public ObservableCollection<ChildrenData> _listChildren;

        public ObservableCollection<ChildrenData> ListChildren
        {
            get
            {
                return _listChildren;
            }
            set
            {
                _listChildren = value;
                OnPropertyChange();
            }
        }

        public ManageChildrenViewModel()
        {
            SearchCommand = new RelayCommand<String>((p)=> { return true; },
                (p)=> 
                {
                    if (p == null)
                        return;
                    List<child> SearchedChildren = DataProvider.Ins.DB.children.Where<child> (x => x.name.Contains(p)).Join(
                    DataProvider.Ins.DB.parents,
                        d => d.id_parent,
                        f => f.id,
                        (d, f) => d
                    ).ToList();
                    LoadData(SearchedChildren);
                });

            DeleteCommand = new RelayCommand<int>((p)=> { return true; },
                async (p)=> 
                {
                    if (p == null)
                        return;
                    var CurrentWindow = Application.Current.MainWindow as MetroWindow;
                    var mySettings = new MetroDialogSettings()
                    {
                        AffirmativeButtonText = "Yes",
                        NegativeButtonText = "No",
                        FirstAuxiliaryButtonText = "Ok",
                        ColorScheme = CurrentWindow.MetroDialogOptions.ColorScheme
                    };
                    var mySettings2 = new MetroDialogSettings()
                    {
                        AffirmativeButtonText = "Ok",
                        NegativeButtonText = "No",
                        FirstAuxiliaryButtonText = "Ok",
                        ColorScheme = CurrentWindow.MetroDialogOptions.ColorScheme
                    };
                    MessageDialogResult mR = await CurrentWindow.ShowMessageAsync("Hello!", "Do you really want to delete this child information ?", MessageDialogStyle.AffirmativeAndNegative, mySettings);
                    if(mR == MessageDialogResult.Affirmative)
                    {
                        child DeleteChild = DataProvider.Ins.DB.children.Where(x => x.id == p).ToArray()[0];
                        DataProvider.Ins.DB.children.Remove(DeleteChild);
                        DataProvider.Ins.DB.SaveChanges();
                        await CurrentWindow.ShowMessageAsync("Hello!", "Deleted Successfully.", MessageDialogStyle.Affirmative, mySettings2);
                        LoadData();
                    }
                });

            ListChildren = new ObservableCollection<ChildrenData>();
            LoadData();
        }

        public void LoadData()
        {
            if(this._listChildren == null)
            {
                this._listChildren = new ObservableCollection<ChildrenData>();
            }
            else
            {
                this._listChildren.Clear();
            }
            List<child> listOfChildren = (from o in DataProvider.Ins.DB.children join d in DataProvider.Ins.DB.parents on o.id_parent equals d.id join f in DataProvider.Ins.DB.classes on o.id_class equals f.id select o).ToList(); 
            foreach (child kid in listOfChildren)
            {
                ChildrenData x = new ChildrenData();
                x.id = kid.id;
                x.Name = kid.name;
                x.Sex = (kid.sex == true) ? "Male" : "Female";
                x.ClassName = kid.@class.name;
                x.FatherName = kid.parent.FatherName;
                x.MotherName = kid.parent.Mothername;
                this._listChildren.Add(x);
            }
        }

        public void LoadData(List<child> listOfChildren)
        {
            this._listChildren.Clear();
            foreach (child kid in listOfChildren)
            {
                ChildrenData x = new ChildrenData();
                x.id = kid.id;
                x.Name = kid.name;
                x.Sex = (kid.sex == true) ? "Male" : "Female";
                x.ClassName = "HHH";
                x.FatherName = kid.parent.FatherName;
                x.MotherName = kid.parent.Mothername;
                this._listChildren.Add(x);
            }
        }

        public class ChildrenData:BaseViewModel
        {

            private int _id;
            private String _name;
            private String _sex;
            private String _className;

            private String _fatherName;
            private String _motherName;

            public int id
            {
                get
                {
                    return _id;
                }
                set
                {
                    _id = value;
                    OnPropertyChange("id");
                }
            }
            public String Name
            {
                get
                {
                    return _name;
                }
                set
                {
                    _name = value;
                    OnPropertyChange("Name");
                }
            }
            public String Sex
            {
                get
                {
                    return _sex;
                }
                set
                {
                    _sex = value;
                    OnPropertyChange("Sex");
                }
            }
            public String ClassName
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
            public String FatherName
            {
                get
                {
                    return _fatherName;
                }
                set
                {
                    _fatherName = value;
                    OnPropertyChange("FatherName");
                }
            }
            public String MotherName
            {
                get
                {
                    return _motherName;
                }
                set
                {
                    _motherName = value;
                    OnPropertyChange("MotherName");
                }
            }
            public ChildrenData()
            {}
        }
    }
}
