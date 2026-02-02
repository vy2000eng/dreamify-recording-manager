using System.Security.Claims;
using dreamify.Domain.Entities;
using drf.Application.Abstracts;
using drf.Domain.Exceptions;
using drf.Domain.Requests;

namespace drf.Application.Services;

public class DatabaseService:IDatabaseService
{
    private readonly IDreamsRepository _dreamsRepository;
    private readonly IUserRepository _userRepository;

    public DatabaseService(IDreamsRepository dreamsRepository,IUserRepository userRepository)
    {
        _dreamsRepository = dreamsRepository;
        _userRepository = userRepository;
    }
    
    public async Task AddDreamToDataBase(ClaimsPrincipal claimsPrincipal,UploadRequest uploadRequest)
    {
        try
        {
            var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                         ?? claimsPrincipal.FindFirst("sub")?.Value;
            if (userId == null)
            {
                throw new UserNotFoundException();
            }
            
            var dream = Dream.Create(Guid.Parse(userId),uploadRequest.LocalDreamId, uploadRequest.FileName, uploadRequest.TranscribedText,
                DateTime.Parse(uploadRequest.CreatedAt));

            await _dreamsRepository.AddDream(dream);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw new Exception($"Unable to add Dream to data base: {uploadRequest.FileName}.");
        }
 
    }

    public async Task<Dream?> GetDream( ClaimsPrincipal claimsPrincipal,string dreamId)
    {
        var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                     ?? claimsPrincipal.FindFirst("sub")?.Value;
        if (userId == null)
        {
            throw new UserNotFoundException();
        }
        var dreams = await _userRepository.GetUserDreams(userId);
       
       return dreams.Find(dream => dream.LocalDreamId == dreamId);


    }

    public async Task<List<Dream>> GetDreamMetaData(ClaimsPrincipal claimsPrincipal)
    {
        var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                     ?? claimsPrincipal.FindFirst("sub")?.Value;
        if (userId == null)
        {
            throw new UserNotFoundException();
        }
        return await _userRepository.GetUserDreams(userId);


    }



    public async Task UpdateDream(ClaimsPrincipal claimsPrincipal, UpdateDreamRequest request)
    {
        var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                     ?? claimsPrincipal.FindFirst("sub")?.Value;
        if (userId == null)
        {
            throw new UserNotFoundException();
        }
        
        
        var dream  = await _dreamsRepository.GetDream(request.DreamId);

        if (request.Tag != null)
            dream.Tag = request.Tag;
    
        if (request.Title != null)
            dream.Title = request.Title;
    
        if (request.Transcription != null)
            dream.TranscribedText = request.Transcription;
    
        if (request.Analysis != null)
            dream.AnalyzedText = request.Analysis;
    
        await _dreamsRepository.UpdateDream(dream);


        }
    }
    
    

    
