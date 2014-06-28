using System;

namespace Rest4Net.Ghost.Responses
{
    public interface IUser
    {
        int Id { get; set; }
        Guid Uuid { get; set; }
        string Name { get; set; }
        string Slug { get; set; }
        string Email { get; set; }
        string Image { get; set; }
        string Cover { get; set; }
        string Bio { get; set; }
        string Website { get; set; }
        string Location { get; set; }
        string Accessibility { get; set; }
        string Status { get; set; }
        string Language { get; set; }
        string MetaTitle { get; set; }
        string MetaDescription { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
    }
}
