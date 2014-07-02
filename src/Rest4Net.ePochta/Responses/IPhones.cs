using System.Collections.Generic;

namespace Rest4Net.ePochta.Responses
{
    public interface IPhones
    {
        int Count { get; }
        IEnumerable<IPhone> Items { get; }
    }
}
