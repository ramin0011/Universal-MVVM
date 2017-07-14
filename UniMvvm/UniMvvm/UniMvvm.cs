using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniMvvm.Helpers;
using UniMvvm.Services;
using UniMvvm.Services.Interfaces;
using UniMvvm.ViewModels;
using Xamarin.Forms;

namespace UniMvvm.Engine
{
    public class UniMvvmEngine
    {
        public static Task Run(List<NavigationMapping> mappings, MasterDetailPage mainPage, ContentPage loginPage,
            bool checkAuthentication = false)
        {
            ViewModelLocator.Init(mappings);
            var navigationService = ViewModelLocator.Instance.Resolve<INavigationService>();
            return navigationService.InitializeAsync(mainPage, loginPage, checkAuthentication);
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
}

