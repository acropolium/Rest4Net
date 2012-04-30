using System;
using System.Collections.Generic;
using System.IO;
using System.Json;

namespace Rest4Net
{
    public class CommandResult : IDisposable
    {
        private readonly Stream _dataStream;
        private readonly IDictionary<string, string> _metas = new Dictionary<string, string>();

        public CommandResult(Stream dataStream)
        {
            _dataStream = dataStream;
        }

        public IDictionary<string, string> Metadata
        {
            get { return _metas; }
        }

        public Stream ToStream()
        {
            return _dataStream;
        }

        public JsonValue ToJson()
        {
            return JsonValue.Load(_dataStream);
        }

        public override string ToString()
        {
            using (var r = new CommandUtils.ResponseReaders.StringReader())
                return r.Read(_dataStream);
        }
        
        public void Dispose()
        {
            if (_dataStream != null)
                _dataStream.Dispose();
        }

        public CommandResult WithMeta(string key, string value)
        {
            _metas[key] = value;
            return this;
        }
    }
}
