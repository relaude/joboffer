namespace JO.Persistence
{
    public interface IAppSettings
    {
        string GetConnectionStringName();
        bool IsProduction();
    }
}