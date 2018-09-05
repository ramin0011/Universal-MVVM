using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using UniMvvm.Test.Models;
using UniMvvm.Test.Views;
using UniMvvm.Test.ViewModels;

namespace UniMvvm.Test.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
    {
        public ItemsPage()
        {
            InitializeComponent();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Item;
            if (item == null)
                return;
            await ((ItemsViewModel)BindingContext).DialogService.ShowAlertAsync(
                $"this is powered by Arc.UserDialogs nuget package ","this is a cross platform dialoge view", "ok");
            await ((ItemsViewModel) BindingContext).NavigationService.NavigateToAsync<ItemDetailViewModel>(new ItemDetailViewModel(item));
            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

     
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (((ItemsViewModel) BindingContext).Items.Count == 0)
                ((ItemsViewModel) BindingContext).LoadItemsCommand.Execute(null);
        }
    }
}