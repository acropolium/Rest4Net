using System.Collections.Generic;

namespace Rest4Net.ePochta.Responses.Implementation
{
    internal class PhoneExceptionsImpl : IPhoneExceptions
    {
#pragma warning disable 649
        private int count;
        private readonly List<PhoneExceptionImpl> items = new List<PhoneExceptionImpl>();
#pragma warning restore 649

        public int Count
        {
            get { return count; }
        }

        public IEnumerable<IPhoneException> Items
        {
            get
            {
                foreach (var item in items)
                    yield return item;
            }
        }
    }
}
