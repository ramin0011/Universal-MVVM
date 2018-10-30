using System.Threading.Tasks;
using UniMvvm.DataServices.Interfaces;
using UniMvvm.Services.Interfaces;

namespace UniMvvm.ViewModels.Base
{
    public abstract class ViewModelBase : ExtendedBindableObject
    {
        private IRequestProvider httProvider;
        public IRequestProvider HttProvider
        {
            get { return httProvider ?? (httProvider = ViewModelLocator.Instance.Resolve<IRequestProvider>()); }
        }

        private IDialogService dialogService;
        public IDialogService DialogService
        {
            get { return dialogService ?? (dialogService = ViewModelLocator.Instance.Resolve<IDialogService>()); }
        }

        private INavigationService navigationService;
        public INavigationService NavigationService
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
                IsNotBusy = !_isBusy;
                RaisePropertyChanged(() => IsBusy);
                RaisePropertyChanged(()=>IsNotBusy);
            }
        }
        public bool IsNotBusy { get; private set; }



        public ViewModelBase()
        {
            IsBusy = false;
            IsNotBusy = true;
        }

        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }
    }
}
