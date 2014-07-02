namespace Rest4Net.ePochta.Responses.Implementation
{
    internal class ResponseImpl<TInterface, TClass> where TClass : class, TInterface
    {
#pragma warning disable 649
        public readonly TClass result;
#pragma warning restore 649
    }
}