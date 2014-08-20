using System;
using System.Collections.Generic;
using System.Reflection;

namespace Rest4Net.GoogleCustomSearch
{
    public class SearchParameters
    {
        public string Cr = null;
        public string DateRestrict = null;
        public uint C2coff = 0;
        public string ExactTerms = null;
        public string ExcludeTerms = null;
        public string LinkSite = null;
        public string FileType = null;
        public uint Filter = 1;
        public string Gl = null;
        public string Googlehost = null;
        public string HighRange = null;
        public string Hl = null;
        public string Hq = null;
        public string ImgColorType = null;
        public string ImgDominantColor = null;
        public string ImgSize = null;
        public string ImgType = null;
        public string LowRange = null;
        public string Lr = null;
        public uint Num = 10;
        public string OrTerms = null;
        public string RelatedSite = null;
        public string Rights = null;
        public string Safe = null;
        public string SearchType = null;
        public string SiteSearch = null;
        public string SiteSearchFilter = null;
        public int Start = 1;

#if !PORTABLE
        private static IEnumerable<FieldInfo> GetFields(Type type)
        {
            return type.GetFields(BindingFlags.Public | BindingFlags.Instance);
        }
#else
        private static IEnumerable<FieldInfo> GetFields(Type type)
        {
            return type.GetTypeInfo().DeclaredFields;
        }
#endif

        internal Command ProcessCommand(Command cmd)
        {
            foreach (var field in GetFields(GetType()))
            {
                var v = field.GetValue(this);
                if (v == null)
                    continue;
                cmd.WithParameter(Char.ToLowerInvariant(field.Name[0]) + field.Name.Substring(1), v.ToString());
            }
            return cmd;
        }
    }
}
