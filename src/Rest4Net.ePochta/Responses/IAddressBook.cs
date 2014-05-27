using System;

namespace Rest4Net.ePochta.Responses
{
    public interface IAddressBook
    {
        int Id { get; }
        string Name { get; }
        string Description { get; }
        int PhonesCount { get; }
        int ExceptionsCount { get; }
        DateTime Created { get; }
    }
}
