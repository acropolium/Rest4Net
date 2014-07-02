using System;
using Rest4Net.ePochta.Utils;

namespace Rest4Net.ePochta.Responses.Implementation
{
    internal class PhoneExceptionImpl : IPhoneException
    {
#pragma warning disable 649
        private int id;
        private string phone;
        private string added;
        private string comment;
#pragma warning restore 649

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
            get { return DateUtils.ToPochtaDate(added); }
        }

        public string Comment
        {
            get { return comment; }
        }
    }
}
