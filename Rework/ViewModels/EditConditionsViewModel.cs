using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Rework.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Rework.ViewModels
{
    public class EditConditionsViewModel: BaseViewModel
    {
        private static ObservableCollection<condition> conditions;

        public static ObservableCollection<condition> Conditions
        {
            get
            {
                return conditions;
            }
            set
            {
                conditions = value;
            }
        }
        public ICommand DeleteCommand { get; set; }
        public ICommand SaveCommand { get; set; }

        public EditConditionsViewModel()
        {
            LoadData();
            DeleteCommand = new RelayCommand<int>((p)=> 
            {
                if (p != 0)
                    return true;
                else
                    return false;
            },
                (p)=> 
                {
                    condition w = DataProvider.Ins.DB.conditions.Where(x => x.id == p).ToArray()[0];
                    DataProvider.Ins.DB.conditions.Remove(w);
                    DataProvider.Ins.DB.SaveChanges();
                    LoadData();
                    EditChildrenViewModel.LoadConditions();
                });
            SaveCommand = new RelayCommand<MetroWindow>((p)=> { return true; },
                async (p) =>
                {
                    var mySettings = new MetroDialogSettings()
                    {
                        AffirmativeButtonText = "Ok",
                        ColorScheme = p.MetroDialogOptions.ColorScheme
                    };
                    DataProvider.Ins.DB.conditions.AddRange(conditions.Where(x => x.id == 0).ToList());
                    DataProvider.Ins.DB.SaveChanges();
                    LoadData();
                    EditChildrenViewModel.LoadConditions();
                    await p.ShowMessageAsync("Hello!", "Saved successfully.", MessageDialogStyle.Affirmative, mySettings);
                    p.Close();
                });  
        }


        public static void LoadData()
        {
            if(conditions == null)
            {
                conditions = new ObservableCollection<condition>();
            }
            else
            {
                conditions.Clear();
            }
            foreach (condition c in DataProvider.Ins.DB.conditions.ToList())
            {
                conditions.Add(c);
            }
        }
    }
}
