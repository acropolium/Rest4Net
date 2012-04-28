using System.Collections.Generic;

namespace Rest4Net.IronMq.Responses.Implementation
{
    internal class DataImpl<TInterface, TClass> where TClass : class, TInterface
    {
        private readonly IList<TClass> data = new List<TClass>();

        public IEnumerable<TInterface> Data
        {
            get { return data; }
        }
    }
}