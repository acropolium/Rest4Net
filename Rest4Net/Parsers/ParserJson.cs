using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Rest4Net.Parsers
{
    internal class ParserJson : BaseSerializer<object>
    {
        public ParserJson(RestApiSerializableAttribute attribute)
            : base(attribute) { }

        protected override object ParseContent(byte[] content)
        {
            return (new JsonFormatParser()).JsonDecode(Encoding.UTF8.GetString(content)) as Hashtable;
        }

        protected override object FindSubLeaf(object parent, string key)
        {
            var ht = ((Hashtable) parent);
            var nkey = ht.Keys.OfType<string>().FirstOrDefault(
                l =>
                Attribute.IgnoreMemberCase
                    ? key.ToLower().Equals(GetRealName(l.ToLower()))
                    : key.Equals(GetRealName(l)));
            return nkey != null ? ht[nkey] : null;
        }

        protected override bool LeafHasChildren(object leaf)
        {
            return !(leaf is String);
        }

        protected override string GetValue(object leaf)
        {
            return leaf as String;
        }

        protected override IEnumerable<object> GetArray(object leaf)
        {
            return ((ArrayList)leaf).ToArray();
        }
/*
        private static void ParseElement(Type type, TreeLeaf leaf, Hashtable ht)
        {
            if (ht == null)
                return;
            foreach (string key in ht.Keys)
            {
                var l = new TreeLeaf { Children = new List<TreeLeaf>(), Name = key };
                leaf.Children.Add(l);
                var value = ht[key];
                if (!(value is Hashtable))
                {
                    if (value is ArrayList)
                    {
                        var i = 1;
                        foreach (var o in (ArrayList)value)
                        {
                            var li = new TreeLeaf { Children = new List<TreeLeaf>(), Name = String.Format("Item{0}", i++) };
                            l.Children.Add(li);
                            ParseElement(li, o as Hashtable);
                        }
                    }
                    else
                        l.Value = (value ?? String.Empty).ToString();
                }
                else
                    ParseElement(l, value as Hashtable);
            }
        }*/

        #region Json Parser
        /// <summary>
        /// This class encodes and decodes JSON strings.
        /// Spec. details, see http://www.json.org/
        /// 
        /// JSON uses Arrays and Objects. These correspond here to the datatypes ArrayList and Hashtable.
        /// All numbers are parsed to doubles.
        /// </summary>
        public class JsonFormatParser
        {
            private const int TokenNone = 0;
            private const int TokenCurlyOpen = 1;
            private const int TokenCurlyClose = 2;
            private const int TokenSquaredOpen = 3;
            private const int TokenSquaredClose = 4;
            private const int TokenColon = 5;
            private const int TokenComma = 6;
            private const int TokenString = 7;
            private const int TokenNumber = 8;
            private const int TokenTrue = 9;
            private const int TokenFalse = 10;
            private const int TokenNull = 11;

            private const int BuilderCapacity = 2000;

            /// <summary>
            /// On decoding, this value holds the position at which the parse failed (-1 = no error).
            /// </summary>
            protected int LastErrorIndex = -1;
            protected string LastDecode = "";

            /// <summary>
            /// Parses the string json into a value
            /// </summary>
            /// <param name="json">A JSON string.</param>
            /// <returns>An ArrayList, a Hashtable, a double, a string, null, true, or false</returns>
            public object JsonDecode(string json)
            {
                // save the string for debug information
                LastDecode = json;

                if (json != null)
                {
                    char[] charArray = json.ToCharArray();
                    int index = 0;
                    bool success = true;
                    object value = ParseValue(charArray, ref index, ref success);
                    if (success)
                    {
                        LastErrorIndex = -1;
                    }
                    else
                    {
                        LastErrorIndex = index;
                    }
                    return value;
                }
                    return null;
            }

            /// <summary>
            /// On decoding, this function returns the position at which the parse failed (-1 = no error).
            /// </summary>
            /// <returns></returns>
            public bool LastDecodeSuccessful()
            {
                return (LastErrorIndex == -1);
            }

            /// <summary>
            /// On decoding, this function returns the position at which the parse failed (-1 = no error).
            /// </summary>
            /// <returns></returns>
            public int GetLastErrorIndex()
            {
                return LastErrorIndex;
            }

            /// <summary>
            /// If a decoding error occurred, this function returns a piece of the JSON string 
            /// at which the error took place. To ease debugging.
            /// </summary>
            /// <returns></returns>
            public string GetLastErrorSnippet()
            {
                if (LastErrorIndex == -1)
                    return String.Empty;
                var startIndex = LastErrorIndex - 5;
                var endIndex = LastErrorIndex + 15;
                if (startIndex < 0)
                    startIndex = 0;
                if (endIndex >= LastDecode.Length)
                    endIndex = LastDecode.Length - 1;
                return LastDecode.Substring(startIndex, endIndex - startIndex + 1);
            }

            protected Hashtable ParseObject(char[] json, ref int index)
            {
                var table = new Hashtable();

                // {
                NextToken(json, ref index);

                while (true)
                {
                    var token = LookAhead(json, index);
                    switch (token)
                    {
                        case TokenNone:
                            return null;
                        case TokenComma:
                            NextToken(json, ref index);
                            break;
                        case TokenCurlyClose:
                            NextToken(json, ref index);
                            return table;
                        default:
                            {
                                // name
                                var name = ParseString(json, ref index);
                                if (name == null)
                                {
                                    return null;
                                }

                                // :
                                token = NextToken(json, ref index);
                                if (token != TokenColon)
                                {
                                    return null;
                                }

                                // value
                                var success = true;
                                var value = ParseValue(json, ref index, ref success);
                                if (!success)
                                    return null;

                                table[name] = value;
                            }
                            break;
                    }
                }
            }

            protected ArrayList ParseArray(char[] json, ref int index)
            {
                var array = new ArrayList();

                // [
                NextToken(json, ref index);

                while (true)
                {
                    var token = LookAhead(json, index);
                    switch (token)
                    {
                        case TokenNone:
                            return null;
                        case TokenComma:
                            NextToken(json, ref index);
                            break;
                        case TokenSquaredClose:
                            NextToken(json, ref index);
                            break;
                        default:
                            {
                                var success = true;
                                var value = ParseValue(json, ref index, ref success);
                                if (!success)
                                    return array;
                                array.Add(value);
                            }
                            break;
                    }
                }
            }

            protected object ParseValue(char[] json, ref int index, ref bool success)
            {
                switch (LookAhead(json, index))
                {
                    case TokenString:
                        return ParseString(json, ref index);
                    case TokenNumber:
                        //return ParseString(json, ref index);
                        return ParseNumber(json, ref index);
                    case TokenCurlyOpen:
                        return ParseObject(json, ref index);
                    case TokenSquaredOpen:
                        return ParseArray(json, ref index);
                    case TokenTrue:
                        NextToken(json, ref index);
                        return "true";
                        //return Boolean.Parse("TRUE");
                    case TokenFalse:
                        NextToken(json, ref index);
                        return "false";
                        //return Boolean.Parse("FALSE");
                    case TokenNull:
                        NextToken(json, ref index);
                        return null;
                    case TokenNone:
                        break;
                }

                success = false;
                return null;
            }

            protected string ParseString(char[] json, ref int index)
            {
                var s = new StringBuilder(BuilderCapacity);

                EatWhitespace(json, ref index);

                // "
                var c = json[index++];

                var complete = false;
                while (!complete)
                {

                    if (index == json.Length)
                    {
                        break;
                    }

                    c = json[index++];
                    if (c == '"')
                    {
                        complete = true;
                        break;
                    }
                    if (c == '\\')
                    {

                        if (index == json.Length)
                        {
                            break;
                        }
                        c = json[index++];
                        switch (c)
                        {
                            case '"':
                                s.Append('"');
                                break;
                            case '\\':
                                s.Append('\\');
                                break;
                            case '/':
                                s.Append('/');
                                break;
                            case 'b':
                                s.Append('\b');
                                break;
                            case 'f':
                                s.Append('\f');
                                break;
                            case 'n':
                                s.Append('\n');
                                break;
                            case 'r':
                                s.Append('\r');
                                break;
                            case 't':
                                s.Append('\t');
                                break;
                            case 'u':
                                {
                                    var remainingLength = json.Length - index;
                                    if (remainingLength >= 4)
                                    {
                                        // fetch the next 4 chars
                                        var unicodeCharArray = new char[4];
                                        Array.Copy(json, index, unicodeCharArray, 0, 4);
                                        // parse the 32 bit hex into an integer codepoint
                                        var codePoint = UInt32.Parse(new string(unicodeCharArray), NumberStyles.HexNumber);
                                        // convert the integer codepoint to a unicode char and add to string
                                        s.Append(Char.ConvertFromUtf32((int)codePoint));
                                        // skip 4 chars
                                        index += 4;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                break;
                        }

                    }
                    else
                    {
                        s.Append(c);
                    }
                }

                return !complete ? null : s.ToString();
            }

            protected string ParseNumber(char[] json, ref int index)
            {
                EatWhitespace(json, ref index);

                var lastIndex = GetLastIndexOfNumber(json, index);
                var charLength = (lastIndex - index) + 1;
                var numberCharArray = new char[charLength];

                Array.Copy(json, index, numberCharArray, 0, charLength);
                index = lastIndex + 1;
                return new String(numberCharArray);
            }
/*
            protected double ParseNumber(char[] json, ref int index)
            {
                EatWhitespace(json, ref index);

                var lastIndex = GetLastIndexOfNumber(json, index);
                var charLength = (lastIndex - index) + 1;
                var numberCharArray = new char[charLength];

                Array.Copy(json, index, numberCharArray, 0, charLength);
                index = lastIndex + 1;
                return Double.Parse(new string(numberCharArray), CultureInfo.InvariantCulture);
            }
            */
            protected int GetLastIndexOfNumber(char[] json, int index)
            {
                int lastIndex;
                for (lastIndex = index; lastIndex < json.Length; lastIndex++)
                {
                    if ("0123456789+-.eE".IndexOf(json[lastIndex]) == -1)
                    {
                        break;
                    }
                }
                return lastIndex - 1;
            }

            protected void EatWhitespace(char[] json, ref int index)
            {
                for (; index < json.Length; index++)
                {
                    if (" \t\n\r".IndexOf(json[index]) == -1)
                    {
                        break;
                    }
                }
            }

            protected int LookAhead(char[] json, int index)
            {
                var saveIndex = index;
                return NextToken(json, ref saveIndex);
            }

            protected int NextToken(char[] json, ref int index)
            {
                EatWhitespace(json, ref index);

                if (index == json.Length)
                {
                    return TokenNone;
                }

                var c = json[index];
                index++;
                switch (c)
                {
                    case '{':
                        return TokenCurlyOpen;
                    case '}':
                        return TokenCurlyClose;
                    case '[':
                        return TokenSquaredOpen;
                    case ']':
                        return TokenSquaredClose;
                    case ',':
                        return TokenComma;
                    case '"':
                        return TokenString;
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                    case '-':
                        return TokenNumber;
                    case ':':
                        return TokenColon;
                }
                index--;

                var remainingLength = json.Length - index;

                // false
                if (remainingLength >= 5)
                {
                    if (json[index] == 'f' &&
                        json[index + 1] == 'a' &&
                        json[index + 2] == 'l' &&
                        json[index + 3] == 's' &&
                        json[index + 4] == 'e')
                    {
                        index += 5;
                        return TokenFalse;
                    }
                }

                // true
                if (remainingLength >= 4)
                {
                    if (json[index] == 't' &&
                        json[index + 1] == 'r' &&
                        json[index + 2] == 'u' &&
                        json[index + 3] == 'e')
                    {
                        index += 4;
                        return TokenTrue;
                    }
                }

                // null
                if (remainingLength >= 4)
                {
                    if (json[index] == 'n' &&
                        json[index + 1] == 'u' &&
                        json[index + 2] == 'l' &&
                        json[index + 3] == 'l')
                    {
                        index += 4;
                        return TokenNull;
                    }
                }

                return TokenNone;
            }

            protected bool SerializeObjectOrArray(object objectOrArray, StringBuilder builder)
            {
                if (objectOrArray is Hashtable)
                {
                    return SerializeObject((Hashtable)objectOrArray, builder);
                }
                if (objectOrArray is ArrayList)
                {
                    return SerializeArray((ArrayList)objectOrArray, builder);
                }
                return false;
            }

            protected bool SerializeObject(Hashtable anObject, StringBuilder builder)
            {
                builder.Append("{");

                var e = anObject.GetEnumerator();
                var first = true;
                while (e.MoveNext())
                {
                    string key = e.Key.ToString();
                    object value = e.Value;

                    if (!first)
                    {
                        builder.Append(", ");
                    }

                    SerializeString(key, builder);
                    builder.Append(":");
                    if (!SerializeValue(value, builder))
                    {
                        return false;
                    }

                    first = false;
                }

                builder.Append("}");
                return true;
            }

            protected bool SerializeArray(ArrayList anArray, StringBuilder builder)
            {
                builder.Append("[");
                var first = true;
                foreach (var value in anArray)
                {
                    if (!first)
                        builder.Append(", ");
                    if (!SerializeValue(value, builder))
                        return false;
                    first = false;
                }
                builder.Append("]");
                return true;
            }

            protected bool SerializeValue(object value, StringBuilder builder)
            {
                if (value is string)
                {
                    SerializeString((string)value, builder);
                }
                else if (value is Hashtable)
                {
                    SerializeObject((Hashtable)value, builder);
                }
                else if (value is ArrayList)
                {
                    SerializeArray((ArrayList)value, builder);
                }
                else if (IsNumeric(value))
                {
                    SerializeNumber(Convert.ToDouble(value), builder);
                }
                else if ((value is Boolean) && (Boolean)value)
                {
                    builder.Append("true");
                }
                else if ((value is Boolean) && ((Boolean)value == false))
                {
                    builder.Append("false");
                }
                else if (value == null)
                {
                    builder.Append("null");
                }
                else
                {
                    return false;
                }
                return true;
            }

            protected void SerializeString(string aString, StringBuilder builder)
            {
                builder.Append("\"");

                var charArray = aString.ToCharArray();
                foreach (char c in charArray)
                {
                    switch (c)
                    {
                        case '"':
                            builder.Append("\\\"");
                            break;
                        case '\\':
                            builder.Append("\\\\");
                            break;
                        case '\b':
                            builder.Append("\\b");
                            break;
                        case '\f':
                            builder.Append("\\f");
                            break;
                        case '\n':
                            builder.Append("\\n");
                            break;
                        case '\r':
                            builder.Append("\\r");
                            break;
                        case '\t':
                            builder.Append("\\t");
                            break;
                        default:
                            {
                                int codepoint = Convert.ToInt32(c);
                                if ((codepoint >= 32) && (codepoint <= 126))
                                {
                                    builder.Append(c);
                                }
                                else
                                {
                                    builder.Append("\\u" + Convert.ToString(codepoint, 16).PadLeft(4, '0'));
                                }
                            }
                            break;
                    }
                }

                builder.Append("\"");
            }

            protected void SerializeNumber(double number, StringBuilder builder)
            {
                builder.Append(Convert.ToString(number, CultureInfo.InvariantCulture));
            }

            /// <summary>
            /// Determines if a given object is numeric in any way
            /// (can be integer, double, etc). C# has no pretty way to do this.
            /// </summary>
            protected bool IsNumeric(object o)
            {
                try
                {
                    Double.Parse(o.ToString());
                }
                catch (Exception)
                {
                    return false;
                }
                return true;
            }
        }
        #endregion Json Parser
    }
}
