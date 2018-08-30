using System.Threading.Tasks;
using UniMvvm.DataServices.Interfaces;
using UniMvvm.Services.Interfaces;

namespace UniMvvm.ViewModels.Base
{
    public abstract class ViewModelBase : ExtendedBindableObject
    {
        private IRequestProvider httProvider;
        protected IRequestProvider HttProvider
        {
            get { return httProvider ?? (httProvider = ViewModelLocator.Instance.Resolve<IRequestProvider>()); }
        }

        private IDialogService dialogService;
        protected IDialogService DialogService
        {
            get { return dialogService ?? (dialogService = ViewModelLocator.Instance.Resolve<IDialogService>()); }
        }

        private INavigationService navigationService;
        protected INavigationService NavigationService
        {
            get { return navigationService ?? (navigationService = ViewModelLocator.Instance.Resolve<INavigationService>()); }
        }

        private bool _isBusy;

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }

            set
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

       

        public ViewModelBase()
        {
        }

        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }
    }
}
