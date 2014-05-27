using System;

namespace Rest4Net.ePochta.Responses
{
    public class MessageInfo
    {
        /// <summary>
        /// Initialize message
        /// </summary>
        /// <param name="text">Message text</param>
        /// <param name="sender">Sender title</param>
        public MessageInfo(string text, string sender)
        {
            Sender = sender;
            Text = text;
            Lifetime = SmsLifetime.Maximum;
            Type = MessageType.Default;
        }

        /// <summary>
        /// Sender title
        /// </summary>
        public string Sender { get; private set; }

        /// <summary>
        /// Message text
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Alternative sender title
        /// //TODO: detail
        /// </summary>
        public string AlternativeSender { get; set; }

        /// <summary>
        /// Delay sms send-out to this time
        /// </summary>
        public DateTime? SendAt { get; set; }

        /// <summary>
        /// Sms lifetime. Maximum 24 hours to receive
        /// </summary>
        public SmsLifetime Lifetime { get; set; }

        /// <summary>
        /// Message type. Custom values are used for Russian networks
        /// </summary>
        public MessageType Type { get; set; }

        /// <summary>
        /// Custom application/module name for detailed reports
        /// </summary>
        public string UserApp { get; set; }

        internal Command FillCommand(Command cmd)
        {
            var c =
                cmd.WithParameter("sender", Sender)
                    .WithParameter("text", Text)
                    .WithParameter("sms_lifetime", (int) Lifetime)
                    .WithParameter("datetime", SendAt.ToPochtaString(""));
            if (Type != MessageType.Default)
                c = c.WithParameter("type", (int)Type);
            if (!String.IsNullOrWhiteSpace(AlternativeSender))
                c = c.WithParameter("asender", AlternativeSender);
            if (!String.IsNullOrWhiteSpace(UserApp))
                c = c.WithParameter("userapp", UserApp);
            return c;
        }
    }
}
