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
    public class MainViewModel : BaseViewModel
    {
        private static MainViewModel _ins;
        private string _name;
        private int _id;
        private IDialogCoordinator _dialogCoordinator;

        public string TeacherName
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChange("TeacherName");
            }
        }
        public IDialogCoordinator dialogCoordinator
        {
            get
            {
                return _dialogCoordinator;
            }
            set
            {
                _dialogCoordinator = value;
            }
        }
        public static MainViewModel Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new MainViewModel();
                return _ins;
            }
            set
            {
                _ins = value; 
            }
        }

        public ICommand SettingsCommand { get; set; }
        public ICommand LogOutCommand { get; set; }
        public int Id { get => _id; set => _id = value; }

        private MainViewModel()
        {
            CreateAdminUser();
            SettingViewModel.Init();
            SettingsCommand = new RelayCommand<MetroAnimatedTabControl>((p)=> { return true; },
                (p)=> 
                {
                    p.SelectedIndex = 7;
                });
            LogOutCommand = new RelayCommand<MetroAnimatedTabControl>((p)=> { return true; }, (p)=>
            {
                LogInViewModel.isLogin = false;
                p.SelectedIndex = 0;
            });
        }

        private void CreateAdminUser()
        {
            user newUser = DataProvider.Ins.DB.users.Where(x => x.username == "admin").FirstOrDefault();
            if (newUser == null)
            {
                teacher newTeacher = new teacher();
                newTeacher.name = "admin";
                
                newUser = new user();
                newUser.id_teacher = DataProvider.Ins.DB.teachers.Add(newTeacher).id;
                newUser.username = "admin";
                newUser.password = "admin";

                DataProvider.Ins.DB.users.Add(newUser);
                DataProvider.Ins.DB.SaveChanges();
            }
        }

        public void LoadUserName(int idUser)
        {
            this.Id = idUser;
            int idTeacher = DataProvider.Ins.DB.users.Where(x => x.id == idUser).ToArray()[0].id_teacher;
            this.TeacherName = DataProvider.Ins.DB.teachers.Where(x => x.id == idTeacher).ToArray()[0].name;
        }
    }
}
