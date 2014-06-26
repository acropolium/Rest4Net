using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Rest4Net.ePochta.Responses;

namespace Rest4Net.ePochta.Utils
{
    internal static class AtomParkUtils
    {
        public static JToken ConvertArrayedResult(this JToken input, Func<JToken, JToken> errorChecker)
        {
            var oInitial = errorChecker(input);
            var objInitial = oInitial as JObject;
            if (objInitial == null || objInitial["result"] == null)
                return oInitial;
            var o = (JObject) objInitial.GetValue("result");
            var keys = o.Properties().Select(x => x.Name).ToArray();

            var result = new JArray();
            var cnt = ((JArray)o[keys.First()]).Count;
            for (var i = 0; i < cnt; i++)
            {
                var obj = new JObject();
                foreach (var key in keys)
                    obj.Add(key, ((JArray)o[key])[i]);
                result.Add(obj);
            }
            oInitial["result"] = result;
            return oInitial;
        }

        public static string PhonesToJson(Phone phone, Phone[] phones)
        {
            var items = new List<Phone> { phone };
            items.AddRange(phones ?? new Phone[0]);
            var array = new JArray();
            foreach (var item in items)
            {
                var itm = new JArray { new JValue(item.PhoneNumber) };
                if (!String.IsNullOrWhiteSpace(item.Variables))
                    itm.Add(new JValue(item.Variables));
                array.Add(itm);
            }
            return array.ToString();
        }
    }
}
