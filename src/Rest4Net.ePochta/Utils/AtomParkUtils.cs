using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using Rest4Net.ePochta.Responses;

namespace Rest4Net.ePochta.Utils
{
    internal static class AtomParkUtils
    {
        public static JsonValue ConvertArrayedResult(this JsonValue input, Func<JsonValue, JsonValue> errorChecker)
        {
            var oInitial = errorChecker(input);
            if (oInitial == null || !oInitial.ContainsKey("result"))
                return oInitial;
            var o = (JsonObject)oInitial["result"];
            var keys = o.Keys.ToArray();

            var result = new JsonArray();
            var cnt = o[keys.First()].Count;
            for (var i = 0; i < cnt; i++)
            {
                var obj = new JsonObject();
                foreach (var key in keys)
                    obj.Add(key, o[key][i]);
                result.Add(obj);
            }
            oInitial.SetValue("result", result);
            return oInitial;
        }

        public static string PhonesToJson(Phone phone, Phone[] phones)
        {
            var items = new List<Phone> { phone };
            items.AddRange(phones ?? new Phone[0]);
            var array = new JsonArray();
            foreach (var item in items)
            {
                var itm = new JsonArray { new JsonPrimitive(item.PhoneNumber) };
                if (!String.IsNullOrWhiteSpace(item.Variables))
                    itm.Add(new JsonPrimitive(item.Variables));
                array.Add(itm);
            }
            return array.ToString();
        }
    }
}
