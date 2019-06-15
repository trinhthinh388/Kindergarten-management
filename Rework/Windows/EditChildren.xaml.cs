using MahApps.Metro.Controls;
using Rework.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Rework
{
    /// <summary>
    /// Interaction logic for EditChildren.xaml
    /// </summary>
    public partial class EditChildren : MetroWindow
    {
        public int id { get; set; } = 0;
        public EditChildren()
        {
            InitializeComponent();
            this.DataContext = new EditChildrenViewModel();
        }

        public EditChildren(int _id)
        {
            this.id = _id;
            InitializeComponent();
            this.DataContext = new EditChildrenViewModel();
        }
    }
}
