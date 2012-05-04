namespace Rest4Net.BitLy.Responses.Implementation
{
    internal class BitlyResponseImpl<TBitlyResponseInterface, TBitlyResponseClass> : IBitlyResponse<TBitlyResponseInterface>
        where TBitlyResponseClass : TBitlyResponseInterface
    {
        private int _statusCode;
        private string _statusTxt;
        private TBitlyResponseClass _data;

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
