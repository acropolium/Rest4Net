using System;

namespace Rest4Net.ePochta.Responses.Implementation
{
    internal class SenderImpl : ISender
    {
        private int id;
        private string name;
        private int status;
        private string country;

        public int Id
        {
            get { return id; }
        }

        public string Name
        {
            get { return name; }
        }

        public ReviewStatus Status
        {
            get { return (ReviewStatus)status; }
        }

        public Country Country
        {
            get { return country.AsCountry(); }
        }

        public override string ToString()
        {
            return String.Format("{0} ({1}): {2}", Name ?? "NONE", Country, Status);
        }
    }
}
