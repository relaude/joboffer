namespace JO.Persistence
{
    public interface IAppSettings
    {
        string GetBaseUrl();
        string GetConnectionStringName();
        string GetOneDriveLocation();
        bool IsProduction();
    }
}