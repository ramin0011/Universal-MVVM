using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

using UniMvvm.Test.Models;
using UniMvvm.Test.Services;
using UniMvvm.Test.Views;

namespace UniMvvm.Test.ViewModels
{
    public class ItemsViewModel : UniMvvm.ViewModels.Base.ViewModelBase
    {
        public ObservableCollection<Item> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        public MockDataStore DataStore=new MockDataStore();

        public ICommand AddNewItem { get; set; }
        public ItemsViewModel()
        {
            AddNewItem=new Command(AddNewCommand);
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            LoadItemsCommand.Execute(null);
            MessagingCenter.Subscribe<NewItemVm, Item>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Item;
                Items.Add(newItem);
                await DataStore.AddItemAsync(newItem);
            });
        }

        private async void AddNewCommand()
        {
            await NavigationService.NavigateToAsync<NewItemVm>();
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}