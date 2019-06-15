using MahApps.Metro.Controls;
using Rework.Models;
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
    /// Interaction logic for EditClass.xaml
    /// </summary>
    public partial class EditClass : MetroWindow
    {
        private int id;
        private List<string> ListGrade;
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }
        public EditClass()
        {
            InitializeComponent();
            this.DataContext = new ClassViewModel(id);
        }
        

        public EditClass(int _id)
        {
            InitializeComponent();
            this.id = _id;
            this.DataContext = new ClassViewModel(id);
        }

    }
}
