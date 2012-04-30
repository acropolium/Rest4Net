using System;
using System.IO;

namespace Rest4Net
{
    public interface ICommandResponseReader<out T> : IDisposable
    {
        T Read(Stream respomseStream);
    }
}
