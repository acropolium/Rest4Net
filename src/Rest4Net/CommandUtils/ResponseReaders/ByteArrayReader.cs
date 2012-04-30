using System.IO;

namespace Rest4Net.CommandUtils.ResponseReaders
{
    public sealed class ByteArrayReader : ICommandResponseReader<byte[]>
    {
        public byte[] Read(Stream responseStream)
        {
            var buffer = new byte[16 * 1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public void Dispose() { }
    }
}
