using ConfigurationLibrary;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ConfigurationConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("ConfigurationDb");
            var collection = database.GetCollection<Configuration>("Configurations");

            var filter = Builders<Configuration>.Filter.And(
                Builders<Configuration>.Filter.Eq(c => c.ApplicationName, "SERVICE-A"),
                Builders<Configuration>.Filter.Eq(c => c.IsActive, false)
            );

            var configurations = collection.Find(filter).ToList();

            foreach (var config in configurations)
            {
                Console.WriteLine(
                    $"ID: {config.ID}, Name: {config.Name}, Type: {config.Type}, Value: {config.Value}, IsActive: {config.IsActive}, ApplicationName: {config.ApplicationName}");
            }
        }

        public class Configuration
        {
            public ObjectId Id { get; set; }
            public int ID { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public string Value { get; set; }
            public bool IsActive { get; set; }
            public string ApplicationName { get; set; }
        }
    }
}