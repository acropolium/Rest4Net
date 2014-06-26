using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Rest4Net.IronMq.Responses.Implementation;
using Rest4Net.Protocols;

namespace Rest4Net.IronMq
{
    public class IronMqProvider : RestApiProvider
    {
        private readonly string _token;
        private readonly string _projectId;

        public IronMqProvider(string token, string projectId, Provider provider = Provider.AWS)
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
            return Cmd(String.Format("/1/projects/{0}/queues{1}", _projectId, path), type);
        }

        private static string GetProviderHost(Provider provider)
        {
            switch (provider)
            {
                case Provider.Rackspace:
                    return "mq-rackspace-dfw";
                default:
                    return "mq-aws-us-east-1";
            }
        }

        public IEnumerable<IQueue> Queues(uint page = 0)
        {
            return
                BuildWithPath("").WithParameter("page", page.ToString(CultureInfo.InvariantCulture)).Execute().To
                    <DataImpl<IQueue, QueueImpl>>(
                        JsonPreparer).Data;
        }

        public IQueue Queue(string name)
        {
            var q = BuildWithPath("/" + name).Execute().To<QueueImpl>(JsonPreparer);
            q.ProjectID = _projectId;
            return q;
        }

        private string RunInfo(string path, RequestType type = RequestType.Post)
        {
            return BuildWithPath(path, type).Execute().To<InfoImpl>(JsonPreparer).Message;
        }

        public bool QueueClear(string name)
        {
            return RunInfo("/" + name + "/clear") == "Cleared";
        }

        private static string MessagesToJson(IEnumerable<Message> messages)
        {
            var sb = new StringBuilder();
            sb.Append('{');
            sb.Append("\"messages\":[");
            var f = true;
            foreach (var message in messages)
            {
                if (f)
                    f = false;
                else
                    sb.Append(',');
                sb.Append(message.ToJson());
            }
            sb.Append(']').Append('}');
            return sb.ToString();
        }

        public IEnumerable<IMessage> AddMessage(string queueName, Message message, params Message[] messages)
        {
            var l = new List<Message> {message};
            if (messages != null)
                l.AddRange(messages);
            return AddMessages(queueName, l.ToArray());
        }

        public IEnumerable<IMessage> AddMessages(string queueName, Message[] messages)
        {
            if (messages == null)
                return null;
            var r =
                BuildWithPath("/" + queueName + "/messages", RequestType.Post).WithBody(MessagesToJson(messages)).
                    Execute().To<InfoMsgImpl>(JsonPreparer);
            var l = new List<IMessage>();
            var i = 0;
            foreach (var message in messages)
            {
                message.ID = r.ids[i];
                l.Add(message);
                i++;
            }
            return l;
        }

        public IMessage GetMessage(string queueName)
        {
            return GetMessages(queueName).FirstOrDefault();
        }

        public IEnumerable<IMessage> GetMessages(string queueName, int countToTake = 1)
        {
            return BuildWithPath("/" + queueName + "/messages").WithParameter("n", countToTake.ToString()).Execute().To<MessagesImpl>(JsonPreparer).messages;
        }

        public bool RemoveMessage(string queueName, string id)
        {
            return RunInfo("/" + queueName + "/messages/" + id, RequestType.Delete) == "Deleted";
        }
    }
}
