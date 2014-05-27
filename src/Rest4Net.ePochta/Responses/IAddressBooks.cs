using System.Collections.Generic;

namespace Rest4Net.ePochta.Responses
{
    public interface IAddressBooks
    {
        int Count { get; }
        IList<IAddressBook> Items { get; }
    }
}
