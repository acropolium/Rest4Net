using System.Collections.Generic;

namespace Rest4Net.BitLy.Responses.Implementation
{
    internal class ExpandedImpl : IExpanded
    {
        private readonly IList<BitlyItemImpl> _expand = new List<BitlyItemImpl>();

        public IEnumerable<IBitlyItem> Expand
        {
            get { return _expand; }
        }
    }
}
