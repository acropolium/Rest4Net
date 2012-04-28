using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Rest4Net.IronMq.Responses.Implementation;

namespace Rest4Net.IronMq
{
    public class Client : Connector
    {
        private readonly string _token;
        private readonly string _projectId;

        public Client(string token, string projectId, Provider provider = Provider.AWS)
            : base(GetProviderHost(provider) + ".iron.io", true)
        {
            _token = token;
            _projectId = projectId;
        }

        protected override IRequest Build
        {
            get
            {
                var rq = base.Build
                    .AddHeader("Authorization", String.Format("OAuth {0}", _token))
                    .AlwaysUseContentType("application/json");
                rq.OnRequest += rq_OnRequest;
                rq.OnResponse += rq_OnResponse;
                return rq;
            }
        }

        private static void rq_OnRequest(HttpWebRequest request)
        {
            request.ContentType = "application/json";
        }

        private static void rq_OnResponse(IRequest request, HttpWebResponse webresponse, ref IResponse response)
        {
            if (response.Content == null || response.Content.Length == 0 || response.Content[0] != 91) return;
            var json = "{\"data\":" + Encoding.UTF8.GetString(response.Content) + "}";
            response.Content = Encoding.UTF8.GetBytes(json);
            response.ContentLength = response.Content.Length;
        }

        protected IRequest BuildWithPath(string path)
        {
            return Build.SetPath(String.Format("/1/projects/{0}/queues{1}", _projectId, path));
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
            return BuildWithPath("").AddQueryParam("page", page.ToString()).Run<DataImpl<IQueue, QueueImpl>>().Data;
        }

        public IQueue Queue(string name)
        {
            var q = BuildWithPath("/" + name).Run<QueueImpl>();
            q.ProjectID = _projectId;
            return q;
        }

        private string RunInfo(string path, RequestType type = RequestType.Post)
        {
            return BuildWithPath(path).SetMethod(type).Run<InfoImpl>().Message;
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
                BuildWithPath("/" + queueName + "/messages").SetMethod(RequestType.Post).Run<InfoMsgImpl>(
                    MessagesToJson(messages));
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
            return BuildWithPath("/" + queueName + "/messages").AddQueryParam("n", countToTake.ToString()).Run<MessagesImpl>().messages;
        }

        public bool RemoveMessage(string queueName, string id)
        {
            return RunInfo("/" + queueName + "/messages/" + id, RequestType.Delete) == "Deleted";
        }
    }
}
