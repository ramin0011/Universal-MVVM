using System.Collections.Generic;
using System.Threading.Tasks;

namespace UniMvvm.DataServices.Interfaces
{
    public interface IRequestProvider
    {
        Task<TResult> GetAsync<TResult>(string uri, Dictionary<string, string> headers=null);

        Task<TResult> PostAsync<TResult>(string uri, TResult data, Dictionary<string, string> headers = null);

        Task<TResult> PostAsync<TRequest, TResult>(string uri, TRequest data, Dictionary<string, string> headers = null);

        Task<TResult> PutAsync<TResult>(string uri, TResult data);

        Task<TResult> PutAsync<TRequest, TResult>(string uri, TRequest data);
    }
}
