using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Rework.Models;
using Rework.Windows;
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
    class ClassViewModel: BaseViewModel
    {
        public ICommand AddClassCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        private static ObservableCollection<ClassData> _listClass;
        private static ObservableCollection<string> _grades;
        private string _className;
        private string _gradeName;



        public static ObservableCollection<string> Grades
        {
            get
            {
                return _grades;
            }
            set
            {
                _grades = value;
            }
        }
        public static ObservableCollection<ClassData> ListClass
        {
            get
            {
                return _listClass;
            }
            set
            {
                _listClass = value;
            }
        }
        public string GradeName
        {
            get
            {
                return _gradeName;
            }
            set
            {
                _gradeName = value;
            }
        }
        public string ClassName {
            get {
                return _className;
            }
            set {
                _className = value;
            }

        }

        public ClassViewModel(int _id)
        {
            LoadGrades();
            _className = DataProvider.Ins.DB.classes.Where(x => x.id == _id).ToArray()[0].name;
            int IdGrade = DataProvider.Ins.DB.classes.Where(x => x.id == _id).ToArray()[0].id_grade;
            GradeName = DataProvider.Ins.DB.grades.Where(x => x.id == IdGrade).ToArray()[0].name;
            SaveCommand = new RelayCommand<Window>((p) => { return true; },
                async (p) =>
                {
                    EditClass w = p as EditClass;
                    var mySettings = new MetroDialogSettings()
                    {
                        AffirmativeButtonText = "Ok",
                        ColorScheme = w.MetroDialogOptions.ColorScheme
                    };
                    @class EditingClass = DataProvider.Ins.DB.classes.Where(x => x.id == w.Id).ToArray()[0];
                    EditingClass.name = _className;
                    EditingClass.id_grade = DataProvider.Ins.DB.grades.Where(x => x.name == _gradeName).ToArray()[0].id;
                    DataProvider.Ins.DB.SaveChanges();
                    await w.ShowMessageAsync("Hello!", "Saved successfully.", MessageDialogStyle.Affirmative, mySettings);
                    LoadData();
                    EnrollViewModel.LoadClasses();
                    w.Close();
                });
        }

        public ClassViewModel()
        {
            LoadGrades();
            AddClassCommand = new RelayCommand<UserControl>((p)=> { return true; },
                async (p)=> 
                {
                    MetroWindow CurrentWindow = Application.Current.MainWindow as MetroWindow;
                    var mySettings = new MetroDialogSettings()
                    {
                        AffirmativeButtonText = "Ok",
                        ColorScheme = CurrentWindow.MetroDialogOptions.ColorScheme
                    };

                    var mySettings2 = new MetroDialogSettings()
                    {
                        AffirmativeButtonText = "Yes",
                        NegativeButtonText = "No",
                        ColorScheme = CurrentWindow.MetroDialogOptions.ColorScheme
                    };

                    if (_className == null || _className == "")
                    {
                        await CurrentWindow.ShowMessageAsync("Hello!", "Please fill in every blanks.", MessageDialogStyle.Affirmative, mySettings);
                        return;
                    }

                    if (_gradeName == null || _gradeName == "")
                    {
                        await CurrentWindow.ShowMessageAsync("Hello!", "This grade doesn't exist.", MessageDialogStyle.Affirmative, mySettings);
                        return;
                    }

                    if(DataProvider.Ins.DB.grades.Where(x => x.name == _gradeName).Count() == 0)
                    {
                        await CurrentWindow.ShowMessageAsync("Hello!", "This grade doesn't exist.", MessageDialogStyle.Affirmative, mySettings);
                        return;
                    }

                    MessageDialogResult mr = await CurrentWindow.ShowMessageAsync("Hello!", "Do you want to add class " + _className + " ?", MessageDialogStyle.AffirmativeAndNegative, mySettings2);
                    if (mr == MessageDialogResult.Negative)
                        return;
                    if (DataProvider.Ins.DB.classes.Where(x => x.name == this._className).Count() > 0)
                    {
                        await CurrentWindow.ShowMessageAsync("Hello!", "This class existed.", MessageDialogStyle.Affirmative, mySettings);
                    }
                    else
                    {
                        @class AddingClass = new @class();
                        AddingClass.name = this._className;
                        AddingClass.id_grade = DataProvider.Ins.DB.grades.Where(x => x.name == _gradeName).ToArray()[0].id;
                        DataProvider.Ins.DB.classes.Add(AddingClass);
                        DataProvider.Ins.DB.SaveChanges();
                        await CurrentWindow.ShowMessageAsync("Hello!", "Added successfully.", MessageDialogStyle.Affirmative, mySettings);
                    }
                    LoadData();
                    EnrollViewModel.LoadClasses();
                });

            

            EditCommand = new RelayCommand<Button>((p)=> { return true; },
                (p)=> 
                {
                    int id = Convert.ToInt32(p.Tag);
                    EditClass editClass = new EditClass(id);
                    editClass.ShowDialog();
                });

            DeleteCommand = new RelayCommand<int>((p) => { return true; },
                async (p) =>
                {
                    MetroWindow CurrentWindow = Application.Current.MainWindow as MetroWindow;
                    var mySettings = new MetroDialogSettings()
                    {
                        AffirmativeButtonText = "Ok",
                        ColorScheme = CurrentWindow.MetroDialogOptions.ColorScheme
                    };
                    var mySettings2 = new MetroDialogSettings()
                    {
                        AffirmativeButtonText = "Yes",
                        NegativeButtonText = "No",
                        ColorScheme = CurrentWindow.MetroDialogOptions.ColorScheme
                    };

                    MessageDialogResult mr = await CurrentWindow.ShowMessageAsync("Hello!", "Do you really want to delete this class?", MessageDialogStyle.AffirmativeAndNegative, mySettings2);
                    if(mr == MessageDialogResult.Affirmative)
                    {
                        @class DeletingClass = DataProvider.Ins.DB.classes.Where(x => x.id == p).ToArray()[0];
                        DataProvider.Ins.DB.classes.Remove(DeletingClass);
                        DataProvider.Ins.DB.SaveChanges();
                        await CurrentWindow.ShowMessageAsync("Hello!", "Deleted successfully.", MessageDialogStyle.Affirmative, mySettings);
                        LoadData();
                        EnrollViewModel.LoadClasses();
                    }
                });
            SearchCommand = new RelayCommand<String>((p) => { return true; },
    (p) =>
    {
        if (p == null)
            return;
        List<@class> SearchedClass = DataProvider.Ins.DB.classes.Where<@class>(x => x.name.Contains(p)).Join(
                    DataProvider.Ins.DB.grades,
                    d => d.id_grade,
                    f => f.id,
                    (d, f) => d
                ).ToList();
        LoadData(SearchedClass);
    });
            LoadData();
        }

        public static void LoadGrades()
        {
            if(_grades == null)
            {
                _grades = new ObservableCollection<string>();
            }
            else
            {
                _grades.Clear();
            }

            List<grade> ListGrades = DataProvider.Ins.DB.grades.ToList();
            foreach(grade g in ListGrades)
            {
                _grades.Add(g.name);
            }
        }

        public void LoadData()
        {
            if(_listClass != null)
            {
                _listClass.Clear();
            }
            else
            {
                _listClass = new ObservableCollection<ClassData>();
            }
            List<@class> Classes = DataProvider.Ins.DB.classes.Join(
                    DataProvider.Ins.DB.grades,
                    d => d.id_grade,
                    f => f.id,
                    (d, f) => d
                ).ToList();
            foreach(@class c in Classes)
            {
                ClassData temp = new ClassData();
                temp.Id = c.id;
                temp.ClassName = c.name;
                temp.GradeName = c.grade.name;
                _listClass.Add(temp);
            }
        }

        public void LoadData(List<@class> SearchedClass)
        {
            if (_listClass != null)
            {
                _listClass.Clear();
            }
            else
            {
                _listClass = new ObservableCollection<ClassData>();
            }
            foreach (@class c in SearchedClass)
            {
                ClassData temp = new ClassData();
                temp.Id = c.id;
                temp.ClassName = c.name;
                temp.GradeName = c.grade.name;
                _listClass.Add(temp);
            }
        }

        public class ClassData
        {
            private int id;
            private string className;
            private string gradeName;


            public int Id
            {
                get
                {
                    return id;
                }
                set
                {
                    id = value;
                }
            }
            public string ClassName
            {
                get
                {
                    return className;
                }
                set
                {
                    className = value;
                }
            }
            public string GradeName
            {
                get
                {
                    return gradeName;
                }
                set
                {
                    gradeName = value;
                }
            }
            public ClassData()
            {

            }
        }
    }
}
