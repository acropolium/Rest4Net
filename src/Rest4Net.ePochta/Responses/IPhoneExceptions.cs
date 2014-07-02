using System.Collections.Generic;

namespace Rest4Net.ePochta.Responses
{
    public interface IPhoneExceptions
    {
        int Count { get; }
        IEnumerable<IPhoneException> Items { get; }
    }
}
