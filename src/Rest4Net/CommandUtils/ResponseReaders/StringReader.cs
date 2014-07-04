using System.IO;
using System.Text;

namespace Rest4Net.CommandUtils.ResponseReaders
{
    public sealed class StringReader : ICommandResponseReader<string>
    {
        public string Read(Stream responseStream)
        {
            using (var br = new ByteArrayReader())
            {
                var buffer = br.Read(responseStream);
                return Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            }
        }

        public void Dispose() { }
    }
}
