using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Rest4Net.CommandUtils.BodyProviders;

namespace Rest4Net
{
    public class Command
    {
        private readonly RequestType _type;
        private readonly string _path;
        private readonly RestApiProvider _executor;
        private readonly IList<KeyValuePair<string, string>> _options = new List<KeyValuePair<string, string>>();
        private readonly IList<KeyValuePair<string, string>> _headers = new List<KeyValuePair<string, string>>();
        private readonly IList<KeyValuePair<string, string>> _parameters = new List<KeyValuePair<string, string>>();

        private Command(RequestType type, string path, RestApiProvider executor)
        {
            _type = type;
            _path = path;
            _executor = executor;
        }

        internal static Command Create(string path, RequestType type, RestApiProvider executor)
        {
            if (String.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");
            return new Command(type, path, executor);
        }

        private Command WithSomething(ICollection<KeyValuePair<string, string>> list, string key, string value)
        {
            if (String.IsNullOrEmpty(key))
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

        public Command WithContitionParameter(bool condition, string key, string value)
        {
            return condition ? WithParameter(key, value) : this;
        }

        public Command WithParameterIfNotNull(string key, string value)
        {
            return WithContitionParameter(!String.IsNullOrEmpty(value), key, value);
        }

        public Command WithParameter(string key, int value)
        {
            return WithParameter(key, value.ToString(CultureInfo.InvariantCulture));
        }

        public Command WithParameterIfGreaterThanZero(string key, int value)
        {
            return value <= 0 ? this : WithParameter(key, value);
        }

        public Command WithParameter(string key, uint value)
        {
            return WithParameter(key, value.ToString(CultureInfo.InvariantCulture));
        }

#if PORTABLE
        private static IEnumerable<FieldInfo> GetFields(Type type)
        {
            return type.GetTypeInfo().DeclaredFields;
        }

        private static IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            return type.GetTypeInfo().DeclaredProperties;
        }
#else
        private static IEnumerable<FieldInfo> GetFields(Type type)
        {
            return type.GetFields();
        }

        private static IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            return type.GetProperties();
        }
#endif

        private static IEnumerable<KeyValuePair<string, string>> GetParametersFromObject(object parameters)
        {
            foreach (var f in GetFields(parameters.GetType()))
                yield return new KeyValuePair<string, string>(f.Name, f.GetValue(parameters).ToString());
            foreach (var f in GetProperties(parameters.GetType()))
                yield return new KeyValuePair<string, string>(f.Name, f.GetValue(parameters, null).ToString());
        }

        public Command WithParameter(object parameters)
        {
            return WithParameter(GetParametersFromObject(parameters));
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
