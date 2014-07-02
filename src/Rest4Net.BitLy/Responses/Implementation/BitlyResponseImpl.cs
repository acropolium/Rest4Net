namespace Rest4Net.BitLy.Responses.Implementation
{
    internal class BitlyResponseImpl<TBitlyResponseInterface, TBitlyResponseClass> : IBitlyResponse<TBitlyResponseInterface>
        where TBitlyResponseClass : TBitlyResponseInterface
    {
#pragma warning disable 649
        private int _statusCode;
        private string _statusTxt;
        private TBitlyResponseClass _data;
#pragma warning restore 649

        public int StatusCode
        {
            get { return _statusCode; }
        }

        public string StatusTxt
        {
            get { return _statusTxt; }
        }

        public TBitlyResponseInterface Data
        {
            get { return _data; }
        }
    }
}
