namespace Citolab.Repository.Options
{
    public interface IRepositoryOptions
    {
        bool FlagDelete { get; set; }
        bool TimeLoggingEnabled { get; set; }
    }
}