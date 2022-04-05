using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Order.Test;
using Orders.Core.Contracts;
using Orders.Core.Services;
using Orders.Infrastructure.Data.Common;
using Orders.Infrastructure.Data.Models;
using Orders.Infrastructure.Data.Repositories;
using System.Threading.Tasks;

namespace Orders.Test
{
    public class RestaurantsServicesTest
    {
        private ServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;

        [SetUp]
        public void Setup()
        {
            dbContext = new InMemoryDbContext();

            var serviceCollection = new ServiceCollection();
            serviceProvider = serviceCollection
                .AddSingleton(sp => dbContext.CreateContext())
                .AddSingleton<IApplicatioDbRepository, ApplicatioDbRepository>()
                .AddSingleton<IRestaurantService, RestaurantService>()
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IApplicatioDbRepository>();
            var seed = SeedDbAsync(repo);
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        private async Task SeedDbAsync(IApplicatioDbRepository repo)
        {
            var addres = new Address
            {
                Town = "София",
                Street = "Леге",
                Number = "10"
            };

            await repo.AddAsync(addres);
            await repo.SaveChangesAsync();
        }
    }
}