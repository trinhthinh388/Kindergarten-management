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
    /// Interaction logic for EditGrade.xaml
    /// </summary>
    public partial class EditGrade : MetroWindow
    {
        private int id;

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
        public EditGrade()
        {
            this.id = 0;
            InitializeComponent();
            this.DataContext = new GradeViewModel(Id);
        }
        public EditGrade(int _id)
        {
            this.id = _id;
            InitializeComponent();
            this.DataContext = new GradeViewModel(Id);
        }
    }
}
