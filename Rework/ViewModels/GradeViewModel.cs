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
    public class GradeViewModel: BaseViewModel
    {
        private string gradeName;
        private static ObservableCollection<grade> listGrade;


        public static ObservableCollection<grade> ListGrade
        {
            get
            {
                return listGrade;
            }
            set
            {
                listGrade = value;
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
        public ICommand AddGradeCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        
        public GradeViewModel(int _id)
        {
            gradeName = DataProvider.Ins.DB.grades.Where(x => x.id == _id).ToArray()[0].name;
            SaveCommand = new RelayCommand<MetroWindow>((p)=> { return true; },
                async (p)=> 
                {
                    var mySettings = new MetroDialogSettings()
                    {
                        AffirmativeButtonText = "Ok",
                        ColorScheme = p.MetroDialogOptions.ColorScheme
                    };
                    grade EditingGrade = DataProvider.Ins.DB.grades.Where(x => x.id == _id).ToArray()[0];
                    EditingGrade.name = gradeName;
                    DataProvider.Ins.DB.SaveChanges();
                    await p.ShowMessageAsync("Hello!", "Saved successfully.", MessageDialogStyle.Affirmative, mySettings);
                    p.Close();
                    LoadData();
                });
        }

        public GradeViewModel()
        {
            LoadData();
            AddGradeCommand = new RelayCommand<UserControl>((p)=> { return true; },
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
                    MessageDialogResult mr = await CurrentWindow.ShowMessageAsync("Hello!", "Do you want to add grade" + gradeName + "?", MessageDialogStyle.AffirmativeAndNegative, mySettings2);
                    if(mr == MessageDialogResult.Affirmative)
                    {
                        if(DataProvider.Ins.DB.grades.Where(x=>x.name == gradeName).Count() > 0)
                        {
                            await CurrentWindow.ShowMessageAsync("Hello!", "This grade is already existed.", MessageDialogStyle.Affirmative, mySettings);
                            return;
                        }
                        else
                        {
                            grade AddingGrade = new grade();
                            AddingGrade.name = gradeName;
                            DataProvider.Ins.DB.grades.Add(AddingGrade);
                            DataProvider.Ins.DB.SaveChanges();
                            await CurrentWindow.ShowMessageAsync("Hello!", "Saved successfully.", MessageDialogStyle.Affirmative, mySettings);
                            LoadData();
                        }
                    }
                });

            DeleteCommand = new RelayCommand<int>((p)=> { return true; },
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
                    
                    grade DeletingGrade = DataProvider.Ins.DB.grades.Where(x => x.id == p).ToArray()[0];
                    MessageDialogResult mr = await CurrentWindow.ShowMessageAsync("Hello!", "Do you want to delete grade " + DeletingGrade.name + "?", MessageDialogStyle.AffirmativeAndNegative, mySettings2);
                    if(mr == MessageDialogResult.Affirmative)
                    {
                        if(DeletingGrade.classes.Count() > 0)
                        {
                            await CurrentWindow.ShowMessageAsync("Hello!", "This grade has more than one class. Please delete its classes first.", MessageDialogStyle.Affirmative, mySettings);
                            return;
                        }
                        DataProvider.Ins.DB.grades.Remove(DeletingGrade);
                        DataProvider.Ins.DB.SaveChanges();
                        await CurrentWindow.ShowMessageAsync("Hello!", "Deleted successfully.", MessageDialogStyle.Affirmative, mySettings);
                        LoadData();
                    }
                });

            EditCommand = new RelayCommand<Button>((p)=> { return true; },
                (p)=> 
                {
                    int id = Convert.ToInt32(p.Tag);
                    EditGrade editGrade = new EditGrade(id);
                    editGrade.ShowDialog();
                });

            SearchCommand = new RelayCommand<string>((p) => { return true; },
                (p)=> 
                {
                    List<grade> SearchedGrade = DataProvider.Ins.DB.grades.Where(x => x.name.Contains(p)).ToList();
                    LoadData(SearchedGrade);
                });
        }

        public void LoadData(List<grade> SearchedGrade)
        {
            listGrade.Clear();
            foreach (grade g in SearchedGrade)
            {
                ListGrade.Add(g);
            }
        }

        public void LoadData()
        {
            if(listGrade != null)
            {
                ListGrade.Clear();
            }
            else
            {
                listGrade = new ObservableCollection<grade>();
            }
            List<grade> grades = DataProvider.Ins.DB.grades.ToList();
            foreach(grade g in grades)
            {
                ListGrade.Add(g);
            }
        }
    }
}
