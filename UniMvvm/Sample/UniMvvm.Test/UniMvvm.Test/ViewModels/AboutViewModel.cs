using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace UniMvvm.Test.ViewModels
{
    public class AboutViewModel : UniMvvm.ViewModels.Base.ViewModelBase
    {
        public AboutViewModel()
        {
            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://xamarin.com/platform")));
        }

        public ICommand OpenWebCommand { get; }
    }
}