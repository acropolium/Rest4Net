using System.Collections.Generic;
using System.Linq;

namespace Rest4Net.ePochta.Responses.Implementation
{
    internal class PhonesImpl : IPhones
    {
        private int count;
        private readonly List<PhoneImpl> items = new List<PhoneImpl>();

        public int Count
        {
            get { return count; }
        }

        public IList<IPhone> Items
        {
            get { return items.Select(x => (IPhone)x).ToList(); }
        }
    }
}
