using System.Collections.Generic;

namespace Rest4Net.BitLy.Responses
{
    public interface IExpanded
    {
        IEnumerable<IBitlyItem> Expand { get; }
    }
}
