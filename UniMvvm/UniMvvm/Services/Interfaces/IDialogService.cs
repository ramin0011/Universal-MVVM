using System.Threading.Tasks;

namespace UniMvvm.Services.Interfaces
{
    public interface IDialogService
    {
        Task ShowAlertAsync(string message, string title, string okText);
    }
}
