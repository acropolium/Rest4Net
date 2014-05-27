using System;
using System.Collections.Generic;

namespace Rest4Net.ePochta.Responses.Implementation
{
    internal class PhoneImpl : IPhone
    {
        private int id;
        private int addressbook;
        private string phone;
        private string normalphone;
        private string variables;
        private int status;
        private string code;

        public int Id
        {
            get { return id; }
        }

        public int AddressbookId
        {
            get { return addressbook; }
        }

        public string Phone
        {
            get { return phone; }
        }

        public string NormalPhone
        {
            get { return normalphone; }
        }

        public HashSet<string> Variables
        {
            get { return new HashSet<string>((variables ?? String.Empty).Split(';')); }
        }

        public int Status
        {
            get { return status; }
        }

        public string Code
        {
            get { return code; }
        }

        public override string ToString()
        {
            return String.Format("{0} {1}", NormalPhone, String.Join(";", Variables));
        }
    }
}
