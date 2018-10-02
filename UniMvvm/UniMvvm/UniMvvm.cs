using System;
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
            ViewModelLocator.Init(options.Mappings,options.Services);
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

        public static void Register<T>(T instance)
        {
             ViewModelLocator.Instance.Register<T>(instance);
        }

        public static void Register<TInterface, T>() where T : TInterface
        {
             ViewModelLocator.Instance.Register<TInterface,T>();
        }

        public static object Resolve(Type type)
        {
           return ViewModelLocator.Instance.Resolve(type);
        }
    }
    public class UniMvvmOptions
    {
        public List<NavigationMapping> Mappings { get; set; }
        public Dictionary<Type,Type> Services { get; set; }
        public Page MainPage { get; set; }
        public ContentPage LoginPage { get; set; }
        public bool CheckAuthentication { get; set; }= false;
        public string AuthenticationEndpoint { get; set; }
    }
}

