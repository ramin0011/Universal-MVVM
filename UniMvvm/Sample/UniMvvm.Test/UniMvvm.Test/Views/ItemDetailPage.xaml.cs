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
        
        public ItemDetailPage(ItemDetailViewModel viewModel)
        {
            InitializeComponent();
            BindingContext =  viewModel;
        }

        public ItemDetailPage()
        {
            InitializeComponent();
            var  vm =new ItemDetailViewModel(new Item()
            {
                Text = "hey",
                Description = "hi"
            });
            BindingContext = vm;
        }
    }
}