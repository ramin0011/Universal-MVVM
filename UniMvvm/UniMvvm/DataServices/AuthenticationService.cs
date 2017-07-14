using System;
using System.Threading.Tasks;
using UniMvvm.DataServices.Interfaces;
using UniMvvm.Helpers;
using UniMvvm.Models.Authentication;

namespace UniMvvm.DataServices
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IRequestProvider _requestProvider;

        public bool IsAuthenticated => !string.IsNullOrEmpty(Settings.AccessToken);

        public AuthenticationService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<bool> LoginAsync(string userName, string password)
        {
            var auth = new AuthenticationRequest
            {
                UserName = userName,
                Credentials = password,
                GrantType = "password",
            };

            UriBuilder builder = new UriBuilder(GlobalSettings.AuthenticationEndpoint);
            builder.Path = "api/login";

            string uri = builder.ToString();

            AuthenticationResponse authenticationInfo = await _requestProvider.PostAsync<AuthenticationRequest, AuthenticationResponse>(uri, auth);
            Settings.UserId = authenticationInfo.UserId;
            Settings.ProfileId = authenticationInfo.ProfileId;
            Settings.AccessToken = authenticationInfo.AccessToken;

            return true;
        }

        public Task LogoutAsync()
        {
            Settings.RemoveUserId();
            Settings.RemoveProfileId();
            Settings.RemoveAccessToken();
            return Task.FromResult(false);
        }

        public int GetCurrentUserId()
        {
            return Settings.UserId;
        }
    }
}

