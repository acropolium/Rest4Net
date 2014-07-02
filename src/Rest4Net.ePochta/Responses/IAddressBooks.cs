using System.Collections.Generic;

namespace Rest4Net.ePochta.Responses
{
    public interface IAddressBooks
    {
        int Count { get; }
        IEnumerable<IAddressBook> Items { get; }
    }
}
