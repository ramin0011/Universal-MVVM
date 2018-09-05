using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UniMvvm.DataServices.Interfaces;
using UniMvvm.Pages;
using UniMvvm.Services.Interfaces;
using UniMvvm.ViewModels;
using UniMvvm.ViewModels.Base;
using Xamarin.Forms;

namespace UniMvvm.Services
{
    public class NavigationMapping
    {
        public NavigationMapping()
        {
            
        }
        public NavigationMapping(Type view,Type viewModel)
        {
            this.View = view;
            this.ViewModel = viewModel;
        }
        public Type ViewModel { get; set; }
        public Type View { get; set; }
    }
    public class NavigationService : INavigationService
    {
        private readonly IAuthenticationService _authenticationService;
        protected readonly Dictionary<Type, Type> _mappings;
        private INavigationService _navigationServiceImplementation;
        private MasterDetailPage LaunchingPage { get; set; }
        private ContentPage LoginPage { get; set; }

        protected Application CurrentApplication
        {
            get { return Application.Current; }
        }

        public NavigationService(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            _mappings = new Dictionary<Type, Type>();
            
            CreateMessengerSubscriptions();
        }

        public Task InitializeAsync(MasterDetailPage mainPage,ContentPage loginPage,bool checkAuthentication=false)
        {
            this.LaunchingPage = mainPage;
            this.LoginPage = loginPage;

            _mappings.Clear();
            if (ViewModelLocator.Mappings != null)
                foreach (var mapping in ViewModelLocator.Mappings)
                {
                    _mappings.Add(mapping.ViewModel, mapping.View);
                }

            if (_authenticationService.IsAuthenticated || !checkAuthentication)
            {
                return InternalNavigateToAsync(_mappings.First(a=>a.Value==LaunchingPage.GetType()).Key, null);
            }
            else
            {
                return InternalNavigateToAsync(_mappings.First(a => a.Value == LoginPage.GetType()).Key, null);
            }
        }

        public Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), null);
        }

        public Task NavigateToAsync<TViewModel>(params object[] parameter) where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), parameter);
        }

        public Task NavigateToAsync(Type viewModelType)
        {
            return InternalNavigateToAsync(viewModelType, null);
        }
        public Task NavigateToPageAsync(Type pageType)
        {
            var viewModelType = GetViewModelTypeForPage(pageType);
            return InternalNavigateToAsync(viewModelType, null);
        }

        public Task NavigateToAsync(Type viewModelType, params object[] parameter)
        {
            return InternalNavigateToAsync(viewModelType, parameter);
        }

        public async Task NavigateBackAsync()
        {
            if (CurrentApplication.MainPage.GetType() == this.LaunchingPage.GetType())
            {
                var mainPage =(MasterDetailPage) CurrentApplication.MainPage;
                await mainPage.Detail.Navigation.PopAsync();
            }
            else if (CurrentApplication.MainPage != null)
            {
                await CurrentApplication.MainPage.Navigation.PopAsync();
            }
        }

        public virtual Task RemoveLastFromBackStackAsync()
        {
            var mainPage = CurrentApplication.MainPage as MasterDetailPage;

            if (mainPage != null)
            {
                mainPage.Detail.Navigation.RemovePage(
                    mainPage.Detail.Navigation.NavigationStack[mainPage.Detail.Navigation.NavigationStack.Count - 2]);
            }

            return Task.FromResult(true);
        }

        protected virtual async Task InternalNavigateToAsync(Type viewModelType,params object[] parameter)
        {
            Page page = CreateAndBindPage(viewModelType, parameter);

            if (page.GetType() == this.LaunchingPage.GetType())
            {
                CurrentApplication.MainPage = new CustomNavigationPage(page)
                {
                };
            }
            else if (page.GetType() == this.LoginPage?.GetType())
            {
                CurrentApplication.MainPage = new CustomNavigationPage(page);
            }
            else if (page is MasterDetailPage)
            {
                var navigationPage = ((MasterDetailPage) page).Detail as CustomNavigationPage;

                if (navigationPage != null)
                {
                    await navigationPage.PushAsync(page);
                }
                else
                {
                    var actionPage = CurrentApplication.MainPage;
                    if (actionPage.Navigation != null && actionPage.Navigation.NavigationStack.LastOrDefault() !=null)
                        actionPage = actionPage.Navigation.NavigationStack.Last();
                    await actionPage.Navigation.PushAsync(page);
                }

                ((MasterDetailPage) page).IsPresented = false;
            }
            else
            {
                var navigationPage = CurrentApplication.MainPage as CustomNavigationPage;

                if (navigationPage != null)
                {
                    await navigationPage.PushAsync(page);
                }
                else
                {
                    var actionPage = CurrentApplication.MainPage;
                    if (actionPage.Navigation != null && actionPage.Navigation.NavigationStack.LastOrDefault() != null)
                        actionPage = actionPage.Navigation.NavigationStack.Last();
                    await actionPage.Navigation.PushAsync(page);
                }
            }

            await (page.BindingContext as ViewModelBase).InitializeAsync(parameter);
        }

        private Type GetPageTypeForViewModel(Type viewModelType)
        {
            if (!_mappings.ContainsKey(viewModelType))
            {
                throw new KeyNotFoundException($"No map for ${viewModelType} was found on navigation mappings");
            }

            return _mappings[viewModelType];
        }
        private Type GetViewModelTypeForPage(Type pageType)
        {
            if (!_mappings.ContainsValue(pageType))
            {
                throw new KeyNotFoundException($"No map for ${pageType} was found on navigation mappings");
            }
            return _mappings.SingleOrDefault(a=>a.Value==pageType).Key;
        }

        protected Page CreateAndBindPage(Type viewModelType,params object[] parameter)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType);

            if (pageType == null)
            {
                throw new Exception($"Mapping type for {viewModelType} is not a page");
            }

            Page page;
            if (parameter != null && parameter.Any())
            {
                try
                {
                    var ctor = pageType.GetConstructor(new[] {parameter[0].GetType()});
                    if (ctor != null)
                        page = ctor.Invoke(new object[] {parameter.First()}) as Page;
                    else
                        // if the page class doesnt have the constructor requested , create the page and pass the parameteres to viewmodel instead
                        page = Activator.CreateInstance(pageType) as Page;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            else
                page = Activator.CreateInstance(pageType) as Page;

            var viewModel = ViewModelLocator.Instance.Resolve(viewModelType);
            if (page.BindingContext == null)
                page.BindingContext = viewModel;

            if (page is MasterDetailPage)
            {
                (page as MasterDetailPage).Detail.BindingContext =
                    ViewModelLocator.Instance.Resolve(GetViewModelTypeForPage((page as MasterDetailPage).Detail.GetType()));
                (page as MasterDetailPage).Master.BindingContext =
                    ViewModelLocator.Instance.Resolve(GetViewModelTypeForPage((page as MasterDetailPage).Master.GetType()));
            }
            if (page is TabbedPage)
            {
                for (var index = 0; index < (page as TabbedPage).Children.Count; index++)
                {
                   (page as TabbedPage).Children[index].BindingContext=
                       ViewModelLocator.Instance.Resolve(GetViewModelTypeForPage((page as TabbedPage).Children[index].GetType()));
                }
            }

            return page;
        }


        private void CreateMessengerSubscriptions()
        {
            // MessagingCenter.Subscribe<ReportedIssue>(this, MessengerKeys.GoBackFromReportRequest, GoBackFromReportRequested);
        }
    }
}