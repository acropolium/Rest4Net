using System;
using System.IO;

namespace Rest4Net
{
    public interface ICommandBodyProvider : IDisposable
    {
        void Provide(Stream outputStream);
    }
}
