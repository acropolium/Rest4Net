namespace Rest4Net.Ghost.Exceptions
{
    public class GhostPleaseSignInException : GhostException
    {
        public GhostPleaseSignInException()
            : base("Please sign in")
        {
        }
    }
}
