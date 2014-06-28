using System;
using System.Collections.Generic;

namespace Rest4Net.Ghost.Responses
{
    public interface IPost : IJsonifable
    {
        int Id { get; }
        Guid Uuid { get; }
        string Title { get; set; }
        string Slug { get; set; }
        string Markdown { get; set; }
        string Html { get; }
        string Image { get; set; }
        bool IsFeatured { get; set; }
        bool IsPage { get; set; }
        ContentStatus Status { get; set; }
        string Language { get; set; }
        string MetaTitle { get; set; }
        string MetaDescription { get; set; }
        int AuthorId { get; set; }
        DateTime CreatedAt { get; set; }
        int CreatedBy { get; set; }
        DateTime UpdatedAt { get; set; }
        int UpdatedBy { get; set; }
        DateTime PublishedAt { get; set; }
        int? PublishedBy { get; set; }
        IUser Author { get; set; }
        IUser User { get; }
        ITag AddTag(string tagName);
        ITag GetTag(string tagName);
        IPost WithTag(string tagName);
        IPost DeleteTag(string tagName);
        bool HasTag(string tagName);
        IEnumerable<ITag> Tags { get; }
    }
}
