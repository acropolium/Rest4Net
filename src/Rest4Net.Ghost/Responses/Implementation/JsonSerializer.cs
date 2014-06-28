using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace Rest4Net.Ghost.Responses.Implementation
{
    internal class JsonSerializer
    {
        public static JObject ConvertToJson(object ob)
        {
            var fields = ob.GetType()
                    .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(x => x.Name.StartsWith("_") && !x.GetCustomAttributes(typeof(IgnoreAttribute)).Any());
            var obj = new JObject();
            foreach (var field in fields)
            {
                var ignoreIfNull = field.GetCustomAttributes(typeof(IgnoreIfNullAttribute)).Any();
                var val = field.GetValue(ob);
                if (ignoreIfNull && (val == null))
                    continue;
                JToken t = null;
                if (field.FieldType == typeof (int))
                {
                    var vint = (int) val;
                    if (ignoreIfNull && vint == 0)
                        t = new JValue((object)null);
                    else
                        t = new JValue((int) val);
                }
                else if (field.FieldType == typeof (string))
                    t = new JValue((string) val);
                else if (field.FieldType == typeof (int?))
                    t = new JValue(((int?) val).Value);
                else if (field.FieldType == typeof (DateTime))
                    t = new JValue(((DateTime) val).ToString("yyyy-MM-ddTHH:mm:ssZ"));
                else if (field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof (List<>))
                {
                    if (typeof (IJsonifable).IsAssignableFrom(field.FieldType.GenericTypeArguments[0]))
                    {
                        var arr = new JArray();
                        foreach (IJsonifable o in (IEnumerable) val)
                            arr.Add(o.ToJson());
                        t = arr;
                    }
                }
                if (t == null) continue;
                obj.Add(field.Name.Substring(1), t);
            }
            return obj;
        }
    }
}
