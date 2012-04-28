using System;

namespace Rest4Net.IronMq.Tests
{
    internal static class GeneralTestHelper
    {
        private static readonly Random Rng = new Random();
        private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        internal static string RandomString(this int size)
        {
            var buffer = new char[size];
            for (var i = 0; i < size; i++)
            {
                buffer[i] = Chars[Rng.Next(Chars.Length)];
            }
            return new string(buffer);
        }

        internal static bool HasId(this IMessage message)
        {
            return !String.IsNullOrWhiteSpace(message.ID);
        }

        internal static Message ToMessage(this string name)
        {
            return new Message(name);
        }
    }
}
