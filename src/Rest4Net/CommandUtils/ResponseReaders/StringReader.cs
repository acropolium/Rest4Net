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
                return Encoding.UTF8.GetString(br.Read(responseStream));
            }
        }

        public void Dispose() { }
    }
}
