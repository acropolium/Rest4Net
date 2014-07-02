using System.Collections.Generic;

namespace Rest4Net.ePochta.Responses.Implementation
{
    internal class SendersImpl : ISenders
    {
#pragma warning disable 649
        private int count;
        private readonly List<SenderImpl> items = new List<SenderImpl>();
#pragma warning restore 649

        public int Count
        {
            get { return count; }
        }

        public IEnumerable<ISender> Items
        {
            get
            {
                foreach (var item in items)
                    yield return item;
            }
        }
    }
}
