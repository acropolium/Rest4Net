using System;
using System.Globalization;

namespace Rest4Net.ePochta.Responses
{
    internal static class DateUtils
    {
        private const string EmptyDate = "0000-00-00 00:00:00";
        private const string Format = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";

        public static string ToPochtaString(this DateTime time)
        {
            return time.ToString(Format);
        }

        public static string ToPochtaString(this DateTime? time, string customEmptyValue = null)
        {
            if (time == null)
                return customEmptyValue ?? EmptyDate;
            return time.Value.ToPochtaString();
        }

        public static DateTime ToPochtaDate(this string date)
        {
            return DateTime.ParseExact(date, Format, CultureInfo.InvariantCulture);
        }

        public static DateTime? ToPochtaDateNullable(this string date)
        {
            if (String.IsNullOrWhiteSpace(date) || String.CompareOrdinal(EmptyDate, date) == 0)
                return null;
            return date.ToPochtaDate();
        }
    }
}
