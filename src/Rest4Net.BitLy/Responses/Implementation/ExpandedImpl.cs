using System.Collections.Generic;

namespace Rest4Net.BitLy.Responses.Implementation
{
    internal class ExpandedImpl : IExpanded
    {
#pragma warning disable 649
        private readonly List<BitlyItemImpl> _expand;
#pragma warning restore 649

        public IEnumerable<IBitlyItem> Expand
        {
            get
            {
                foreach (var impl in _expand)
                    yield return impl;
            }
        }
    }
}
