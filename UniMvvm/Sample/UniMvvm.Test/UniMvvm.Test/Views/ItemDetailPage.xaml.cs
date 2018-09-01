using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using UniMvvm.Test.Models;
using UniMvvm.Test.ViewModels;

namespace UniMvvm.Test.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemDetailPage : ContentPage
    {
        ItemDetailViewModel viewModel;
        private Item Item;
        
        public ItemDetailPage(ItemDetailViewModel viewModel)
        {
            InitializeComponent();
            this.Item = viewModel.Item;
            BindingContext = this.viewModel = viewModel;
        }

    }
}