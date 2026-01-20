using System.Security.Claims;

namespace dreamify.Domain.Entities;

public class Dream
{

    public int Id { get; set; }
    public Guid UserId { get; set; }  // ← Add this (foreign key)
    public User User { get; set; }
    public string? AnalyzedText { get; set; }
    public string? Tag { get; set; }
    public string FileName { get; set; }
    public string Title { get; set; }

    public string? TranscribedText { get; set; }
    public DateTime CreatedAt { get; set; }

    public static Dream Create(Guid userId,string title, string transcribedText, DateTime createdAt)
    {
        //var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return new Dream
        {
            UserId = userId,
            AnalyzedText = null,
            Title = title,
            FileName = title,
            TranscribedText = transcribedText,
            CreatedAt = createdAt,
            Tag = null
        };
    }


}