namespace Citolab.Repository.Options
{
    public class InMemoryDatabaseOptions : IInMemoryDatabaseOptions
    {
        public bool FlagDelete { get; set; } = true;
        public bool TimeLoggingEnabled { get; set; }
    }
}

