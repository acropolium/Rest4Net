using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace Rest4Net.CommandUtils
{
    internal static class JsonValue2Object
    {
        public static T ConvertTo<T>(this JToken value)
        {
            return (T)Convert(typeof (T), value);
        }

        private static bool KeysCheck(string name, string jsonName)
        {
            return String.Compare(name, jsonName, StringComparison.OrdinalIgnoreCase) == 0;
        }

        private static bool KeysAreEqual(string name, string jsonName)
        {
            var n1 = name;
            var n2 = jsonName;
            if (KeysCheck(n1, n2)) return true;
            if (n1.StartsWith("_"))
                n1 = n1.Substring(1);
            if (KeysCheck(n1, n2)) return true;
            n2 = n2.Replace("_", "");
            
            return KeysCheck(n1, n2);
        }

        private static object Convert(Type resultType, JToken value)
        {
            if (value == null)
                return null;
            var vObject = value as JObject;
            if (vObject != null)
            {
                var o = Activator.CreateInstance(resultType);
                foreach (var field in resultType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    var fn = field.Name;
                    foreach (var v in vObject.Properties().Where(v => KeysAreEqual(fn, v.Name)))
                    {
                        field.SetValue(o, Convert(field.FieldType, v.Value));
                        break;
                    }
                }
                return o;
            }
            var vValue = value as JValue;
            if (vValue != null)
            {
                try
                {
                    if (resultType == typeof (Guid))
                    {
                        var s = (string)System.Convert.ChangeType(vValue.Value, typeof(string));
                        return new Guid(s);
                    }
                }
                // ReSharper disable once EmptyGeneralCatchClause
                catch { }
                try
                {
                    return System.Convert.ChangeType(vValue.Value, resultType);
                }
                // ReSharper disable once EmptyGeneralCatchClause
                catch { }
                if (resultType == typeof(bool) && value.ToString() == "1")
                    return true;
            }
            var vArray = value as JArray;
            if (vArray != null)
            {
                if (resultType.IsGenericType && typeof(List<>) == resultType.GetGenericTypeDefinition())
                {
                    var type = resultType.GetGenericArguments()[0];
                    var o = Activator.CreateInstance(resultType);
                    var mi = resultType.GetMethod("Add");
                    foreach (var item in vArray)
                    {
                        mi.Invoke(o,
                                  new[]
                                      {
                                          Convert(type, item)
                                      });
                    }
                    return o;
                }
            }
            return null;
        }
    }
}
