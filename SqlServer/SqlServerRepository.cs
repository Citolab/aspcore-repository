using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Citolab.Repository.SqlServer
{
    /// <summary>
    /// <typeparam name="T"></typeparam>
    public class SqlServerRepository<T> : IRepository<T> where T : Citolab.Repository.Model, new()
    {
        private readonly SqlDatabaseContext _context;
        protected readonly ILogger Logger;
        public SqlServerRepository(ILoggerFactory loggerFactory, SqlDatabaseContext context)
        {
            _context = context;
            _context.AddType(typeof(T));
            Logger = loggerFactory.CreateLogger(GetType());
        }

        public IQueryable<T> AsQueryable()
        {
            try
            {
                return _context.Set<T>().AsQueryable();
            }
            catch
            {
                CreateTablesOnException();
                return _context.Set<T>().AsQueryable();
            }
        }

        public Task<T> GetAsync(Guid id) =>
            Task.Run<T>(() =>
            {
                try
                {
                    return _context.FindAsync<T>(id).Result;
                }
                catch
                {
                    CreateTablesOnException();
                    return _context.FindAsync<T>(id).Result;
                }
            });


        public Task<bool> UpdateAsync(T document, Guid? userId) =>
            Task.Run(() =>
            {
                try
                {
                    return Update();
                }
                catch
                {
                    CreateTablesOnException();
                    return Update();
                }
                bool Update()
                {
                    _context.Update(document);
                    return _context.SaveChangesAsync().Result > 0;
                }
            });

        public Task<bool> UpdateAsync(T document) =>
            UpdateAsync(document, null);

        public Task<T> AddAsync(T document, Guid? userId) =>
            Task.Run(() =>
            {
                var ret = _context.AddAsync(document)?.Result?.Entity;
                try
                {
                    _context.SaveChanges();
                }
                catch
                {
                    CreateTablesOnException();
                    _context.SaveChanges();
                }
                return ret;
            });


        public Task<T> AddAsync(T document) =>
            AddAsync(document);

        public Task<bool> DeleteAsync(Guid id, Guid? userId) =>
            Task.Run(() =>
            {
                try
                {
                    Remove();
                }
                catch
                {
                    CreateTablesOnException();
                    Remove();
                }
                void Remove()
                {
                    _context.Remove(_context.Find<T>(id));
                    _context.SaveChanges();
                }
                return true;
            });


        public Task<bool> DeleteAsync(Guid id) =>
             DeleteAsync(id, null);

        public Task<long> GetCountAsync() =>
            GetCountAsync(s => true);


        public Task<long> GetCountAsync(Expression<Func<T, bool>> filter) =>
            Task.Run(() =>
            {
                try { return (long)_context.Set<T>().Where(filter).Count(); }
                catch
                {
                    CreateTablesOnException();
                    return (long)_context.Set<T>().Where(filter).Count();
                }
            });

        public Task<bool> AnyAsync() => AnyAsync(s => true);


        public Task<bool> AnyAsync(Expression<Func<T, bool>> filter)
        {
            try { return _context.Set<T>().Where(filter).AnyAsync(); }
            catch
            {
                CreateTablesOnException();
                return _context.Set<T>().Where(filter).AnyAsync();
            }
        }


        public Task<T> FirstOrDefaultAsync() =>
            _context.Set<T>().FirstOrDefaultAsync();


        private void CreateTablesOnException()
        {
            try
            {
                var dbCreator = (RelationalDatabaseCreator)_context.Database.GetService<IDatabaseCreator>();
                dbCreator.CreateTables();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                //Ignore
            }
        }

    }
}
