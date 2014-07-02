using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Rest4Net.Ghost.Responses.Implementation
{
    internal class Post : IPost
    {
#pragma warning disable 649
        [Ignore]
        private int _id;
        [Ignore]
        private Guid _uuid;
        private string _title;
        [IgnoreIfNull]
        private string _slug;
        private string _markdown;
        [Ignore]
        private string _html;
        [IgnoreIfNull]
        private string _image;
        private bool _featured;
        private bool _page;
        [IgnoreIfNull]
        private string _status;
        [IgnoreIfNull]
        private string _language;
        [IgnoreIfNull]
        private string _meta_title;
        [IgnoreIfNull]
        private string _meta_description;
        [IgnoreIfNull]
        private int _author_id;
        private DateTime _created_at;
        [IgnoreIfNull]
        private int _created_by;
        private DateTime _updated_at;
        [IgnoreIfNull]
        private int _updated_by;
        private DateTime _published_at;
        [IgnoreIfNull]
        private int? _published_by;
        [Ignore]
        private User _author;
        [Ignore]
        private User _user;
        private List<Tag> _tags = new List<Tag>();
#pragma warning restore 649

        public override string ToString()
        {
            return Title;
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

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public string Slug
        {
            get { return _slug; }
            set { _slug = value; }
        }

        public string Markdown
        {
            get { return _markdown; }
            set { _markdown = value; }
        }

        public string Html
        {
            get { return _html; }
            set { _html = value; }
        }

        public string Image
        {
            get { return _image; }
            set { _image = value; }
        }

        public bool IsFeatured
        {
            get { return _featured; }
            set { _featured = value; }
        }

        public bool IsPage
        {
            get { return _page; }
            set { _page = value; }
        }

        public ContentStatus Status
        {
            get { return ContentStatusHelper.ToPageStatus(_status); }
            set { _status = ContentStatusHelper.ToGhostString(value); }
        }

        public string Language
        {
            get { return _language; }
            set { _language = value; }
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

        public int AuthorId
        {
            get { return _author_id; }
            set { _author_id = value; }
        }

        public DateTime CreatedAt
        {
            get { return _created_at; }
            set { _created_at = value; }
        }

        public int CreatedBy
        {
            get { return _created_by; }
            set { _created_by = value; }
        }

        public DateTime UpdatedAt
        {
            get { return _updated_at; }
            set { _updated_at = value; }
        }

        public int UpdatedBy
        {
            get { return _updated_by; }
            set { _updated_by = value; }
        }

        public DateTime PublishedAt
        {
            get { return _published_at; }
            set { _published_at = value; }
        }

        public int? PublishedBy
        {
            get { return _published_by; }
            set { _published_by = value; }
        }

        public IUser Author
        {
            get { return _author; }
            set { _author_id = value.Id; }
        }

        public IUser User
        {
            get { return _user; }
        }

        public ITag AddTag(string tagName)
        {
            var tag = (Tag) GetTag(tagName);
            if (tag != null)
                return tag;
            tag = new Tag {Name = tagName};
            _tags.Add(tag);
            return tag;
        }

        public IPost WithTag(string tagName)
        {
            AddTag(tagName);
            return this;
        }

        public IPost DeleteTag(string tagName)
        {
            var tag = GetTag(tagName);
            if (tag != null)
                _tags.Remove((Tag)tag);
            return this;
        }

        public ITag GetTag(string tagName)
        {
            foreach (var tag in _tags)
            {
                if (tag.Name == tagName)
                    return tag;
            }
            return null;
        }

        public bool HasTag(string tagName)
        {
            return GetTag(tagName) != null;
        }

        public IEnumerable<ITag> Tags
        {
            get
            {
                foreach (var tag in _tags)
                    yield return tag;
            }
        }

        public JObject ToJson()
        {
            return JsonSerializer.ConvertToJson(this);
        }
    }
}
