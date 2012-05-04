using System.IO;

namespace Rest4Net.CommandUtils.BodyProviders
{
    public sealed class StringProvider : ICommandBodyProvider
    {
        private readonly string _data;

        public StringProvider(string data)
        {
            _data = data;
        }

        public void Provide(Stream outputStream)
        {
            using (var tw = new StreamWriter(outputStream))
            {
                tw.Write(_data);
                tw.Flush();
            }
        }

        public void Dispose() { }
    }
}
