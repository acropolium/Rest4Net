using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Reflection;

namespace Rest4Net.CommandUtils
{
    internal static class JsonValue2Object
    {
        public static T ConvertTo<T>(this JsonValue value)
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

        private static object Convert(Type resultType, JsonValue value)
        {
            if (value == null)
                return null;
            var vt = value.GetType();
            if (vt == typeof(JsonObject))
            {
                var o = Activator.CreateInstance(resultType);
                foreach (var field in resultType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    var fn = field.Name;
                    foreach (var v in value.Where(v => KeysAreEqual(fn, v.Key)))
                    {
                        field.SetValue(o, Convert(field.FieldType, v.Value));
                        break;
                    }
                }
                return o;
            }
            if (vt == typeof(JsonPrimitive))
            {
                object obj;
                if (value.TryReadAs(resultType, out obj))
                    return obj;
                if (resultType == typeof(bool) && value.ToString() == "1")
                    return true;
            }
            if (vt == typeof(JsonArray))
            {
                if (resultType.IsGenericType && typeof(List<>) == resultType.GetGenericTypeDefinition())
                {
                    var type = resultType.GetGenericArguments()[0];
                    var o = Activator.CreateInstance(resultType);
                    var mi = resultType.GetMethod("Add");
                    foreach (var item in value)
                    {
                        mi.Invoke(o,
                                  new[]
                                      {
                                          Convert(type, item.Value)
                                      });
                    }
                    return o;
                }
            }
            return null;
        }
    }
}
