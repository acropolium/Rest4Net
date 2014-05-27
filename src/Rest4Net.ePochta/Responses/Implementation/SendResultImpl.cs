using System;

namespace Rest4Net.ePochta.Responses.Implementation
{
    internal class SendResultImpl : SendPriceImpl, ISendResult
    {
        private int id;

        public int Id
        {
            get { return id; }
        }

        public override string ToString()
        {
            return String.Format("{0}: {1}", Id, base.ToString());
        }
    }
}
