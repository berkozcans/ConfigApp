using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ConfigurationLibrary;

public class Configuration
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId _id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Value { get; set; }
    public bool IsActive { get; set; }
    public string ApplicationName { get; set; }
}