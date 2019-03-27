using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KindergatenManagement.ViewModel
{
    public class ControlBarViewModel: BaseViewModel
    {
        #region commands
        public ICommand CloseWindowCommand { get; set; }
        public ICommand MinimizeWindowCommand { get; set; }
        public ICommand DragMoveWindowCommand { get; set; }
        #endregion

        public ControlBarViewModel()
        {
            CloseWindowCommand = new RelayCommand<UserControl>((p)=> { return p == null ? false : true; }, (p)=> 
            {
               var window =  GetWindowParent(p) as Window;
                if(window != null)
                {
                    window.Close();
                }
            });
            MinimizeWindowCommand = new RelayCommand<UserControl>((p)=> { return p == null ? false : true; }, (p)=>
            {
                var window = GetWindowParent(p) as Window;
                if (window != null)
                {
                    if (window.WindowState == WindowState.Normal)
                        window.WindowState = WindowState.Minimized;
                    else
                        window.WindowState = WindowState.Normal;
                }
            });
            DragMoveWindowCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; }, (p) =>
            {
                var window = GetWindowParent(p) as Window;
                if (window != null)
                {
                    window.DragMove();
                }
            });
        }

        private FrameworkElement GetWindowParent(UserControl p)
        {
            FrameworkElement parent = p;
            while(parent.Parent != null)
            {
                parent = parent.Parent as FrameworkElement;
            }
            return parent;
        }
    }
}
