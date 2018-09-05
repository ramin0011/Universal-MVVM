using UniMvvm.Test.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniMvvm.Test.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UniMvvm.Test.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        Dictionary<int, Type> MenuPages = new Dictionary<int, Type>();
        public MainPage()
        {
            InitializeComponent();

            MasterBehavior = MasterBehavior.Default;

            MenuPages.Add((int)MenuItemType.Browse, typeof(ItemsPage));
            MenuPages.Add((int)MenuItemType.About, typeof(AboutPage));
        }

        public async Task NavigateFromMenu(int id)
        {
            var newPage = MenuPages[id];

            if (newPage != null && Detail.GetType() != newPage)
            {
                //caling view model directly from view
                ((MainPageVm) BindingContext).NavigateToPage(newPage);
            }
        }
    }
}