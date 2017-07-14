using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniMvvm.ViewModels.Base;
using Xamarin.Forms;

namespace UniMvvm.Services.Interfaces
{
    public interface INavigationService
    {
        Task InitializeAsync(MasterDetailPage mainPage, ContentPage loginPage,
            bool checkAuthentication = false);
        Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase;

        Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase;

        Task NavigateToAsync(Type viewModelType);

        Task NavigateToAsync(Type viewModelType, object parameter);

        Task NavigateBackAsync();

        Task RemoveLastFromBackStackAsync();
    }
}