using System;

namespace Rest4Net.Ghost.Responses
{
    public interface ITag : IJsonifable
    {
        int Id { get; }
        Guid Uuid { get; }
        string Name { get; set; }
        string Slug { get; set; }
        string Description { get; set; }
        int? ParentId { get; set; }
        string MetaTitle { get; set; }
        string MetaDescription { get; set; }
        DateTime CreatedAt { get; set; }
        int CreatedBy { get; set; }
        DateTime UpdatedAt { get; set; }
        int UpdatedBy { get; set; }
    }
}
