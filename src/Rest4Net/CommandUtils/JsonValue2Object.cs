using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace Rest4Net.CommandUtils
{
    internal class JsonValue2Object
    {
        public static T ConvertTo<T>(JToken value)
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

#if PORTABLE
        private static IEnumerable<FieldInfo> GetFields(Type resultType)
        {
            return resultType.GetTypeInfo().DeclaredFields;
        }
#else
        private static IEnumerable<FieldInfo> GetFields(Type resultType)
        {
            return resultType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        }
#endif

        private static object Convert(Type resultType, JToken value)
        {
            if (value == null)
                return null;
            var vObject = value as JObject;
            if (vObject != null)
            {
                var o = Activator.CreateInstance(resultType);
                if (o as ContainJson != null)
                    (o as ContainJson).Json = value;
                foreach (var field in GetFields(resultType))
                {
                    var fn = field.Name;
                    foreach (var v in vObject.Properties())
                    {
                        if (!KeysAreEqual(fn, v.Name))
                            continue;
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
                        var s = System.Convert.ChangeType(vValue.Value, typeof(string)) as string;
                        return s==null ? Guid.Empty : new Guid(s);
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
                if (IsGenericList(resultType))
                {
#if PORTABLE
                    var t = resultType.GetTypeInfo();
                    var type = t.GenericTypeArguments[0];
                    var mi = t.GetDeclaredMethod("Add");
#else
                    var type = resultType.GetGenericArguments()[0];
                    var mi = resultType.GetMethod("Add");
#endif
                    var o = Activator.CreateInstance(resultType);
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

#if PORTABLE
        private static bool IsGenericList(Type resultType)
        {
            return resultType.IsConstructedGenericType && typeof(List<>) == resultType.GetGenericTypeDefinition();
        }
#else
        private static bool IsGenericList(Type resultType)
        {
            return resultType.IsGenericType && typeof(List<>) == resultType.GetGenericTypeDefinition();
        }
#endif
    }
}
