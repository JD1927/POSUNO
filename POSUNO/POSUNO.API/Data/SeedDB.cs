using Microsoft.EntityFrameworkCore;
using POSUNO.API.Data.Entities;
using POSUNO.API.Enums;
using POSUNO.API.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSUNO.API.Data
{
    public class SeedDB
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;


        public SeedDB(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckRolesAsync();
            await CheckUserAsync("Juan", "Aguirre", "ja@yopmail.com", "123 123113");
            await CheckUserAsync("JD", "nulo", "jd@yopmail.com", "123 1231321");
            await CheckCustomerAsync();
            await CheckProductAsync();
        }

        private async Task<User> CheckUserAsync(string firstName, string lastName, string email, string phoneNumber)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phoneNumber,
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, UserType.Admin.ToString());
                await _userHelper.AddUserToRoleAsync(user, UserType.User.ToString());
            }

            return user;
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }

        private async Task CheckCustomerAsync()
        {
            if (!_context.Products.Any())
            {
                User user = await _context.Users.FirstOrDefaultAsync();
                for (int i = 0; i <= 50; i++)
                {
                    _context.Customers.Add(new Customer
                    {
                        Email = $"cliente{i}@yopmail.com",
                        FirstName = $"Cliente {i}",
                        LastName = $"Apellido {i}",
                        PhoneNumber = "1234567",
                        Address = "Calle 12 Viva El Paro",
                        IsActive = true,
                        User = user
                    });
                }
                _ = await _context.SaveChangesAsync();
            }
        }

        private async Task CheckProductAsync()
        {
            if (!_context.Products.Any())
            {
                Random random = new Random();
                User user = await _context.Users.FirstOrDefaultAsync();
                for (int i = 0; i <= 200; i++)
                {
                    _context.Products.Add(new Product
                    {
                        Name = $"Producto {i}", 
                        Description = $"Producto {i}",
                        Price = random.Next(5, 1000),
                        Stock = random.Next(0, 500),
                        IsActive = true,
                        User = user
                    });
                }
                _ = await _context.SaveChangesAsync();
            }
        }
    }
}
