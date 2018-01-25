using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyModel;

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
