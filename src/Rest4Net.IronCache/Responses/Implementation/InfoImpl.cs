namespace Rest4Net.IronCache.Responses.Implementation
{
    internal class InfoImpl
    {
#pragma warning disable 649
        private string msg;
        private string value;
#pragma warning restore 649

        public string Message
        {
            get { return msg; }
        }
        
        public string Value
        {
            get { return value; }
        }
    }
}
