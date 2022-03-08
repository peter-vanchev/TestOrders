<<<<<<< HEAD
﻿using Microsoft.AspNetCore.Identity;
using TestOrders.Contracts;
=======
﻿using TestOrders.Contracts;
>>>>>>> e861814d30e26a93f10edce1a3f4906e5ef6ea83
using TestOrders.Data.Common;
using TestOrders.Data.Models;
using TestOrders.Models;

namespace TestOrders.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRepository repo;
<<<<<<< HEAD
        private readonly UserManager<ApplicationUser> userManager;


        public RestaurantService(
            IRepository _repo,
            UserManager<ApplicationUser> _userManager)
        {
            repo = _repo;
            userManager = _userManager;
        }

        public async Task<(bool created, string error)> Create(RestaurantViewModel model)
=======

        public RestaurantService(IRepository _repo)
        {
            repo = _repo;
        }

        public (bool created, string error) Create(RestaurantViewModel model)
>>>>>>> e861814d30e26a93f10edce1a3f4906e5ef6ea83
        {
            bool created = true;
            string error = null;

<<<<<<< HEAD
            var user = new ApplicationUser
            {
                Email = model.UserEmail,
                UserName = model.UserEmail,
                EmailConfirmed = true
            };

=======
            //var (isValid, validationError) = validationService.ValidateModel(model);
>>>>>>> e861814d30e26a93f10edce1a3f4906e5ef6ea83
            var address = new Address()
            {
                Town = model.Town,
                Number = model.Number,
                Street = model.Street
            };

            var restaurant = new Restaurant()
            {
                AddressId = address.Id,
                Address = address,
                Name = model.Name,
                Category = model.Category,
                Description = model.Description,
                PhoneNumner = model.PhoneNumner,
                Url = model.Url                
            };

            try
            {
<<<<<<< HEAD
                await userManager.CreateAsync(user, model.UserPassword);
=======
>>>>>>> e861814d30e26a93f10edce1a3f4906e5ef6ea83
                repo.Add(restaurant);
                repo.SaveChanges();
                created = true;
            }
            catch (Exception)
            {
                error = "Could not save product";
            }

<<<<<<< HEAD
            var temp = await userManager.AddToRoleAsync(user, "Restaurant");

=======
>>>>>>> e861814d30e26a93f10edce1a3f4906e5ef6ea83
            return (created, error);
        }

        public (bool created, string error) Delete(string restaurantId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ObjectViewModel> GetAll()
        {
            var restaurant = repo.All<Restaurant>()
                .Select(p => new ObjectViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Category = p.Category,
                    Description = p.Description,
                    Url = p.Url                    
                })
                .ToList();

            return restaurant;
        }

        public IEnumerable<ProductViewModel> GetMenu(string restorantId)
        {
            var restaurant = repo.All<Product>()

                .Select(p => new ProductViewModel()
                {
                    Name = p.Name,
                    Category = p.Category,
                    Description = p.Description,
                    Url = p.Url
                })
                .ToList();

            return restaurant;
        }


    }
}


