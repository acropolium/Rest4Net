using System.Collections.Generic;

namespace Rest4Net.IronCache.Responses.Implementation
{
    internal class DataImpl<TInterface, TClass> where TClass : class, TInterface
    {
#pragma warning disable 649
        private readonly List<TClass> data;
#pragma warning restore 649

        public IEnumerable<TInterface> Data
        {
            get
            {
                foreach (var d in data)
                    yield return d;
            }
        }
    }
}
