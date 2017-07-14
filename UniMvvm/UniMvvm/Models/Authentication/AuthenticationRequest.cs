namespace UniMvvm.Models.Authentication
{
    public class AuthenticationRequest
    {
        public string UserName { get; set; }

        public string Credentials { get; set; }

        public string GrantType { get; set; }
    }
}
