namespace Rest4Net.IronMq.Responses.Implementation
{
    internal class InfoImpl : IInfo
    {
#pragma warning disable 649
        private string msg;
#pragma warning restore 649

        public string Message
        {
            get { return msg; }
        }
    }
}
