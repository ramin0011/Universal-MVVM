using System;
using System.Collections.Generic;
using System.Text;

namespace UniMvvm.Test.ViewModels
{
    public class MainPageVm : UniMvvm.ViewModels.Base.ViewModelBase
    {
        public async void NavigateToPage(Type page)
        {
            // you can also naviagte to page by sending the page type instead of the view model  type
            await NavigationService.NavigateToPageAsync(page);
        }
    }
}
