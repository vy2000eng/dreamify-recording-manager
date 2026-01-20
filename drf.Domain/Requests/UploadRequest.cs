using dreamify.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace drf.Domain.Requests;



public record UploadRequest
{
    public IFormFile? File { get; init; }
    public string? FileName { get; init; }
    public string? Title { get; init; }
    public string? Tag { get; init; }
    public string? TranscribedText { get; init; }
    public string? CreatedAt { get; init; }
}

