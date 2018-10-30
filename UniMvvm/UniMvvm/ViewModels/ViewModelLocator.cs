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

        public static void Init(List<NavigationMapping> mappings, Dictionary<Type, Type> services)
        {
            if (Instance == null)
            {
                Instance=new ViewModelLocator(mappings);
                Instance.RegisterViewModels();
                if(services!=null)
                for (var index = 0; index < services.Count; index++)
                {
                    Instance.Register(services.Keys.ToList()[index],services.Values.ToList()[index],new ContainerControlledLifetimeManager() );
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
            try
            {
                return _unityContainer.Resolve(type);
            }
            catch (Exception e)
            {
                throw;
            }
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
        public void Register(Type @from, Type to, LifetimeManager lifeltime)
        {
            _unityContainer.RegisterType(from,to, lifeltime);
        }

        public void RegisterSingleton<TInterface, T>() where T : TInterface
        {
            _unityContainer.RegisterType<TInterface, T>(new ContainerControlledLifetimeManager());
        }
    }
}
