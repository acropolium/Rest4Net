namespace Rest4Net.BitLy.Responses.Implementation
{
    internal class AuthenticateImpl : IAuthenticate
    {
        private AuthenticateItemImpl _authenticate;

        public IAuthenticateItem Authenticate
        {
            get { return _authenticate; }
        }
    }

    internal class AuthenticateItemImpl : IAuthenticateItem
    {
        private bool _successful;

        private string _username;

        private string _apiKey;

        public bool Successful
        {
            get { return _successful; }
        }

        public string Username
        {
            get { return _username; }
        }

        public string ApiKey
        {
            get { return _apiKey; }
        }
    }
}
