using System.Collections.Generic;

namespace Rest4Net.ePochta.Responses
{
    public interface ISenders
    {
        int Count { get; }
        IList<ISender> Items { get; }
    }
}
