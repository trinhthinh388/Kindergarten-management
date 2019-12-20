﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Rework.Models;
using Rework.Windows;

namespace Rework.ViewModels
{
    public class ParentViewModel: BaseViewModel
    {
        public ICommand SaveCommand { get; set; }
        public ICommand AddParentCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand EditCommand { get; set; }
        
        private static ObservableCollection<parent> _listParent;
        public ObservableCollection<parent> ListParent
        {
            get
            {
                return _listParent;
            }
            set
            {
                _listParent = value;
                OnPropertyChange();
            }
        }
        #region Parent fields

        private string _motherName;
        private string _fatherName;
        private string _address;
        private string _phoneNumber;

        public String MotherName
        {
            get
            {
                return _motherName;
            }
            set
            {
                if (value != "")
                    _motherName = value;
                OnPropertyChange("MotherName");
            }
        }
        public String FatherName
        {
            get
            {
                return _fatherName;
            }
            set
            {
                if (value != "")
                    _fatherName = value;
                OnPropertyChange("FatherName");
            }
        }
        public String Address
        {
            get
            {
                return _address;
            }
            set
            {
                if (value != "")
                    _address = value;
                OnPropertyChange("Addresss");
            }
        }
        public String PhoneNumber
        {
            get
            {
                return this._phoneNumber;
            }
            set
            {
                if (value != "")
                    _phoneNumber = value;
                OnPropertyChange("PhoneNumber");
            }
        }
        #endregion 

        public ParentViewModel()
        {

            SaveCommand = new RelayCommand<MetroWindow>((p) => { return true; },
                async (p) =>
                {
                    string regexString = "^\\+?\\d{1,3}?[- .]?\\(?(?:\\d{2,3})\\)?[- .]?\\d\\d\\d[- .]?\\d\\d\\d\\d$";
                    EditParent editParent = p as EditParent;
                    var mySettings = new MetroDialogSettings()
                    {
                        AffirmativeButtonText = "Ok",
                        NegativeButtonText = "Go away!",
                        FirstAuxiliaryButtonText = "Cancel",
                        ColorScheme = p.MetroDialogOptions.ColorScheme
                    };
                    if (!Regex.IsMatch(this._phoneNumber, regexString))
                    {
                        await p.ShowMessageAsync("Hello!", "Phone number is not valid.", MessageDialogStyle.Affirmative, mySettings);
                        return;
                    }
                    parent EditingParent = DataProvider.Ins.DB.parents.Where(x => x.id == editParent.id).ToArray()[0];
                    EditingParent.FatherName = this._fatherName;
                    EditingParent.Mothername = this._motherName;
                    EditingParent.address = this._address;
                    EditingParent.phonenumber = this._phoneNumber;
                    DataProvider.Ins.DB.SaveChanges();
                    LoadData();
                    ManageChildrenViewModel.Ins.LoadData();
                    await p.ShowMessageAsync("Hello!", "Saved changes successfully.", MessageDialogStyle.Affirmative, mySettings);
                    p.Close();
                });

            AddParentCommand = new RelayCommand<UserControl>((p)=> { return true; },
                async (p)=> 
                {
                    string regexString = "^\\+?\\d{1,3}?[- .]?\\(?(?:\\d{2,3})\\)?[- .]?\\d\\d\\d[- .]?\\d\\d\\d\\d$";
                    MetroWindow CurrentWindow = Application.Current.MainWindow as MetroWindow;
                    var mySettings = new MetroDialogSettings()
                    {
                        AffirmativeButtonText = "Ok",
                        ColorScheme = CurrentWindow.MetroDialogOptions.ColorScheme
                    };
                    if(MotherName == null || FatherName == null || Address == null || PhoneNumber == null)
                    {
                        await CurrentWindow.ShowMessageAsync("Hello!", "Please fill in every blanks.", MessageDialogStyle.Affirmative, mySettings);
                        return;
                    }
                    if(!Regex.IsMatch(this._phoneNumber, regexString))
                    {
                        await CurrentWindow.ShowMessageAsync("Hello!", "Phone number is not valid.", MessageDialogStyle.Affirmative, mySettings);
                        return;
                    }
                    await Task.Factory.StartNew(() => CheckingAndAdding(mySettings, CurrentWindow));
                });
            DeleteCommand = new RelayCommand<int>((p) => { return true; },
                async (p) =>
                {
                    if (p == null)
                        return;
                    var CurrentWindow = Application.Current.MainWindow as MetroWindow;
                    var mySettings = new MetroDialogSettings()
                    {
                        AffirmativeButtonText = "Yes",
                        NegativeButtonText = "No",
                        FirstAuxiliaryButtonText = "Ok",
                        ColorScheme = CurrentWindow.MetroDialogOptions.ColorScheme
                    };
                    var mySettings2 = new MetroDialogSettings()
                    {
                        AffirmativeButtonText = "Ok",
                        NegativeButtonText = "No",
                        FirstAuxiliaryButtonText = "Ok",
                        ColorScheme = CurrentWindow.MetroDialogOptions.ColorScheme
                    };
                    MessageDialogResult mR = await CurrentWindow.ShowMessageAsync("Hello!", "Do you really want to delete this parent information ?", MessageDialogStyle.AffirmativeAndNegative, mySettings);
                    if (mR == MessageDialogResult.Affirmative)
                    {
                        parent DeleteParent = DataProvider.Ins.DB.parents.Where(x => x.id == p).ToArray()[0];
                        if(DeleteParent.children.Count() > 0)
                        {
                            await CurrentWindow.ShowMessageAsync("Hello!", "This parent has at least one child in the school. Please delete their children information first.", MessageDialogStyle.Affirmative, mySettings2);
                            return;
                        }
                        DataProvider.Ins.DB.parents.Remove(DeleteParent);
                        DataProvider.Ins.DB.SaveChanges();
                        await CurrentWindow.ShowMessageAsync("Hello!", "Deleted Successfully.", MessageDialogStyle.Affirmative, mySettings2);
                        LoadData();
                    }
                });
            SearchCommand = new RelayCommand<String>((p) => { return true; },
                (p) =>
                {
                    if (p == null)
                        return;
                    List<parent> SearchedChildren = DataProvider.Ins.DB.parents.Where<parent>(x => x.Mothername.Contains(p) || x.FatherName.Contains(p)).ToList();
                    LoadData(SearchedChildren);
                });
            EditCommand = new RelayCommand<Button>((p) => { return true; },
                (p) =>
                {
                    var tag = p.Tag;
                    int Id = Convert.ToInt32(tag);
                    EditParent editParent = new EditParent(Id);
                    parent EditingParent = DataProvider.Ins.DB.parents.Where(x => x.id == Id).ToArray()[0];
                    ParentViewModel editParentVM = editParent.DataContext as ParentViewModel;
                    editParentVM.MotherName = EditingParent.Mothername;
                    editParentVM.FatherName = EditingParent.FatherName;
                    editParentVM.Address = EditingParent.address;
                    editParentVM.PhoneNumber = EditingParent.phonenumber;
                    editParent.ShowDialog();
                }
                );
            LoadData();
        }

