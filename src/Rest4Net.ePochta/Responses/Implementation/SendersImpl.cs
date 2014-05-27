using System.Collections.Generic;
using System.Linq;

namespace Rest4Net.ePochta.Responses.Implementation
{
    internal class SendersImpl : ISenders
    {
        private int count;
        private readonly List<SenderImpl> items = new List<SenderImpl>();

        public int Count
        {
            get { return count; }
        }

        public IList<ISender> Items
        {
            get { return items.Select(x => (ISender)x).ToList(); }
        }
    }
}
