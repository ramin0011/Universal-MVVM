using System;
using System.Collections.Generic;
using System.Linq;
using UniMvvm.DataServices;
using UniMvvm.DataServices.Base;
using UniMvvm.DataServices.Interfaces;
using UniMvvm.Services;
using UniMvvm.Services.Interfaces;
using Unity;
using Unity.Lifetime;

namespace UniMvvm.ViewModels
{
    internal class ViewModelLocator
    {
        private ViewModelLocator()
        {
            
        }
        private readonly IUnityContainer _unityContainer;
        public static List<NavigationMapping> Mappings { get; private set; }
        private static  ViewModelLocator _instance=null;

        public static ViewModelLocator Instance
        {
            get
            {
                return _instance;
            }
            private set { _instance = value; }
        }

        public static void Init(List<NavigationMapping> mappings, Dictionary<Type, Type> optionsServices)
        {
            if (Instance == null)
            {
                Instance=new ViewModelLocator(mappings);
                Instance.RegisterViewModels();
                for (var index = 0; index < optionsServices.Count; index++)
                {
                    Instance.Register(optionsServices.Keys.ToList()[index],optionsServices.Values.ToList()[index]);
                }
            }
        }

       
        private ViewModelLocator(List<NavigationMapping> mappings)
        {
            Mappings = mappings;
            _unityContainer = new UnityContainer();

            // providers
            _unityContainer.RegisterType<IRequestProvider, RequestProvider>();
       
            // services
            _unityContainer.RegisterType<IDialogService, DialogService>();
            RegisterSingleton<INavigationService, NavigationService>();

            // data services
            _unityContainer.RegisterType<IAuthenticationService, AuthenticationService>();
        }

        private void RegisterViewModels()
        {
            // view models
            Mappings?.ForEach(a => _unityContainer.RegisterType(a.ViewModel));
        }
        public T Resolve<T>()
        {
            return _unityContainer.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return _unityContainer.Resolve(type);
        }

        public void Register<T>(T instance)
        {
            _unityContainer.RegisterInstance<T>(instance);
        }

        public void Register<TInterface, T>() where T : TInterface
        {
            _unityContainer.RegisterType<TInterface, T>();
        }
        public void Register(Type from,Type to) 
        {
            _unityContainer.RegisterType(from,to);
        }

        public void RegisterSingleton<TInterface, T>() where T : TInterface
        {
            _unityContainer.RegisterType<TInterface, T>(new ContainerControlledLifetimeManager());
        }
    }
}
