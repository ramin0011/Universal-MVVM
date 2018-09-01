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
        ItemsViewModel viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ItemsViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Item;
            if (item == null)
                return;
            await ((ItemsViewModel)BindingContext).DialogService.ShowAlertAsync(
                $"Great you clicked on {item.Description}", item.Text, "ok");
            await ((ItemsViewModel) BindingContext).NavigationService.NavigateToAsync<ItemDetailViewModel>(new ItemDetailViewModel(item));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await ((ItemsViewModel) BindingContext).NavigationService.NavigateToAsync<ItemDetailViewModel>();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}