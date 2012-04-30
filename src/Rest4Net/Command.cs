using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using Rest4Net.CommandUtils.BodyProviders;

namespace Rest4Net
{
    public class Command
    {
        private readonly RequestType _type;
        private readonly string _path;
        private readonly RestServiceAccessProvider _executor;
        private readonly IList<KeyValuePair<string, string>> _options = new List<KeyValuePair<string, string>>();
        private readonly IList<KeyValuePair<string, string>> _headers = new List<KeyValuePair<string, string>>();
        private readonly IList<KeyValuePair<string, string>> _parameters = new List<KeyValuePair<string, string>>();

        private Command(RequestType type, string path, RestServiceAccessProvider executor)
        {
            _type = type;
            _path = path;
            _executor = executor;
        }

        internal static Command Create(string path, RequestType type, RestServiceAccessProvider executor)
        {
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException("path");
            return new Command(type, path, executor);
        }

        private Command WithSomething(ICollection<KeyValuePair<string, string>> list, string key, string value)
        {
            if (String.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException("key");
            list.Add(new KeyValuePair<string, string>(key, value));
            return this;
        }

        public Command WithHeader(string key, string value)
        {
            return WithSomething(_headers, key, value);
        }

        public Command WithOption(string key, string value)
        {
            return WithSomething(_options, key, value);
        }

        public Command WithParameter(string key, string value)
        {
            return WithSomething(_parameters, key, value);
        }

        public Command WithParameter(object parameters)
        {
            return WithParameter(
                parameters.GetType().GetFields().Select(
                    pi => new KeyValuePair<string, string>(pi.Name, pi.GetValue(parameters).ToString())))
                .WithParameter(
                    parameters.GetType().GetProperties().Select(
                        pi => new KeyValuePair<string, string>(pi.Name, pi.GetValue(parameters, null).ToString())));
        }

        public Command WithParameter(IEnumerable<KeyValuePair<string, string>> parameters)
        {
            foreach (var keyValuePair in parameters)
                WithParameter(keyValuePair.Key, keyValuePair.Value);
            return this;
        }

        public Command WithBody(string body)
        {
            BodyProvider = new StringProvider(body);
            return this;
        }

        public IList<KeyValuePair<string, string>> Parameters
        {
            get { return _parameters; }
        }

        public IList<KeyValuePair<string, string>> Headers
        {
            get { return _headers; }
        }

        public IList<KeyValuePair<string, string>> Options
        {
            get { return _options; }
        }

        public string Path
        {
            get { return _path; }
        }

        public RequestType Type
        {
            get { return _type; }
        }

        public ICommandBodyProvider BodyProvider { get; private set; }

        public CommandResult Execute()
        {
            return _executor.Execute(this);
        }
    }
}
