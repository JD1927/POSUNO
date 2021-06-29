using Microsoft.EntityFrameworkCore;
using POSUNO.API.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSUNO.API.Data
{
    public class SeedDB
    {
        private readonly DataContext _context;

        public SeedDB(DataContext context)
        {
            this._context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckUserAsync();
            await CheckCustomerAsync();
            await CheckProductAsync();
        }

        private async Task CheckUserAsync()
        {
            if (!_context.Users.Any())
            {
                _context.Users.Add(new User
                {
                    Email = "jd@yopmail.com",
                    FirstName = "Juan",
                    LastName = "Aguirre",
                    Password = "a12345",
                });
                _context.Users.Add(new User
                {
                    Email = "juan@yopmail.com",
                    FirstName = "Juan",
                    LastName = "Aguirre",
                    Password = "a12345",
                });
                _ = await _context.SaveChangesAsync();
            }
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
