using System;
using Rest4Net.ePochta.Utils;

namespace Rest4Net.ePochta.Responses.Implementation
{
    internal class PhoneExceptionImpl : IPhoneException
    {
        private int id;
        private string phone;
        private string added;
        private string comment;

        public int Id
        {
            get { return id; }
        }

        public string Phone
        {
            get { return phone; }
        }

        public DateTime DateOfCreation
        {
            get { return added.ToPochtaDate(); }
        }

        public string Comment
        {
            get { return comment; }
        }
    }
}
