namespace Rest4Net.BitLy.Responses.Implementation
{
    internal class ValidDomainImpl : IValidDomain
    {
#pragma warning disable 649
        private string _domain;
        private bool _bitlyProDomain;
#pragma warning restore 649

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
