using System.Collections.Generic;

namespace Rest4Net.ePochta.Responses
{
    public interface ISenders
    {
        int Count { get; }
        IEnumerable<ISender> Items { get; }
    }
}
