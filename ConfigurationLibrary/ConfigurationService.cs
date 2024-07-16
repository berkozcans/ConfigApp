namespace ConfigurationLibrary;

public class ConfigurationService
{
    public bool IsValidConfiguration(string config)
    {
        return !string.IsNullOrEmpty(config);
    }
}