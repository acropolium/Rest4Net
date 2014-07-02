namespace Rest4Net.ePochta.Responses.Implementation
{
    internal class CampaignImpl : ICampaign
    {
#pragma warning disable 649
        private int id;
        private string @from;
        private string body;
        private int status;
#pragma warning restore 649

        public int Id
        {
            get { return id; }
        }

        public string Sender
        {
            get { return @from; }
        }

        public string Text
        {
            get { return body; }
        }

        public CampaignStatus Status
        {
            get { return (CampaignStatus)status; }
        }
    }
}
