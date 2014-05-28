using System.Collections.Generic;
using System.Linq;

namespace Rest4Net.ePochta.Responses.Implementation
{
    internal class PhoneExceptionsImpl : IPhoneExceptions
    {
        private int count;
        private readonly List<PhoneExceptionImpl> items = new List<PhoneExceptionImpl>();

        public int Count
        {
            get { return count; }
        }

        public IList<IPhoneException> Items
        {
            get { return items.Select(x => (IPhoneException)x).ToList(); }
        }
    }
}
