using System;

namespace Rest4Net.ePochta.Responses
{
    public interface IPhoneException
    {
        int Id { get; }
        string Phone { get; }
        DateTime DateOfCreation { get; }
        string Comment { get; }
    }
}
