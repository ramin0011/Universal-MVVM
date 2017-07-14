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
    }
}
