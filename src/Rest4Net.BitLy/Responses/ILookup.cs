using System.Collections.Generic;

namespace Rest4Net.BitLy.Responses
{
    public interface ILookup
    {
        IEnumerable<ILookupItem> Lookup { get; }
    }

    public interface ILookupItem
    {
        string GlobalHash { get; }
        string ShortUrl { get; }
        string Url { get; }
    }
}