        public static void LoadData()
        {
            if (_listParent == null)
            {
                _listParent = new ObservableCollection<parent>();
            }
            else
            {
                _listParent.Clear();
            }
            List<parent> ListOfParent = DataProvider.Ins.DB.parents.ToList();
            foreach(parent Parent in ListOfParent)
            {
                _listParent.Add(Parent);
            }
        }

        private void LoadData(List<parent> SearchedParent)
        {
            _listParent.Clear();
            foreach (parent Parent in SearchedParent)
            {
                _listParent.Add(Parent);
            }
        }

        private async void CheckingAndAdding(MetroDialogSettings mySettings, MetroWindow CurrentWindow)
        {
            var controller = await MainViewModel.Ins.dialogCoordinator.ShowProgressAsync(MainViewModel.Ins, "Processing", "Proceessing all the things, please wait.");
            controller.SetIndeterminate();
            if (DataProvider.Ins.DB.parents.Where(x => x.Mothername == this._motherName && x.FatherName == this._fatherName && x.address == this._address && x.phonenumber == this._phoneNumber).Count() > 0)
            {
                await Application.Current.Dispatcher.Invoke(async () =>
                {
                    await controller.CloseAsync();
                    await CurrentWindow.ShowMessageAsync("Hello!", "This parent is already in the database.", MessageDialogStyle.Affirmative, mySettings);
                });
                return;
            }
            parent AddingParent = new parent();
            AddingParent.FatherName = this._fatherName;
            AddingParent.Mothername = this._motherName;
            AddingParent.phonenumber = this._phoneNumber;
            AddingParent.address = this._address;

            DataProvider.Ins.DB.parents.Add(AddingParent);
            await Application.Current.Dispatcher.Invoke(async ()=>
            {
                await DataProvider.Ins.DB.SaveChangesAsync();
                await controller.CloseAsync();
                await CurrentWindow.ShowMessageAsync("Hello!", "Added succesfully.", MessageDialogStyle.Affirmative, mySettings);
                LoadData();
            });
        }
    }
}
