namespace Rest4Net.ePochta.Responses
{
    public sealed class Phone
    {
        public Phone(string phone, params string[] variables)
        {
            PhoneNumber = phone;
            Variables = GetVariables(variables);
        }

        internal readonly string PhoneNumber;
        internal readonly string Variables;

        internal static string GetVariables(string[] variables)
        {
            return string.Join(";", (variables ?? new string[0]));
        }
    }
}
