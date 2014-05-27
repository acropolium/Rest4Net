using System;
using System.Collections.Generic;

namespace Rest4Net.ePochta.Responses
{
    public interface IPhone
    {
        int Id { get; }
        int AddressbookId { get; }
        string Phone { get; }
        string NormalPhone { get; }
        HashSet<string> Variables { get; }
        int Status { get; }
        string Code { get; }
    }
}
