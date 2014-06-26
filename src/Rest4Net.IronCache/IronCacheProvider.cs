using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json.Linq;
using Rest4Net.Exceptions;
using Rest4Net.IronCache.Responses.Implementation;
using Rest4Net.Protocols;

namespace Rest4Net.IronCache
{
    public class IronCacheProvider : RestApiProvider
    {
        private readonly string _token;
        private readonly string _projectId;

        public IronCacheProvider(string token, string projectId, Provider provider = Provider.AWS)
            : base(new Https(GetProviderHost(provider) + ".iron.io"))
        {
            _token = token;
            _projectId = projectId;
        }

        protected override Command Cmd(string path, RequestType requestType = RequestType.Get)
        {
            return base.Cmd(path, requestType)
                .WithHeader("Authorization", String.Format("OAuth {0}", _token));
        }

        private static JToken JsonPreparer(JToken input)
        {
            return input as JArray == null ? input : new JObject { { "data", input as JArray } };
        }

        protected Command BuildWithPath(string path, RequestType type = RequestType.Get)
        {
            return Cmd(String.Format("/1/projects/{0}/caches{1}", _projectId, path), type);
        }

        private static string GetProviderHost(Provider provider)
        {
            switch (provider)
            {
                default:
                    return "cache-aws-us-east-1";
            }
        }

        public IEnumerable<ICache> Caches(uint page = 0)
        {
            return
                BuildWithPath("").WithParameter("page", page.ToString(CultureInfo.InvariantCulture)).Execute().To
                    <DataImpl<ICache, CacheImpl>>(JsonPreparer).Data;
        }

        public bool Put(string cacheName, string key, string value, int expiresIn = 604800, bool replace = false, bool add = false)
        {
            var json = new JObject {{"body", value}};
            if (expiresIn != 604800)
                json.Add("expires_in", expiresIn);
            if (replace)
                json.Add("replace", true);
            if (add)
                json.Add("add", true);
            return RunInfo("/" + cacheName + "/items/" + key, RequestType.Put, json.ToString()).Message == "Stored.";
        }

        public bool Put(string cacheName, string key, int value, int expiresIn = 604800, bool replace = false, bool add = false)
        {
            var json = new JObject { { "body", value } };
            if (expiresIn != 604800)
                json.Add("expires_in", expiresIn);
            if (replace)
                json.Add("replace", true);
            if (add)
                json.Add("add", true);
            return RunInfo("/" + cacheName + "/items/" + key, RequestType.Put, json.ToString()).Message == "Stored.";
        }

        public int Increment(string cacheName, string key, int amount)
        {
            var json = new JObject {{"amount", amount}};
            var result = RunInfo("/" + cacheName + "/items/" + key + "/increment", RequestType.Post, json.ToString());
            int r;
            if (result.Message != "Added" || result.Value == null || !Int32.TryParse(result.Value, out r))
                throw new Rest4NetException(result.Message, null);
            return r;
        }

        public string Get(string cacheName, string key)
        {
            return RunInfo("/" + cacheName + "/items/" + key, RequestType.Get).Value;
        }

        private InfoImpl RunInfo(string path, RequestType type = RequestType.Post, string body = null)
        {
            var c = BuildWithPath(path, type);
            if (body != null)
                c = c.WithBody(body);
            return c.Execute().To<InfoImpl>(JsonPreparer);
        }

        public bool Delete(string cacheName, string key)
        {
            return RunInfo("/" + cacheName + "/items/" + key, RequestType.Delete).Message == "Deleted.";
        }
    }
}
