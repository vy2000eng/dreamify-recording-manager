namespace drf.Infrastructure.Options;

public class AwsOptions
{
     public const string AwsOptionsKey = "AWS"; 
     public string AccessKey {get; set;}
     public string SecretKey {get; set;}
     public string ServiceUrl {get; set;}
     public string Bucket {get; set;}
     
     public string Region {get; set;}

}