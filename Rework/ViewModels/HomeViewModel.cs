using MahApps.Metro.Controls;
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
    public class HomeViewModel:BaseViewModel
    {
        public ICommand ManageChildrenCommand { get; set; }

        public HomeViewModel()
        {
            ManageChildrenCommand = new RelayCommand<UserControl>((p)=> { return true; },
                (p)=>
                {
                    MetroAnimatedTabControl w = GetWindowParent(p, "MainTabControl") as MetroAnimatedTabControl;
                    if (w == null)
                        return;
                    w.SelectedIndex = 1;
                });
        }

        FrameworkElement GetWindowParent(UserControl p, string name)
        {
            FrameworkElement parent = p as FrameworkElement;
            if (p == null)
                return null;
            while(parent.Parent != null)
            {
                if (parent.Name == name)
                    return parent;
                parent = parent.Parent as FrameworkElement;
            }
            return parent;
        }
    }
}
