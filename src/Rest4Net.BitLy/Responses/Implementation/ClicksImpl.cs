using System.Collections.Generic;

namespace Rest4Net.BitLy.Responses.Implementation
{
    internal class ClicksImpl : IClicks
    {
#pragma warning disable 649
        private readonly List<BitlyItemImpl> _clicks;
#pragma warning restore 649

        public IEnumerable<IBitlyItem> Clicks
        {
            get
            {
                foreach (var click in _clicks)
                {
                    yield return click;
                }
            }
        }
    }
}
