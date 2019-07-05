using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Rework.Models;
using Rework.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Rework.ViewModels
{
    public class SettingViewModel : BaseViewModel
    {
        private static string[] regulations = { "Class Size", "File Path" };
        private static string filePath;
        private static int classSize;

        public static string FilePath
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
        public static string ClassSize
        {
            get
            {
                return classSize.ToString();
            }
            set
            {
                classSize = Convert.ToInt32(value);
                SaveData();
            }
        }
        public ICommand EditFilePathCommand { get; set; }
        public ICommand EditConditionsCommand { get; set; }

        public SettingViewModel()
        {
            LoadData();
            EditConditionsCommand = new RelayCommand<UserControl>((p) => { return true; },
                (p) =>
                {
                    EditConditions w = new EditConditions();
                    w.ShowDialog();
                });
            EditFilePathCommand = new RelayCommand<UserControl>((p)=> { return true; },
                (p)=> 
                {
                    var dlg = new CommonOpenFileDialog();
                    dlg.Title = "Choose folder to save reports.";
                    dlg.InitialDirectory = filePath;
                    dlg.IsFolderPicker = true;
                    if(dlg.ShowDialog() == CommonFileDialogResult.Ok)
                    {
                        filePath = dlg.FileName;
                        regulation w = DataProvider.Ins.DB.regulations.Where(x => x.content == "File Path").ToArray()[0];
                        w.ValueStr = filePath;
                        DataProvider.Ins.DB.SaveChanges();
                    }

                });
        }

        public static void Init()
        {
            regulation w = new regulation();
            foreach(string m in regulations)
            {
                if (DataProvider.Ins.DB.regulations.Where(x => x.content == m).Count() > 0)
                    continue;
                w.content = m;
                DataProvider.Ins.DB.regulations.Add(w);
                DataProvider.Ins.DB.SaveChanges();
            }
        }

        public static void LoadData()
        {
            if (DataProvider.Ins.DB.regulations.Where(x => x.content == "Class size").Count() == 0)
                return;
            if (DataProvider.Ins.DB.regulations.Where(x => x.content == "File Path").Count() == 0)
                return;
            classSize = DataProvider.Ins.DB.regulations.Where(x => x.content == "Class Size").ToArray()[0].ValueInt;
            filePath = DataProvider.Ins.DB.regulations.Where(x => x.content == "File Path").ToArray()[0].ValueStr;
        }

        public static void SaveData()
        {
            regulation g = DataProvider.Ins.DB.regulations.Where(x => x.content == "Class size").ToArray()[0];
            g.ValueInt = classSize;
            DataProvider.Ins.DB.SaveChanges();
        }
    }
}
