using System.Collections.Generic;

namespace Rest4Net.ePochta.Responses
{
    public enum Country
    {
        Moldova,
        Ukraine
    }

    internal class CountriesUtils
    {
        private static readonly Dictionary<Country, string> Items = new Dictionary<Country, string>
        {
            {Country.Moldova, "MD"},
            {Country.Ukraine, "UA"}
        };

        private static readonly Dictionary<string, Country> ItemsReverse = new Dictionary<string, Country>();

        static CountriesUtils()
        {
            foreach (var item in Items)
                ItemsReverse[item.Value] = item.Key;
        }

        public static string AsString(Country country)
        {
            return Items[country];
        }

        public static Country AsCountry(string country)
        {
            return ItemsReverse[country];
        }
    }
}
