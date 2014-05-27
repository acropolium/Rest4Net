using System.Collections.Generic;
using System.Linq;

namespace Rest4Net.ePochta.Responses.Implementation
{
    internal class AddressBooksImpl : IAddressBooks
    {
        private int count;
        private readonly List<AddressBookImpl> items = new List<AddressBookImpl>();

        public int Count
        {
            get { return count; }
        }

        public IList<IAddressBook> Items
        {
            get { return items.Select(x => (IAddressBook)x).ToList(); }
        }
    }
}
