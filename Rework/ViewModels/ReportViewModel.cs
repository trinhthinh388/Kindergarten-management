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
using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace Rework.ViewModels
{
    public class ReportViewModel : BaseViewModel
    {
        private static string filePath;
        private string selectedClass;
        private static ObservableCollection<Reports> listReport;
        private static ObservableCollection<string> className;


        public static ObservableCollection<Reports> ListReport
        {
            get
            {
                return listReport;
            }
            set
            {
                listReport = value;
            }
        }
        public string FilPath
        {
            get
            {
                return filePath;
            }
            set
            {
                filePath = value;
            }
        }
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
        public ICommand SearchCommand { get; set; }
        public ReportViewModel()
        {
            LoadClass();
            LoadReportHistory();
            GenerateCommand = new RelayCommand<UserControl>((p) => { return true; },
                async (p) =>
                {
                    MetroWindow w = Application.Current.MainWindow as MetroWindow;
                    MetroDialogSettings mySettings = new MetroDialogSettings()
                    {
                        AffirmativeButtonText = "Ok",
                        ColorScheme = w.MetroDialogOptions.ColorScheme
                    };

                    MetroDialogSettings mySettings2 = new MetroDialogSettings()
                    {
                        AffirmativeButtonText = "Yes",
                        NegativeButtonText = "No",
                        ColorScheme = w.MetroDialogOptions.ColorScheme
                    };

                    string fileName = DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + "-" + DateTime.Now.Year.ToString() + "-" + selectedClass;
                    filePath = "C:\\Users\\T\\Desktop\\csv\\" + fileName + ".xlsx";

                    List<ChildrenReport> db = new List<ChildrenReport>();
                    List<child> query = new List<child>();

                    if (File.Exists(filePath))
                    {
                        MessageDialogResult mr = await w.ShowMessageAsync("Hello!", "The file is exist, would you like to overide it?", MessageDialogStyle.AffirmativeAndNegative, mySettings2);
                        if (mr == MessageDialogResult.Negative)
                        {
                            return;
                        }
                    }

                    await Task.Factory.StartNew(async ()=> 
                    {
                        var controller = await MainViewModel.Ins.dialogCoordinator.ShowProgressAsync(MainViewModel.Ins, "Processing", "Proceessing all the things, please wait.");
                        controller.SetIndeterminate();
                        int Id_class = DataProvider.Ins.DB.classes.Where(x => x.name == selectedClass).ToArray()[0].id;
                        query = (from d in DataProvider.Ins.DB.children
                                 join s in DataProvider.Ins.DB.conditions on d.id_condition equals s.id
                                 join f in DataProvider.Ins.DB.classes on d.id_class equals f.id
                                 where f.name == selectedClass
                                 select d
                                    ).ToList();

                        foreach (child c in query)
                        {
                            ChildrenReport temp = new ChildrenReport();
                            temp.Name = c.name;
                            temp.ClassName = c.@class.name;
                            temp.Condition = c.condition.name;
                            db.Add(temp);
                        }
                        report r = new report();

                        if (DataProvider.Ins.DB.reports.Where(x => x.generateDate.Day == DateTime.Now.Day && x.generateDate.Month == DateTime.Now.Month && x.generateDate.Year == DateTime.Now.Year && x.id_class == Id_class).Count() > 0)
                        {
                            r = DataProvider.Ins.DB.reports.Where(x => x.generateDate.Day == DateTime.Now.Day && x.generateDate.Month == DateTime.Now.Month && x.generateDate.Year == DateTime.Now.Year).ToArray()[0];
                            r.generateDate = DateTime.Now;
                        }
                        else
                        {
                            r.id_class = DataProvider.Ins.DB.classes.Where(x => x.name == selectedClass).ToArray()[0].id;
                            r.generateDate = DateTime.Now;
                            DataProvider.Ins.DB.reports.Add(r);
                        }

                        DataProvider.Ins.DB.SaveChanges();
                        if (ExportExcelFile(db, filePath))
                        {
                            await Application.Current.Dispatcher.Invoke(async ()=> 
                            {
                                await controller.CloseAsync();
                                await w.ShowMessageAsync("Hello!", "Exported successfully.", MessageDialogStyle.Affirmative, mySettings);
                            });
                        }
                    });
                });

            SearchCommand = new RelayCommand<string>((p)=> { return true; },
                (p)=> 
                {
                    List<report> listOfReport = (from d in DataProvider.Ins.DB.reports join s in DataProvider.Ins.DB.classes on d.id_class equals s.id select d).Where(x => x.@class.name.Contains(p)).ToList();
                    LoadReportHistory(listOfReport);
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

        public static void LoadReportHistory()
        {
            if(listReport == null)
            {
                listReport = new ObservableCollection<Reports>();
            }
            else
            {
                listReport.Clear();
            }

            List<report> listOfReports = (from d in DataProvider.Ins.DB.reports join s in DataProvider.Ins.DB.classes on d.id_class equals s.id select d).ToList();
            foreach(report item in listOfReports)
            {
                Reports temp = new Reports();
                temp.ClassReport = item.@class.name;
                temp.GeDate = item.generateDate.ToShortDateString();
                listReport.Add(temp);
            }
        }

        public static void LoadReportHistory(List<report> listOfReports)
        {
            if (listReport == null)
            {
                listReport = new ObservableCollection<Reports>();
            }
            else
            {
                listReport.Clear();
            }
            foreach (report item in listOfReports)
            {
                Reports temp = new Reports();
                temp.ClassReport = item.@class.name;
                temp.GeDate = item.generateDate.ToShortDateString();
                listReport.Add(temp);
            }
        }


        public bool ExportExcelFile(List<ChildrenReport> db, string filePath)
        {

            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("Empty path.");
                return false;
            }

            try
            {
                using (ExcelPackage p = new ExcelPackage())
                {
                    p.Workbook.Properties.Author = "Thinh Trinh";

                    p.Workbook.Properties.Title = "Report" + DateTime.Now.ToString();

                    p.Workbook.Worksheets.Add("Working");

                    ExcelWorksheet ws = p.Workbook.Worksheets[1];

                    ws.Name = "ger";

                    ws.Cells.Style.Font.Size = 11;

                    ws.Cells.Style.Font.Name = "Calibri";

                    string[] columnHeader = { "Name", "Class", "Condition" };

                    int countColHeader = columnHeader.Count();

                    ws.Cells[1, 1].Value = "asdfweaf";

                    ws.Cells[1, 1, 1, countColHeader].Merge = true;

                    ws.Cells[1, 1, 1, countColHeader].Style.Font.Bold = true;

                    ws.Cells[1, 1, 1, countColHeader].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    int colIndex = 1;
                    int rowIndex = 2;

                    foreach (var item in columnHeader)
                    {
                        var cell = ws.Cells[rowIndex, colIndex];

                        
                        var fill = cell.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);

                        
                        var border = cell.Style.Border;
                        border.Bottom.Style =
                            border.Top.Style =
                            border.Left.Style =
                            border.Right.Style = ExcelBorderStyle.Thin;

                        
                        cell.Value = item;

                        colIndex++;
                    }

                    foreach (var item in db)
                    {
                        colIndex = 1;

                        rowIndex++;

                        ws.Cells[rowIndex, colIndex++].Value = item.Name;

                        ws.Cells[rowIndex, colIndex++].Value = item.ClassName;

                        ws.Cells[rowIndex, colIndex++].Value = item.Condition;


                    }

                    Byte[] bin = p.GetAsByteArray();
                    File.WriteAllBytes(filePath, bin);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

    }

    public class ChildrenReport
    {
        private string name;
        private string className;
        private string condition;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        public string ClassName
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

        public string Condition
        {
            get
            {
                return condition;
            }
            set
            {
                condition = value;
            }
        }
    }

    public class Reports
    {
        public string GeDate { get; set; }
        public string ClassReport { get; set; }
    }
}
