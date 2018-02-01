using System;
using System.Collections.Generic;
using UniMvvm.DataServices;
using UniMvvm.DataServices.Base;
using UniMvvm.DataServices.Interfaces;
using UniMvvm.Services;
using UniMvvm.Services.Interfaces;
using Unity;
using Unity.Lifetime;

namespace UniMvvm.ViewModels
{
    public class ViewModelLocator
    {
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

        public static void Init(List<NavigationMapping> mappings)
        {
            if(Instance==null)
                Instance=new ViewModelLocator(mappings);
        }

        protected ViewModelLocator(List<NavigationMapping> mappings)
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

            // view models
            mappings.ForEach(a => _unityContainer.RegisterType(a.ViewModel));
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

        public void RegisterSingleton<TInterface, T>() where T : TInterface
        {
            _unityContainer.RegisterType<TInterface, T>(new ContainerControlledLifetimeManager());
        }
    }
}
