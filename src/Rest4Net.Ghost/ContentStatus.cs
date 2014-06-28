using System;

namespace Rest4Net.Ghost
{
    public enum ContentStatus
    {
        All = 0,
        Published = 1,
        Draft = 2
    }

    internal static class ContentStatusHelper
    {
        public static ContentStatus ToPageStatus(this string value)
        {
            try
            {
                var v =  System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
                return (ContentStatus) Enum.Parse(typeof (ContentStatus), v);
            }
            catch (Exception)
            {
                return ContentStatus.Draft;
            }
        }

        public static string ToGhostString(this ContentStatus status)
        {
            return status.ToString().ToLower();
        }
    }
}
