using System;

namespace Rest4Net.Ghost.Exceptions
{
    public class GhostException : Exception
    {
        public GhostException(string message) : base(message) { }
    }
}
