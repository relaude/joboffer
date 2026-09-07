namespace JO.Persistence
{
    public interface IAppSettings
    {
        string GetConnectionStringName();
        string GetOneDriveLocation();
        bool IsProduction();
    }
}