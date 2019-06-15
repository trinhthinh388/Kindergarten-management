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
            ListGrade = new List<string>();
            this.DataContext = new ClassViewModel(id);
        }
        

        public EditClass(int _id)
        {
            InitializeComponent();
            this.id = _id;
            ListGrade = new List<string>();
            this.DataContext = new ClassViewModel(id);
        }

        void UpdateGradeList()
        {
            ListGrade.Clear();
            List<grade> Grades = DataProvider.Ins.DB.grades.ToList();
            foreach (grade g in Grades)
            {
                ListGrade.Add(g.name);
            }
            CbGrade.ItemsSource = ListGrade;
        }

        private void EditClassWindow_LayoutUpdated(object sender, EventArgs e)
        {
            UpdateGradeList();
        }
    }
}
