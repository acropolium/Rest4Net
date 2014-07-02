namespace Rest4Net.BitLy.Responses.Implementation
{
    internal class AuthenticateImpl : IAuthenticate
    {
#pragma warning disable 649
        private AuthenticateItemImpl _authenticate;
#pragma warning restore 649

        public IAuthenticateItem Authenticate
        {
            get { return _authenticate; }
        }
    }

    internal class AuthenticateItemImpl : IAuthenticateItem
    {
#pragma warning disable 649
        private bool _successful;
        private string _username;
        private string _apiKey;
#pragma warning restore 649

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
