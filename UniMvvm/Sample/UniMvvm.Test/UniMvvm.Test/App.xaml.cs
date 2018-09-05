using System;
using System.Collections.Generic;
using UniMvvm.Services;
using UniMvvm.Test.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using UniMvvm.Test.Views;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace UniMvvm.Test
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();


            UniMvvmEngine.Run(new UniMvvmOptions()
            {
                MainPage = new MainPage(),
                Mappings = new List<NavigationMapping>()
                {
                    new NavigationMapping(){View = typeof(MainPage),ViewModel = typeof(MainPageVm)},
                    new NavigationMapping(){View = typeof(MenuPage),ViewModel = typeof(MenuViewVm)},
                    new NavigationMapping(){View = typeof(AboutPage),ViewModel = typeof(AboutViewModel)},
                    new NavigationMapping(){View = typeof(ItemDetailPage),ViewModel = typeof(ItemDetailViewModel)},
                    new NavigationMapping(){View = typeof(ItemsPage),ViewModel = typeof(ItemsViewModel)},
                    new NavigationMapping(){View = typeof(NewItemPage),ViewModel = typeof(NewItemVm)},
                }
            });
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
