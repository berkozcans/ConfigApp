using MongoDB.Driver;

namespace ConfigurationLibrary;

public class ConfigurationReader
{
    private readonly string _applicationName;
    private readonly string _connectionString;
    private readonly int _refreshTimerIntervalInMs;
    private readonly Timer _timer;
    private Dictionary<string, Configuration> _configurations;
    private readonly ConfigurationContext _context;
    private readonly IMongoCollection<Configuration> _collection;


    public ConfigurationReader(string applicationName, string connectionString, int refreshTimerIntervalInMs)
    {
        _applicationName = applicationName;
        _connectionString = connectionString;
        _refreshTimerIntervalInMs = refreshTimerIntervalInMs;
        _context = new ConfigurationContext(_connectionString, "ConfigurationDb");
        
        LoadConfigurations();

        // İlk yükleme ve periyodik güncellemeler için metodu çağır
        var client = new MongoClient(_connectionString);
        var database = client.GetDatabase("ConfigurationDb");
        _collection = database.GetCollection<Configuration>("Configurations");
        LoadConfigurationsFromMongo();
        _timer = new Timer(RefreshConfigurations, null, 0, _refreshTimerIntervalInMs);
    }
    private void LoadConfigurationsFromMongo()
    {
        var filter = Builders<Configuration>.Filter.And(
            Builders<Configuration>.Filter.Eq(c => c.ApplicationName, _applicationName),
            Builders<Configuration>.Filter.Eq(c => c.IsActive, true)
        );

        var configurations = _collection.Find(filter).ToList();
        _configurations = configurations.ToDictionary(c => c.Name, c => c);
    }
    private void LoadConfigurations()
    {
        var configurations = _context.Configurations
            .Find(c => c.ApplicationName == _applicationName && c.IsActive)
            .ToList();

        _configurations = configurations.ToDictionary(c => c.Name);
    }
    public List<Configuration> GetActiveConfigurations()
    {
        return _configurations.Values.ToList();
    }
    private void RefreshConfigurations(object state)
    {
        var updatedConfigurations = _context.Configurations
            .Find(c => c.ApplicationName == _applicationName && c.IsActive)
            .ToList();

        _configurations = updatedConfigurations.ToDictionary(c => c.Name);

    }
    public T GetValue<T>(string key)
    {
        if (_configurations.TryGetValue(key, out var config) && config.IsActive)
        {
            return (T)Convert.ChangeType(config.Value, typeof(T));
        }

        throw new KeyNotFoundException($"Key '{key}' not found or inactive.");
    }

}
