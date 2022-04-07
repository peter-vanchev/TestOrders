using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Order.Test;
using Orders.Core.Contracts;
using Orders.Core.Models;
using Orders.Core.Services;
using Orders.Infrastructure.Data.Common;
using Orders.Infrastructure.Data.Models;
using Orders.Infrastructure.Data.Repositories;
using System;
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

            serviceCollection
                .AddDefaultIdentity<ApplicationUser>()
                .AddRoles<IdentityRole>();

            var repo = serviceProvider.GetService<IApplicatioDbRepository>();
            var seed = SeedDbAsync(repo);
        }

        [Test]
        public void RestaurantServicesGetAllSuxxess()
        {
            var service = serviceProvider.GetService<IRestaurantService>();

            Assert.DoesNotThrowAsync(async () => await service.GetAllAsync());
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        private async Task SeedDbAsync(IApplicatioDbRepository repo)
        {
            var address = new Address
            {
                Town = "София",
                Street = "Леге",
                Number = "10"
            };

            var restaurant = new Restaurant()
            {
                AddressId = address.Id,
                Address = address,
                Name = "Заведението",
                Category = "bar",
                PhoneNumner = "0897 456 654",
                Url = "/alabala.img",
                DataCreated = DateTime.Now
            };

          
            await repo.AddAsync(restaurant);
            await repo.SaveChangesAsync();
        }
    }
}