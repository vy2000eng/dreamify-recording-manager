using System.Security.Claims;
using dreamify.Domain.Entities;
using drf.Application.Abstracts;
using drf.Domain.Requests;

namespace drf.Application.Services;

public class DatabaseService:IDatabaseService
{
    private readonly IDreamsRepository _dreamsRepository;

    public DatabaseService(IDreamsRepository dreamsRepository)
    {
        _dreamsRepository = dreamsRepository;
    }
    
    public async Task AddDreamToDataBase(ClaimsPrincipal claimsPrincipal,UploadRequest uploadRequest)
    {
        try
        {
            var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                         ?? claimsPrincipal.FindFirst("sub")?.Value;
            var dream = Dream.Create(Guid.Parse(userId), uploadRequest.FileName, uploadRequest.TranscribedText,
                DateTime.Parse(uploadRequest.CreatedAt));

            await _dreamsRepository.AddDream(dream);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw new Exception($"Unable to add Dream to data base: {uploadRequest.FileName}.");
        }
 
    }
    
    
    

    
}