using MongoDB.Driver;

namespace ConfigurationLibrary;

public class ConfigReader
{
        private readonly string _applicationName;
        private readonly IMongoCollection<Configuration> _collection;
        private Dictionary<string, Configuration> _configurations;
        private Dictionary<string, Configuration> _configurationCache;

        public ConfigReader(string applicationName, string connectionString, int refreshTimerIntervalInMs)
        {
            _applicationName = applicationName;
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("ConfigurationDB");
            _collection = database.GetCollection<Configuration>("Configurations");

            LoadConfigurationsFromMongo();
            _configurationCache = new Dictionary<string, Configuration>();

            var timer = new System.Threading.Timer(RefreshConfigurations, null, 0, refreshTimerIntervalInMs);
        }

        private void LoadConfigurationsFromMongo()
        {
            var filter = Builders<Configuration>.Filter.Eq("ApplicationName", _applicationName) &
                         Builders<Configuration>.Filter.Eq("IsActive", true);
            var configurations = _collection.Find(filter).ToList();
            _configurations = configurations.ToDictionary(c => c.Name);
        }

        private void RefreshConfigurations(object state)
        {
            LoadConfigurationsFromMongo();
        }

        public T GetValue<T>(string key)
        {
            try
            {
                if (_configurations.TryGetValue(key, out var config) && config.IsActive)
                {
                    _configurationCache[key] = config;

                    return (T)ChangeType(config.Value, typeof(T));
                }
                throw new KeyNotFoundException($"Key '{key}' not found or inactive.");
            }
            catch
            {
                if (_configurationCache.TryGetValue(key, out var cachedConfig))
                {
                    return (T)ChangeType(cachedConfig.Value, typeof(T));
                }
                throw new KeyNotFoundException($"Key '{key}' not found in cache.");
            }
        }

        private object ChangeType(object value, Type conversionType)
        {
            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                conversionType = Nullable.GetUnderlyingType(conversionType);
            }

            if (conversionType.IsEnum)
            {
                return Enum.Parse(conversionType, value.ToString());
            }

            return Convert.ChangeType(value, conversionType);
        }
        public IEnumerable<Configuration> GetAllConfigurations()
        {
            return _configurations.Values;
        }
}