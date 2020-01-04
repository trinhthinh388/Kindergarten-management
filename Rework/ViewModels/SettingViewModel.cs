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
        private string selectedTeacher;
        private List<string> _listTeacher;
        private string _newUsername;
        private string _newTeacherName;
        private int _newPos;
        

        public string SelectedTeacher
        {
            get
            {
                return this.selectedTeacher;
            }
            set
            {
                this.selectedTeacher = value;
            }
        }
        public List<string> ListTeacher
        {
            get
            {
                return this._listTeacher;
            }
            set
            {
                if (_listTeacher == null)
                    _listTeacher = new List<string>();
                this._listTeacher = value;
                OnPropertyChange("ListTeacher");
            }
        }
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
        public ICommand ResetPassCommand { get; set; }
        public ICommand CreateAccountCommand { get; set; }
        public string FullName { get => _fullName; set { _fullName = value; OnPropertyChange("FullName"); } }
        public string Username { get => _username; set { _username = value; OnPropertyChange("Username"); } }

        public string NewUsername { get => _newUsername; set { _newUsername = value; OnPropertyChange("NewUserName"); } }
        public string NewTeacherName { get => _newTeacherName; set { _newTeacherName = value; OnPropertyChange("NewTeacherName"); }}
        public int NewPos { get => _newPos; set { _newPos = value; OnPropertyChange("NewPos"); } }

        private SettingViewModel()
        {
            LoadData();
            GetListTeacher();
            CreateAccountCommand = new RelayCommand<UserControl>((p) => true,async (p) => {
                var CurrentWindow = Application.Current.MainWindow as MetroWindow;
                var mySettings = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "Ok",
                    NegativeButtonText = "No",
                    FirstAuxiliaryButtonText = "Ok",
                    ColorScheme = CurrentWindow.MetroDialogOptions.ColorScheme
                };
                teacher newT = new teacher();
                newT.name = this.NewTeacherName;

                if (this.NewTeacherName == "" || this.NewTeacherName == null || this.NewUsername == "" || this.NewUsername == null)
                {
                    await CurrentWindow.ShowMessageAsync("Hello!", "Please fill in every blank.", MessageDialogStyle.Affirmative, mySettings);
                    return;
                }

                user newU = DataProvider.Ins.DB.users.Where(x => x.username == this.NewUsername).FirstOrDefault();
                if(newU != null)
                {
                    await CurrentWindow.ShowMessageAsync("Hello!", "This username has been taken.", MessageDialogStyle.Affirmative, mySettings);
                    return;
                }

                newU = new user();
                newU.id_teacher = DataProvider.Ins.DB.teachers.Add(newT).id;
                newU.username = this.NewUsername;
                newU.password = "123";
                newU.position = this.NewPos;
                DataProvider.Ins.DB.users.Add(newU);
                DataProvider.Ins.DB.SaveChanges();
                await CurrentWindow.ShowMessageAsync("Hello!", "The new account has been created with the default password is 123.", MessageDialogStyle.Affirmative, mySettings);
                this.NewTeacherName = "";
                this.NewUsername = "";
                this.GetListTeacher();
            });
            ResetPassCommand = new RelayCommand<UserControl>((p) => true,async (p)=> {
                var CurrentWindow = Application.Current.MainWindow as MetroWindow;
                var mySettings = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "Ok",
                    NegativeButtonText = "No",
                    FirstAuxiliaryButtonText = "Ok",
                    ColorScheme = CurrentWindow.MetroDialogOptions.ColorScheme
                };
                user st = DataProvider.Ins.DB.users.Where(x => x.username == this.SelectedTeacher).FirstOrDefault();
                if(st != null)
                {
                    st.password = "123";
                    await DataProvider.Ins.DB.SaveChangesAsync();
                    await CurrentWindow.ShowMessageAsync("Hello!", "The password has been reseted.", MessageDialogStyle.Affirmative, mySettings);
                }
            });
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

        public void GetListTeacher()
        {
            if (ListTeacher == null)
                ListTeacher = new List<string>();
            else
                ListTeacher.Clear();
            foreach(user t in DataProvider.Ins.DB.users.ToArray())
            {
                this.ListTeacher.Add(t.username);
            }
        }
    }
}
