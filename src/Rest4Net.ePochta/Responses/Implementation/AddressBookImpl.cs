using System;
using Rest4Net.ePochta.Utils;

namespace Rest4Net.ePochta.Responses.Implementation
{
    internal class AddressBookImpl : IAddressBook
    {
#pragma warning disable 649
        private int id;
        private string name;
        private string description;
        private int phones;
        private int exceptions;
        private string creationdate;
        private string date;
#pragma warning restore 649

        public int Id
        {
            get { return id; }
        }

        public string Name
        {
            get { return name; }
        }

        public string Description
        {
            get { return description; }
        }

        public int PhonesCount
        {
            get { return phones; }
        }

        public int ExceptionsCount
        {
            get { return exceptions; }
        }

        public DateTime Created
        {
            get { return DateUtils.ToPochtaDate(creationdate ?? date); }
        }
    }
}
