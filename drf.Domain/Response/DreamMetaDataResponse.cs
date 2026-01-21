using System.Text.Json.Serialization;
using dreamify.Domain.Entities;

namespace drf.Domain.Response;

public class DreamMetaDataResponse
{
    [JsonPropertyName("dreamMetaData")]

    public List<Dream> Dreams { get; set; }
}