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

namespace Rework.Windows
{
    /// <summary>
    /// Interaction logic for EditParent.xaml
    /// </summary>
    public partial class EditParent : MetroWindow
    {
        public int id { get; set; } = 0;
        public EditParent()
        {
            InitializeComponent();
            this.DataContext = new ParentViewModel();
        }

        public EditParent(int _id)
        {
            this.id = _id;
            InitializeComponent();
            this.DataContext = new ParentViewModel();
        }
    }
}
