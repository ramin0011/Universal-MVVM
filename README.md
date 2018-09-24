# Universal-MVVM
Universal MVVM - Targeting Mulpitle Platofm Applications Developed By C# (Xamarin or UWP)

by using UniMVVM you will have the benefit of pre configured MVVM structure with some multiplatform packages accessable inside your view model classes 

HOW TO USE 

to use this package you only need to define your own views and viewmodels , viewmodels should inherit from UniMvvm.ViewModels.Base.ViewModelBase


then we will handle everything for you, we inject your view models to your views and you can call http requests and open dialoge messages on diffrent platforms and navigate to any page you like just via your viewmodels , all configured inside the UniMVVM .

refer to the sample app for more info.

this is the only code you need to call inside your Xamarin app 
replace this code with the mainpage inside the your App class.

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


