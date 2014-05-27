namespace Rest4Net.ePochta.Responses.Implementation
{
    internal class CampaignImpl : ICampaign
    {
        private int id;
        private string @from;
        private string body;
        private int status;

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
