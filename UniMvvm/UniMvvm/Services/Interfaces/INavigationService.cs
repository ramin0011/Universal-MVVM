using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniMvvm.ViewModels.Base;
using Xamarin.Forms;

namespace UniMvvm.Services.Interfaces
{
    public interface INavigationService
    {
        Task InitializeAsync(Page mainPage, ContentPage loginPage,
            bool checkAuthentication = false);
        Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase;

        Task NavigateToAsync<TViewModel>(params object[] parameter) where TViewModel : ViewModelBase;

        Task NavigateToAsync(Type viewModelType);
        Task NavigateToPageAsync(Type pageType);

        Task NavigateToAsync(Type viewModelType,params object[] parameter);

        Task NavigateBackAsync();

        Task RemoveLastFromBackStackAsync();
    }
}