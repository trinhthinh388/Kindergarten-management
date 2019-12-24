using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Rework.Models;
using Rework.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Rework.ViewModels
{
    public class SettingViewModel : BaseViewModel
    {
        private static string[] regulations = { "Class Size", "File Path" };
        private static string filePath;
        private static int classSize;
        private string _fullName;
        private string _username;
        private string _newPassword;
        private bool _userAccessible;
        private static SettingViewModel _ins;
        

        public bool UserAccessible
        {
            get
            {
                return this._userAccessible;
            }
            set
            {
                this._userAccessible = value;
                OnPropertyChange("UserAccessible");
            }
        }
        public static SettingViewModel Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new SettingViewModel();
                return _ins;
            }
            set
            {
                _ins = value;
            }
        }
        public static string FilePath
        {
            get
            {
                return filePath;
            }
            set
            {
                filePath = value;
            }
        }       
        public static string ClassSize
        {
            get
            {
                return classSize.ToString();
            }
            set
            {
                classSize = Convert.ToInt32(value);
                SaveData();
            }
        }
        public string NewPassword
        {
            get
            {
                return this._newPassword;
            }
            set
            {
                this._newPassword = value;
            }
        }
        public ICommand UpdatePasswordCommand { get; set; }
        public ICommand UpdateProfileCommand { get; set; }
        public ICommand EditFilePathCommand { get; set; }
        public ICommand EditConditionsCommand { get; set; }
        public string FullName { get => _fullName; set { _fullName = value; OnPropertyChange("FullName"); } }
        public string Username { get => _username; set { _username = value; OnPropertyChange("Username"); } }

        private SettingViewModel()
        {
            LoadData();
            UpdateProfileCommand = new RelayCommand<UserControl>((p) => true, 
                async (p) => 
                {
                    var CurrentWindow = Application.Current.MainWindow as MetroWindow;
                    var mySettings = new MetroDialogSettings()
                    {
                        AffirmativeButtonText = "Ok",
                        NegativeButtonText = "No",
                        FirstAuxiliaryButtonText = "Ok",
                        ColorScheme = CurrentWindow.MetroDialogOptions.ColorScheme
                    };
                    int idTeacher = DataProvider.Ins.DB.users.Where(x => x.id == MainViewModel.Ins.Id).FirstOrDefault().id_teacher;
                    DataProvider.Ins.DB.teachers.Where(x => x.id == idTeacher).FirstOrDefault().name = this.FullName;
                    await DataProvider.Ins.DB.SaveChangesAsync();
                    await Task.Factory.StartNew(() => MainViewModel.Ins.LoadUserName(MainViewModel.Ins.Id));
                    await CurrentWindow.ShowMessageAsync("Hello!", "Update success.", MessageDialogStyle.Affirmative, mySettings);
                });
            UpdatePasswordCommand = new RelayCommand<UserControl>((p)=> { return true; }, 
                async (p)=>
                {
                    var CurrentWindow = Application.Current.MainWindow as MetroWindow;
                    var mySettings = new MetroDialogSettings()
                    {
                        AffirmativeButtonText = "Ok",
                        NegativeButtonText = "No",
                        FirstAuxiliaryButtonText = "Ok",
                        ColorScheme = CurrentWindow.MetroDialogOptions.ColorScheme
                    };
                    Console.WriteLine(this.NewPassword);
                    if(this.NewPassword == "The password is not match!! shit!!")
                    {
                        await CurrentWindow.ShowMessageAsync("Hello!", "The passwords are not match.", MessageDialogStyle.Affirmative, mySettings);
                    }
                    else
                    {
                        DataProvider.Ins.DB.users.Where(x => x.id == MainViewModel.Ins.Id).FirstOrDefault().password = this.NewPassword;
                        await DataProvider.Ins.DB.SaveChangesAsync();
                        await CurrentWindow.ShowMessageAsync("Hello!", "Update password success.", MessageDialogStyle.Affirmative, mySettings);
                    }
                });
            EditConditionsCommand = new RelayCommand<UserControl>((p) => { return true; },
                (p) =>
                {
                    EditConditions w = new EditConditions();
                    w.ShowDialog();
                });
            EditFilePathCommand = new RelayCommand<UserControl>((p)=> { return true; },
                (p)=> 
                {
                    var dlg = new CommonOpenFileDialog();
                    dlg.Title = "Choose folder to save reports.";
                    dlg.InitialDirectory = filePath;
                    dlg.IsFolderPicker = true;
                    if(dlg.ShowDialog() == CommonFileDialogResult.Ok)
                    {
                        filePath = dlg.FileName;
                        regulation w = DataProvider.Ins.DB.regulations.Where(x => x.content == "File Path").ToArray()[0];
                        w.ValueStr = filePath;
                        DataProvider.Ins.DB.SaveChanges();
                    }

                });

        }

        public static void Init()
        {
            try
            {
                regulation w = new regulation();
                foreach (string m in regulations)
                {
                    if (DataProvider.Ins.DB.regulations.Where(x => x.content == m).Count() > 0)
                        continue;
                    w.content = m;
                    DataProvider.Ins.DB.regulations.Add(w);
                    DataProvider.Ins.DB.SaveChanges();
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            
        }

        public static void LoadData()
        {
            if (DataProvider.Ins.DB.regulations.Where(x => x.content == "Class size").Count() == 0)
                return;
            if (DataProvider.Ins.DB.regulations.Where(x => x.content == "File Path").Count() == 0)
                return;
            classSize = DataProvider.Ins.DB.regulations.Where(x => x.content == "Class Size").ToArray()[0].ValueInt;
            filePath = DataProvider.Ins.DB.regulations.Where(x => x.content == "File Path").ToArray()[0].ValueStr;
            user userCount = DataProvider.Ins.DB.users.Where(x => x.id == MainViewModel.Ins.Id).FirstOrDefault();
            if (userCount != null)
            {
                int idTeacher = userCount.id_teacher;
                SettingViewModel.Ins.Username = userCount.username;
                SettingViewModel.Ins.FullName = DataProvider.Ins.DB.teachers.Where(x => x.id == idTeacher).ToArray()[0].name;
                SettingViewModel.Ins.UserAccessible = userCount.position == 1 ? true : false;
            }

        }

        public static void SaveData()
        {
            regulation g = DataProvider.Ins.DB.regulations.Where(x => x.content == "Class size").ToArray()[0];
            g.ValueInt = classSize;
            DataProvider.Ins.DB.SaveChanges();
            EnrollViewModel.LoadClasses();
        }
    }
}
