using System.Collections.Generic;

namespace Rest4Net.BitLy.Responses.Implementation
{
    internal class ClicksImpl : IClicks
    {
        private readonly List<BitlyItemImpl> _clicks;

        public IEnumerable<IBitlyItem> Clicks
        {
            get { return _clicks; }
        }
    }
}
