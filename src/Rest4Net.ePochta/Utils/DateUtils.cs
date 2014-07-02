using System;
using System.Globalization;

namespace Rest4Net.ePochta.Utils
{
    internal class DateUtils
    {
        private const string EmptyDate = "0000-00-00 00:00:00";
        private const string Format = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";

        public static string ToPochtaString(DateTime time)
        {
            return time.ToString(Format);
        }

        public static string ToPochtaString(DateTime? time, string customEmptyValue = null)
        {
            if (time == null)
                return customEmptyValue ?? EmptyDate;
            return ToPochtaString(time.Value);
        }

        public static DateTime ToPochtaDate(string date)
        {
            return DateTime.ParseExact(date, Format, CultureInfo.InvariantCulture);
        }

        public static DateTime? ToPochtaDateNullable(string date)
        {
            if (String.IsNullOrEmpty(date) || String.CompareOrdinal(EmptyDate, date) == 0)
                return null;
            return ToPochtaDate(date);
        }
    }
}
