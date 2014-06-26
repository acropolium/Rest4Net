using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rest4Net.CommandUtils;

namespace Rest4Net
{
    public class CommandResult : IDisposable
    {
        private readonly Stream _dataStream;
        private readonly TextReader _textReader;
        private readonly JsonTextReader _jsonTextReader;
        private readonly IDictionary<string, string> _metas = new Dictionary<string, string>();

        public CommandResult(Stream dataStream)
        {
            _dataStream = dataStream;
            _textReader = new StreamReader(_dataStream);
            _jsonTextReader = new JsonTextReader(_textReader);
        }

        public IDictionary<string, string> Metadata
        {
            get { return _metas; }
        }

        public Stream ToStream()
        {
            return _dataStream;
        }

        public JToken ToJson()
        {
            return JToken.Load(_jsonTextReader);
        }

        public object ToObject()
        {
            dynamic d = ToJson();
            return d;
        }

        public override string ToString()
        {
            using (var r = new CommandUtils.ResponseReaders.StringReader())
                return r.Read(_dataStream);
        }

        public T To<T>(Func<JToken, JToken> prepareJson = null)
        {
            var json = ToJson();
            if (prepareJson != null)
                json = prepareJson(json);
            return json.ConvertTo<T>();
        }
        
        public void Dispose()
        {
            if (_jsonTextReader != null)
                _jsonTextReader.Close();
            if (_textReader != null)
                _textReader.Dispose();
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