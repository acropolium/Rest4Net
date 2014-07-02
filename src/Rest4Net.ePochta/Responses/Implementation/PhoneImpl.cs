using System;
using System.Collections.Generic;

namespace Rest4Net.ePochta.Responses.Implementation
{
    internal class PhoneImpl : IPhone
    {
#pragma warning disable 649
        private int id;
        private int addressbook;
        private string phone;
        private string normalphone;
        private string variables;
        private int status;
        private string code;
#pragma warning restore 649

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

        public IDictionary<string, bool> Variables
        {
            get
            {
                var d = new Dictionary<string, bool>();
                foreach (var item in (variables ?? String.Empty).Split(';'))
                    d[item] = true;
                return d;
            }
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
            var keys = Variables.Keys;
            var vars = new string[keys.Count];
            var i = 0;
            foreach (var key in keys)
                vars[i++] = key;
            return String.Format("{0} {1}", NormalPhone, String.Join(";", vars));
        }
    }
}
