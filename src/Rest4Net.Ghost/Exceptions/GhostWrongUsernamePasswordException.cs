namespace Rest4Net.Ghost.Exceptions
{
    public class GhostWrongUsernamePasswordException : GhostException
    {
        public GhostWrongUsernamePasswordException() : base("Invalid credentials")
        {
        }
    }
}
