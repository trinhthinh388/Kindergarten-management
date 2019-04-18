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
        #endregion



        private NavigationBarVM()
        {
            _welcomeText = "Hi, guest";
        }

        public void resetWelcomeText()
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

    }
}
