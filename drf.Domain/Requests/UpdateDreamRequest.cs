namespace drf.Domain.Requests;

public record UpdateDreamRequest
{
    public string DreamId       { get; set; }
    public string? Title         { get; set; }
    public string? Transcription { get; set; }
    public string? Analysis      { get; set; }
    
    public string? Tag         { get; set; }
}