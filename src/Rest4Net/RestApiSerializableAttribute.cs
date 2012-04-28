using System;

namespace Rest4Net
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class RestApiSerializableAttribute : Attribute
    {
        public bool IgnoreMemberCase
        {
            get; private set;
        }

        public bool IgnoreUnderlineScores
        {
            get; private set;
        }

        public RestApiSerializableAttribute(bool ignoreMemberCase = false, bool ignoreUnderlineScores = false)
        {
            IgnoreMemberCase = ignoreMemberCase;
            IgnoreUnderlineScores = ignoreUnderlineScores;
        }
    }
}
