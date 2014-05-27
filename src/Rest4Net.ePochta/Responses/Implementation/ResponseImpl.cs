namespace Rest4Net.ePochta.Responses.Implementation
{
    internal class ResponseImpl<TInterface, TClass> where TClass : class, TInterface
    {
        public readonly TClass result;
    }
}