using System.Collections.Generic;

namespace Rest4Net.ePochta.Responses.Implementation
{
    internal class PhonesImpl : IPhones
    {
#pragma warning disable 649
        private int count;
        private readonly List<PhoneImpl> items = new List<PhoneImpl>();
#pragma warning restore 649

        public int Count
        {
            get { return count; }
        }

        public IEnumerable<IPhone> Items
        {
            get
            {
                foreach (var item in items)
                    yield return item;
            }
        }
    }
}
