﻿using Rework.Models;
using Rework.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rework.Content
{
    /// <summary>
    /// Interaction logic for ManageChildren.xaml
    /// </summary>
    public partial class ManageChildren : UserControl
    {
        public ManageChildren()
        {
            InitializeComponent();
            this.DataContext = ManageChildrenViewModel.Ins;

        }

        private void MetroDataGrid_AutoGeneratedColumns(object sender, EventArgs e)
        {
            var grid = (DataGrid)sender;
            ColumnTemplate.DisplayIndex = grid.Columns.Count - 1;
            grid.Columns[1].DisplayIndex = 1;
            grid.Columns[1].Header = "Picture";
            grid.Columns[3].Width = 70;
            grid.Columns[4].Width = 100;
            grid.Columns[8].Visibility = Visibility.Collapsed;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var tag = ((Button)sender).Tag;
            int id = Convert.ToInt32(tag);
            EditChildren editChildren = new EditChildren(id);
            editChildren.ShowDialog();
        }
    }
}
