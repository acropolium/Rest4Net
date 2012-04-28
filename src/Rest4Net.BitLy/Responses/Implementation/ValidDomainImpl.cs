namespace Rest4Net.BitLy.Responses.Implementation
{
    internal class ValidDomainImpl : IValidDomain
    {
        private string _domain;

        private bool _bitlyProDomain;

        public string Domain
        {
            get { return _domain; }
        }

        public bool BitlyProDomain
        {
            get { return _bitlyProDomain; }
        }
    }
}
