using System;
using Citolab.Repository.Mongo.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Citolab.Repository.Mongo.Tests
{
    [TestClass]
    public class ConnectionTests
    {
        [TestMethod]
        public void ConnectToNonexistentServer_ShouldThrowTimeoutException()
        {
            
            IServiceCollection services = new ServiceCollection();

            var mongoDbConnectionString = "doesnotexist:27017";
            services.AddMemoryCache();
            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
            });
            services.AddMongoRepository("testdb", mongoDbConnectionString);

            var sp = services.BuildServiceProvider();
            var repositoryFactory = sp.GetService<IRepositoryFactory>();

            Assert.ThrowsException<TimeoutException>(() =>
            {
                var testModelRepo = repositoryFactory.GetRepository<TestModel>();
            });
        }
    }
}
