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
        private IDialogCoordinator _dialogCoordinator;
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

        private MainViewModel()
        {
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
    }
}
