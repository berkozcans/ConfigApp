namespace ConfigurationLibrary;

public class Program
{
    public static void Main(string[] args)
    {
        var reader = new ConfigurationReader("SERVICE-A", "mongodb://localhost:27017", 60000);
        var siteName = reader.GetValue<string>("SiteName");
        Console.WriteLine(siteName); // Output: Boyner.com.tr
    }
}