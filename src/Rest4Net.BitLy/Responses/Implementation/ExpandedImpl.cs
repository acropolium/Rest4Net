using System.Collections.Generic;

namespace Rest4Net.BitLy.Responses.Implementation
{
    internal class ExpandedImpl : IExpanded
    {
        private readonly List<BitlyItemImpl> _expand;

        public IEnumerable<IBitlyItem> Expand
        {
            get { return _expand; }
        }
    }
}
