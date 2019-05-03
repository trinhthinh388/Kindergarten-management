using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace ViewModel
{
    public class NavigationBarVM: BaseViewModel
    {
        private static NavigationBarVM _ins;
        public static NavigationBarVM Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new NavigationBarVM();
                return _ins;
            }

            set
            {
                _ins = value;
            }
        }

        private string _welcomeText; 
        public string WelcomeText
        {
            get
            {
                return _welcomeText;
            }
            set
            {
                _welcomeText = value;
                OnPropertyChange("WelcomeText");
            }
        }

        #region Commands
        public ICommand DashBoardCommand { get; set; }
        public ICommand ReceiveChildCommand { get; set; }
        public ICommand UpdateChildCommand { get; set; }
        public ICommand ArrangeClassCommand { get; set; }
        public ICommand UpdateRegulationCommand { get; set; }
        public ICommand GenerateReportCommand { get; set; }
        #endregion

        private NavigationBarVM()
        {
            _welcomeText = "Hi, guest";
            DashBoardCommand = new RelayCommand<UserControl>((p)=>
            {
                return true;
            },
            (p)=> 
            {
                Window w = GetWindowParent(p) as Window;
                if (w == null)
                    return;
                foreach (UserControl uc in FindVisualChildren<UserControl>(w))
                {
                    if (uc.Name == "ControlBar" || uc.Name == "NavigationBar")
                        continue;
                    if (uc.Name == "DashBoard")
                    {
                        uc.Visibility = Visibility.Visible;
                        continue;
                    }
                    uc.Visibility = Visibility.Collapsed;
                }
            });
            ReceiveChildCommand = new RelayCommand<UserControl>((p) =>
            {
                return true;
            },
                (p) =>
                {
                    LogInVM logInVM = new LogInVM();
                    if(logInVM.IsLogIn == false)
                    {
                        MessageBox.Show("You don't have permission to access this function!");
                        return;
                    }
                    Window w = GetWindowParent(p) as Window;
                    if (w == null)
                        return;
                    foreach(UserControl uc in FindVisualChildren<UserControl>(w))
                    {
                        if (uc.Name == "ControlBar" || uc.Name == "NavigationBar")
                            continue;
                        if(uc.Name == "ReceiveChild")
                        {
                            uc.Visibility = Visibility.Visible;
                            continue;
                        }
                        uc.Visibility = Visibility.Collapsed;
                    }
                });
        }

        public void ResetWelcomeText()
        {
            WelcomeText = "Hi, guest"; ;
        }

        public void ChangedText(string name)
        {
            WelcomeText = "Hello " + "Mr." + name;
        }

        FrameworkElement GetWindowParent(UserControl p)
        {
            FrameworkElement parent = p;

            while (parent.Parent != null)
            {
                parent = parent.Parent as FrameworkElement;
            }

            return parent;
        }

        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

    }
}
