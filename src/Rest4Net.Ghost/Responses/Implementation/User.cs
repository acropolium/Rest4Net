using System;

namespace Rest4Net.Ghost.Responses.Implementation
{
    internal class User : IUser
    {
        private int _id;
        private Guid _uuid;
        private string _slug;
        private string _image;
        private string _status;
        private string _language;
        private string _meta_title;
        private string _meta_description;
        private DateTime _created_at;
        private DateTime _updated_at;
        private string _name;
        private string _email;
        private string _cover;
        private string _bio;
        private string _website;
        private string _location;
        private string _accessibility;

        public override string ToString()
        {
            return Email;
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

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public string Image
        {
            get { return _image; }
            set { _image = value; }
        }

        public string Cover
        {
            get { return _cover; }
            set { _cover = value; }
        }

        public string Bio
        {
            get { return _bio; }
            set { _bio = value; }
        }

        public string Website
        {
            get { return _website; }
            set { _website = value; }
        }

        public string Location
        {
            get { return _location; }
            set { _location = value; }
        }

        public string Accessibility
        {
            get { return _accessibility; }
            set { _accessibility = value; }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; }
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

        public DateTime CreatedAt
        {
            get { return _created_at; }
            set { }
        }

        public DateTime UpdatedAt
        {
            get { return _updated_at; }
            set { }
        }
    }
}
