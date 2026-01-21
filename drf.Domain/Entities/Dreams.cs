using System.Security.Claims;

namespace dreamify.Domain.Entities;

public class Dream
{

    public int Id { get; set; }
    public string LocalDreamId { get; set; } // this id comes from the client, the purpose is to be able to dream/{dreamId}
    public Guid UserId { get; set; }  // ← Add this (foreign key)
    public User User { get; set; }
    public string? AnalyzedText { get; set; }
    public string? Tag { get; set; }
    public string FileName { get; set; }
    public string Title { get; set; }

    public string? TranscribedText { get; set; }
    public DateTime CreatedAt { get; set; }

    public static Dream Create(Guid userId,string localDreamId, string title, string transcribedText, DateTime createdAt)
    {
        //var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return new Dream
        {
            UserId = userId,
            LocalDreamId = localDreamId,
            AnalyzedText = null,
            Title = title,
            FileName = title,
            TranscribedText = transcribedText,
            CreatedAt = createdAt,
            Tag = null
        };
    }


}