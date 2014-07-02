using System;

namespace Rest4Net.ePochta.Responses.Implementation
{
    internal class SenderImpl : ISender
    {
#pragma warning disable 649
        private int id;
        private string name;
        private int status;
        private string country;
#pragma warning restore 649

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
            get { return CountriesUtils.AsCountry(country); }
        }

        public override string ToString()
        {
            return String.Format("{0} ({1}): {2}", Name ?? "NONE", Country, Status);
        }
    }
}
