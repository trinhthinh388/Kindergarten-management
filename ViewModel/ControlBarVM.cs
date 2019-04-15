using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ViewModel
{
    public class ControlBarVM: BaseViewModel
    {
        #region commands
        public ICommand CloseWindowCommand { get; set; }
        public ICommand MaximizeWindowCommand  { get; set; }
        public ICommand MinimizeWindowCommand { get; set; }
        #endregion

        public ControlBarVM()
        {
            CloseWindowCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; }, (p) => {
                FrameworkElement window = GetWindowParent(p);
                var w = window as Window;
                if (w != null)
                {
                    w.Close();
                }
            });

            MaximizeWindowCommand = new RelayCommand<UserControl>((p) => { return true; },p => CloseWindow(p));

            MinimizeWindowCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) =>
            {
                FrameworkElement window = GetWindowParent(p);
                var w = window as Window;
                if (w != null)
                {
                    if (w.WindowState != System.Windows.WindowState.Minimized)
                        w.WindowState = System.Windows.WindowState.Minimized;
                }
            });
        }

        void CloseWindow(UserControl p)
        {
            FrameworkElement window = GetWindowParent(p);
            var w = window as Window;
            if (w != null)
            {
                if (w.WindowState == System.Windows.WindowState.Normal)
                    w.WindowState = System.Windows.WindowState.Maximized;
                else if (w.WindowState == System.Windows.WindowState.Maximized)
                    w.WindowState = System.Windows.WindowState.Normal;
            }
        }

        FrameworkElement GetWindowParent(UserControl p)
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
