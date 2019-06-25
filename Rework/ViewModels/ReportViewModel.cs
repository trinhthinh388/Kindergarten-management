using Rework.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Objects;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Reflection;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Windows;

namespace Rework.ViewModels
{
    public class ReportViewModel:BaseViewModel
    {
        private string selectedClass;
        private static ObservableCollection<string> className;

        public string SelectedClass
        {
            get
            {
                return selectedClass;
            }
            set
            {
                selectedClass = value;
                OnPropertyChange("SelectedClass");
            }
        }
        public static ObservableCollection<string> ClassName
        {
            get
            {
                return className;
            }
            set
            {
                className = value;
            }
        }

        public ICommand GenerateCommand { get; set; }
        public ReportViewModel()
        {
            LoadClass();

            GenerateCommand = new RelayCommand<UserControl>((p) => { return true; },
                (p) =>
                {
                    var query = (from d in DataProvider.Ins.DB.children
                                 join s in DataProvider.Ins.DB.conditions on d.id_condition equals s.id
                                 join f in DataProvider.Ins.DB.classes on d.id_class equals f.id
                                 where f.name == selectedClass
                                 select new
                                 {
                                     Name = d.name,
                                     Condition = s.name
                                 }).AsQueryable();
                });
        }

        public static void LoadClass()
        {
            if (className == null)
            {
                className = new ObservableCollection<string>();
            }
            else
            {
                className.Clear();
            }

            List<@class> classes = DataProvider.Ins.DB.classes.ToList();
            foreach (@class c in classes)
            {
                className.Add(c.name);
            }
        }

        public void EntityToExcelSheet(string excelFilePath, string sheetname, IQueryable result, ObjectContext ctx)
        {
            Excel.Application oXL;
            Excel.Workbook oWB;
            Excel.Worksheet oSheet;
            Excel.Range oRange;
            try
            {
                oXL = new Excel.Application();

                oXL.Visible = true;
                oXL.DisplayAlerts = false;

                oWB = oXL.Workbooks.Add(Missing.Value);

                oSheet = (Excel.Worksheet)oWB.ActiveSheet;
                oSheet.Name = sheetname;


                DataTable dt = EntityToDataTable(result, ctx);

                int rowCount = 1;
                foreach (DataRow dr in dt.Rows)
                {
                    rowCount += 1;
                    for (int i = 1; i < dt.Columns.Count + 1; i++)
                    {
                        // Add the header the first time through 
                        if (rowCount == 2)
                            oSheet.Cells[1, i] = dt.Columns[i - 1].ColumnName;
                        oSheet.Cells[rowCount, i] = dr[i - 1].ToString();
                    }
                }

                // Resize the columns 
                oRange = oSheet.Range[oSheet.Cells[1, 1], oSheet.Cells[rowCount, dt.Columns.Count]];
                oRange.Columns.AutoFit();

                // Save the sheet and close 
                oSheet = null;
                oRange = null;
                oWB.SaveAs(excelFilePath, Excel.XlFileFormat.xlWorkbookNormal, Missing.Value,
                  Missing.Value, Missing.Value, Missing.Value,
                  Excel.XlSaveAsAccessMode.xlExclusive, Missing.Value,
                  Missing.Value, Missing.Value, Missing.Value);
                oWB.Close(Missing.Value, Missing.Value, Missing.Value);
                oWB = null;
                oXL.Quit();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public DataTable EntityToDataTable(IQueryable result, ObjectContext ctx)
        {
            try
            {
                EntityConnection conn = ctx.Connection as EntityConnection;
                using (SqlConnection SQLCon = new SqlConnection(conn.StoreConnection.ConnectionString))
                {
                    ObjectQuery query = result as ObjectQuery;
                    using (SqlCommand Cmd = new SqlCommand(query.ToTraceString(), SQLCon))
                    {
                        foreach (var param in query.Parameters)
                        {
                            Cmd.Parameters.AddWithValue(param.Name, param.Value);
                        }
                        using (SqlDataAdapter da = new SqlDataAdapter(Cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                da.Fill(dt);
                                return dt;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
