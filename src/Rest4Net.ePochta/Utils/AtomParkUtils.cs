using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Rest4Net.ePochta.Responses;

namespace Rest4Net.ePochta.Utils
{
    internal class AtomParkUtils
    {
        public static JToken ConvertArrayedResult(JToken input, CommandResult.JsonPreparer<JToken, JToken> errorChecker)
        {
            var oInitial = errorChecker(input);
            var objInitial = oInitial as JObject;
            if (objInitial == null || objInitial["result"] == null)
                return oInitial;
            var o = (JObject) objInitial.GetValue("result");
            var keys = new List<string>();
            foreach (var p in o.Properties())
                keys.Add(p.Name);

            var result = new JArray();
            var cnt = ((JArray)o[keys[0]]).Count;
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
                if (!String.IsNullOrEmpty(item.Variables))
                    itm.Add(new JValue(item.Variables));
                array.Add(itm);
            }
            return array.ToString();
        }
    }
}
