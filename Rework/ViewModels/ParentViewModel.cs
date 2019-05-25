using System;
using System.Collections.Generic;
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

namespace Rework.ViewModels
{
    public class ParentViewModel: BaseViewModel
    {
        public ICommand AddParentCommand { get; set; }

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
            });
        }


    }
}
