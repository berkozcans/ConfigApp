namespace ConfigurationLibrary;
using MongoDB.Driver;
public class ConfigurationContext
{
    private readonly IMongoDatabase _database;

    public ConfigurationContext(string connectionString, string databaseName)
    {
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<Configuration> Configurations => _database.GetCollection<Configuration>("Configurations");
}