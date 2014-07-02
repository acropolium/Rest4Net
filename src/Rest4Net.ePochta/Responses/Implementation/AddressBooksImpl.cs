using System.Collections.Generic;

namespace Rest4Net.ePochta.Responses.Implementation
{
    internal class AddressBooksImpl : IAddressBooks
    {
#pragma warning disable 649
        private int count;
        private readonly List<AddressBookImpl> items = new List<AddressBookImpl>();
#pragma warning restore 649

        public int Count
        {
            get { return count; }
        }

        public IEnumerable<IAddressBook> Items
        {
            get
            {
                foreach (var item in items)
                    yield return item;
            }
        }
    }
}
