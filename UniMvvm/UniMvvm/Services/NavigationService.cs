﻿using System;
using System.Collections.Generic;
using System.Linq;
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
        public Type ViewModel { get; set; }
        public Type View { get; set; }
    }
    public class NavigationService : INavigationService
    {
        private readonly IAuthenticationService _authenticationService;
        protected readonly Dictionary<Type, Type> _mappings;
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

        public Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), parameter);
        }

        public Task NavigateToAsync(Type viewModelType)
        {
            return InternalNavigateToAsync(viewModelType, null);
        }

        public Task NavigateToAsync(Type viewModelType, object parameter)
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

        protected virtual async Task InternalNavigateToAsync(Type viewModelType, object parameter)
        {
            Page page = CreateAndBindPage(viewModelType, parameter);

            if (page.GetType() == this.LaunchingPage.GetType())
            {
                CurrentApplication.MainPage = page;
            }
            else if (page.GetType() == this.LoginPage.GetType())
            {
                CurrentApplication.MainPage = new CustomNavigationPage(page);
            }
            else if (CurrentApplication.MainPage.GetType() == this.LaunchingPage.GetType())
            {
                var mainPage = CurrentApplication.MainPage as MasterDetailPage;
                var navigationPage = mainPage.Detail as CustomNavigationPage;

                if (navigationPage != null)
                {
                    await navigationPage.PushAsync(page);
                }
                else
                {
                    navigationPage = new CustomNavigationPage(page);
                    mainPage.Detail = navigationPage;
                }

                mainPage.IsPresented = false;
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
                    CurrentApplication.MainPage = new CustomNavigationPage(page);
                }
            }

            await (page.BindingContext as ViewModelBase).InitializeAsync(parameter);
        }

        protected Type GetPageTypeForViewModel(Type viewModelType)
        {
            if (!_mappings.ContainsKey(viewModelType))
            {
                throw new KeyNotFoundException($"No map for ${viewModelType} was found on navigation mappings");
            }

            return _mappings[viewModelType];
        }

        protected Page CreateAndBindPage(Type viewModelType, object parameter)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType);

            if (pageType == null)
            {
                throw new Exception($"Mapping type for {viewModelType} is not a page");
            }

            var page = Activator.CreateInstance(pageType) as Page;
            var viewModel = ViewModelLocator.Instance.Resolve(viewModelType) ;
            page.BindingContext = viewModel;

            return page;
        }


        private void CreateMessengerSubscriptions()
        {
            // MessagingCenter.Subscribe<ReportedIssue>(this, MessengerKeys.GoBackFromReportRequest, GoBackFromReportRequested);
        }
    }
}