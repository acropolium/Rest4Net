using System.Collections.Generic;

namespace Rest4Net.Ghost.Responses
{
    public interface IPosts
    {
        IEnumerable<IPost> Items { get; }
        int Page { get; }
        int Limit { get; }
        int Pages { get; }
        int Total { get; }
    }
}
