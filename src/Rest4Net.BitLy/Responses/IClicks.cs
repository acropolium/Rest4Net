using System.Collections.Generic;

namespace Rest4Net.BitLy.Responses
{
    public interface IClicks
    {
        IEnumerable<IBitlyItem> Clicks { get; }
    }
}
