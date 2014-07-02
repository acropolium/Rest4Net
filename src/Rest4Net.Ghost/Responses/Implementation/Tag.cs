using System;
using Newtonsoft.Json.Linq;

namespace Rest4Net.Ghost.Responses.Implementation
{
    internal class Tag : ITag
    {
#pragma warning disable 649
        [IgnoreIfNull]
        private int _id;
        [Ignore]
        private Guid _uuid;
        [IgnoreIfNull]
        private string _slug;
        [IgnoreIfNull]
        private string _meta_title;
        [IgnoreIfNull]
        private string _meta_description;
        [Ignore]
        private DateTime _created_at;
        [Ignore]
        private int _created_by;
        [Ignore]
        private DateTime _updated_at;
        [Ignore]
        private int _updated_by;
        private string _name;
        [IgnoreIfNull]
        private string _description;
        [Ignore]
        private int? _parent_id;
#pragma warning restore 649

        public override string ToString()
        {
            return Name;
        }

        public JObject ToJson()
        {
            return JsonSerializer.ConvertToJson(this);
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public Guid Uuid
        {
            get { return _uuid; }
            set { _uuid = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Slug
        {
            get { return _slug; }
            set { _slug = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public int? ParentId
        {
            get { return _parent_id; }
            set { _parent_id = value; }
        }

        public string MetaTitle
        {
            get { return _meta_title; }
            set { _meta_title = value; }
        }

        public string MetaDescription
        {
            get { return _meta_description; }
            set { _meta_description = value; }
        }

        public DateTime CreatedAt
        {
            get { return _created_at; }
            set { }
        }

        public int CreatedBy
        {
            get { return _created_by; }
            set { _created_by = value; }
        }

        public DateTime UpdatedAt
        {
            get { return _updated_at; }
            set { }
        }

        public int UpdatedBy
        {
            get { return _updated_by; }
            set { _updated_by = value; }
        }
    }
}
