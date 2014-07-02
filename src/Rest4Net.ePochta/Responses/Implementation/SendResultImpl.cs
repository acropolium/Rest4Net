using System;

namespace Rest4Net.ePochta.Responses.Implementation
{
    internal class SendResultImpl : SendPriceImpl, ISendResult
    {
#pragma warning disable 649
        private int id;
#pragma warning restore 649

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
