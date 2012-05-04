using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Rest4Net.IronMq.Tests
{
    [TestFixture]
    public class GeneralTest
    {
        #region Configuration values
        private const string Token = "";
        private const string ProjectId = "";
        #endregion

        private IronMqProvider _client;
        private string _queueName;
        private HashSet<string> _names;
        private const int NewMessages = 5;
        private const int MessageSize = 10;

        [SetUp]
        public void Init()
        {
            _client = new IronMqProvider(Token, ProjectId);
            _queueName = MessageSize.RandomString();

            // Generate messages
            _names = new HashSet<string>();
            for (var i = 0; i < NewMessages; i++)
            {
                _names.Add(MessageSize.RandomString());
            }
        }

        [TearDown]
        public void Dispose()
        {
            _client.Dispose();
        }

        [Test(Description = "General verification of all functions")]
        public void GeneralVerification()
        {
            // Adding messages
            var msgs = _client.AddMessages(_queueName, _names.Select(n => n.ToMessage()).ToArray()).ToArray();
            Assert.AreEqual(msgs.Length, _names.Count);
            foreach (var message in msgs)
            {
                Assert.AreEqual(true, message.HasId());
                Assert.True(_names.Contains(message.Body));
            }

            var queue = FindQueue(_queueName);
            Assert.IsNotNull(queue);
            var m1 = _client.GetMessage(queue.Name);
            Assert.True(_names.Contains(m1.Body));
            var mq = _client.GetMessages(queue.Name, queue.Size).ToArray();
            Assert.AreEqual(mq.Length, queue.Size-1);
            Assert.True(_client.RemoveMessage(queue.Name, m1.ID));
            foreach (var m in mq)
            {
                Assert.True(_client.RemoveMessage(queue.Name, m.ID));
            }
            Assert.AreEqual(_client.Queue(_queueName).Size, 0);
        }

        private IQueue FindQueue(string queueName)
        {
            var q = _client.Queue(queueName);
            if (q == null)
                return null;
            foreach (var queue in _client.Queues().Where(queue => queue.Name == queueName))
            {
                Assert.AreEqual(queue.ID, q.ID);
                return queue;
            }
            return null;
        }
    }
}
