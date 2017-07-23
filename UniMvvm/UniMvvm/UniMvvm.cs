using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniMvvm.Helpers;
using UniMvvm.Services;
using UniMvvm.Services.Interfaces;
using UniMvvm.ViewModels;
using Xamarin.Forms;

namespace UniMvvm
{
    public class UniMvvmEngine
    {
        public static Task Run(UniMvvmOptions options)
        {
            GlobalSettings.AuthenticationEndpoint = options.AuthenticationEndpoint;
            ViewModelLocator.Init(options.Mappings);
            var navigationService = ViewModelLocator.Instance.Resolve<INavigationService>();
            return navigationService.InitializeAsync(options.MainPage,options.LoginPage,options.CheckAuthentication);
        }
        public static void AdaptColorsToHexString()
        {
            for (var i = 0; i < Application.Current.Resources.Count; i++)
            {
                var key = Application.Current.Resources.Keys.ElementAt(i);
                var resource = Application.Current.Resources[key];

                if (resource is Color)
                {
                    var color = (Color)resource;
                    Application.Current.Resources.Add(key + "HexString", color.ToHexString());
                }
            }
        }
    }
    public class UniMvvmOptions
    {
        public List<NavigationMapping> Mappings { get; set; }
        public MasterDetailPage MainPage { get; set; }
        public ContentPage LoginPage { get; set; }
        public bool CheckAuthentication { get; set; }= false;
        public string AuthenticationEndpoint { get; set; }
    }
}

