using System.Threading.Tasks;
using Acr.UserDialogs;

namespace UniMvvm.Services.Interfaces
{
    public interface IDialogService
    {
        Task ShowAlertAsync(string message, string title, string okText);
        Task ActionSheetAsync(string title, string cancel, string destructive);
        IUserDialogs GetInstance();
    }
}
