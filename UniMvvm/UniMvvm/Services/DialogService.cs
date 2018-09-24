using System.Threading.Tasks;
using Acr.UserDialogs;
using UniMvvm.Services.Interfaces;

namespace UniMvvm.Services
{
    public class DialogService : IDialogService
    {
        public Task ShowAlertAsync(string message, string title, string okText)
        {
            return UserDialogs.Instance.AlertAsync(message, title, okText: okText);
        }
        public Task ActionSheetAsync(string title, string cancel, string destructive)
        {
            return UserDialogs.Instance.ActionSheetAsync(title,cancel,destructive);
        }

        public IUserDialogs GetInstance()
        {
            return UserDialogs.Instance;
        }
    }
}
