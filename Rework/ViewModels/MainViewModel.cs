using MahApps.Metro.Controls.Dialogs;
using Rework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private MainViewModel()
        {
        }

    }
}
