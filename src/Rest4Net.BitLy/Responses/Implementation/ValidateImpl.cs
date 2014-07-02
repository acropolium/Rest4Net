namespace Rest4Net.BitLy.Responses.Implementation
{
    internal class ValidateImpl : IValidate
    {
#pragma warning disable 649
        private bool _valid;
#pragma warning restore 649

        public bool Valid
        {
            get { return _valid; }
        }
    }
}
