using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Citolab.Repository.SqlServer
{
    public class SqlDatabaseContext : DbContext
    {
        private ModelBuilder _modelBuilder;

        public void AddType(Type t)
        {
            var m = _modelBuilder?.Model.GetOrAddEntityType(t);
            if (m != null && !m.GetKeys().Any())
            {
                m.AddKey(m.GetProperties().Where(p => p.Name == "Id").ToList());
            }
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseSqlServer(SqlServerFactory.ConnectionString);

        protected override void OnModelCreating(ModelBuilder modelBuilder) =>
            _modelBuilder = modelBuilder;

    }

}
