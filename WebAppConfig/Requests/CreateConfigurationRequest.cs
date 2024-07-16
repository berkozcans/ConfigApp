using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebAppConfig.Requests;

public class CreateConfigurationRequest
{
    public string Name { get; set; }
    [Range(100, 20000, ErrorMessage = "Miktar alanı 100 ile 20.000 arasında olmalıdır.")]
    public string Value { get; set; }
    [Range(1, 28, ErrorMessage = "1 ve 28 günleri arasında olmalıdır.")]
    public string Type { get; set; }

    public bool IsActive { get; set; }
    public string ApplicationName { get; set; }
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId _id { get; set; }
}