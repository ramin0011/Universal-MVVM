using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using UniMvvm.Test.Models;
using Xamarin.Forms;

namespace UniMvvm.Test.ViewModels
{
    public class NewItemVm : UniMvvm.ViewModels.Base.ViewModelBase
    {
        public ICommand SaveItem { get; set; }
        public ICommand Cancel { get; set; }
        public NewItemVm()
        {
            SaveItem=new Command(Save);
            Cancel=new Command(NavBack);
        }

        private async void Save()
        {
            MessagingCenter.Send(this, "AddItem", new Item() { Text =Title,Description = Description});
            await NavigationService.NavigateBackAsync();
        }
        private async void NavBack()
        {
            await NavigationService.NavigateBackAsync();
        }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
