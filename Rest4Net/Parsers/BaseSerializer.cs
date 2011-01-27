using System;
using System.Collections.Generic;
using System.Reflection;

namespace Rest4Net.Parsers
{
    internal abstract class BaseSerializer<TRootType> : IRestSerializer where TRootType : class
    {
        protected BaseSerializer(RestApiSerializableAttribute attribute)
        {
            Attribute = attribute;
        }

        protected RestApiSerializableAttribute Attribute
        {
            get; private set;
        }

        public TObject Deserialize<TObject>(TObject obj, byte[] content)
        {
            var rootObject = ParseContent(content);
            return (TObject)FillType(obj, rootObject);
        }

        protected abstract TRootType ParseContent(byte[] content);

        protected string GetRealName(string name)
        {
            return Attribute.IgnoreUnderlineScores ? name.Replace("_", "") : name;
        }

        protected abstract TRootType FindSubLeaf(TRootType parent, string key);
        protected abstract bool LeafHasChildren(TRootType leaf);
        protected abstract string GetValue(TRootType leaf);
        protected abstract IEnumerable<TRootType> GetArray(TRootType leaf);

        private static object ConvertType(Type newType, string value)
        {
            try
            {
                return Convert.ChangeType(value, newType);
            }
            catch (FormatException)
            {
                if (newType == typeof(bool))
                {
                    bool val;
                    if (Boolean.TryParse(value, out val))
                        return val;
                    return !String.IsNullOrEmpty(value) && value == "1";
                }
            }
            return null;
        }

        private object FillType(object obj, TRootType leaf)
        {
            var typeOfObject = obj.GetType();
            var fields = typeOfObject.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
                var name = GetRealName(field.Name);
                var subLeaf = FindSubLeaf(leaf, name);
                if (subLeaf == null)
                    continue;
                if (field.FieldType.IsGenericType && typeof(IList<>).Equals(field.FieldType.GetGenericTypeDefinition()))
                {
                    var array = GetArray(subLeaf);
                    var listPointer = field.GetValue(obj);
                    var mi = listPointer.GetType().GetMethod("Add");
                    foreach (var item in array)
                    {
                        var type = field.FieldType.GetGenericArguments()[0];
                        object data;
                        if (type == typeof(string) || type == typeof(bool) || type == typeof(int) || type == typeof(float))
                            data = ConvertType(type, GetValue(item));
                        else
                            data = FillType(Activator.CreateInstance(type), item);
                        mi.Invoke(listPointer,
                                  new[]
                                      {
                                          data
                                      });
                    }
                }
                else
                {
                    field.SetValue(obj,
                                   LeafHasChildren(subLeaf)
                                       ? FillType(Activator.CreateInstance(field.FieldType), subLeaf)
                                       : ConvertType(field.FieldType, GetValue(subLeaf)));
                }
            }
            return obj;
        }
    }
}
