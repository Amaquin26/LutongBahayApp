using LutongBahayApp.Models;
using Microsoft.AspNetCore.Identity;
using MVCWebApp.Models;
using System.Diagnostics;
using System.Net;

namespace LutongBahayApp.Data
{
    public class Seed
    {
        // Run SeedUsersAndRolesAsync First before This
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                context.Database.EnsureCreated();

                if (!context.Foods.Any())
                {
                    context.Foods.AddRange(new List<Food>()
                    {
                        new Food()
                        {
                            Name = "Adobong Manok",
                            Description = "Classic Filipino style Adobong Manok",
                            ImagePath = "/images//sample_adobo.png",
                            Price = 30,
                            MarketId = 1, // Assuming that Market 1 already exist
                        },
                        new Food()
                        {
                            Name = "Sinigang",
                            Description = "Asim style sinigang for the family.",
                            ImagePath = "/images//sample_pizza.png",
                            Price = 30,
                            MarketId = 1, // Assuming that Market 1 already exist
                        },
                        new Food()
                        {
                            Name = "Tortang Talong",
                            Description = "Tortang talong with love.",
                            ImagePath = "/images//sample_talong.png",
                            Price = 25,
                            MarketId = 1, // Assuming that Market 1 already exist
                        }
                    });
                    context.SaveChanges();
                }

                if (!context.Reviews.Any())
                {
                    context.Reviews.AddRange(new List<Review>()
                    {
                        new Review()
                        {
                            Title = "Affordalicious",
                            Comment = "I really enjoy the adobo",
                            Location = "Punta Princesa Cebu City",
                            Rating = 5,
                            FoodId = 1, // Assuming Food Item 1 exist
                            UserId = "0e3df520-98d8-45fa-9210-83a9e5504512" // Assuming it exist and is correct
                        },
                        new Review()
                        {
                            Title = "Not much",
                            Comment = "Slow delivery and did not like the food sorry.",
                            Location = "Cebu City",
                            Rating = 1,
                            FoodId = 1, // Assuming Food Item 1 exist
                            UserId = "f064153e-5f9e-440a-a099-a9607ae2c749" // Assuming it exist and is correct
                        }
                    });
                    context.SaveChanges();
                }
            }
        }

        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                context.Database.EnsureCreated();

                if (!await roleManager.RoleExistsAsync(AppRole.Admin))
                    await roleManager.CreateAsync(new AppRole { Name = AppRole.Admin });
                if (!await roleManager.RoleExistsAsync(AppRole.Customer))
                    await roleManager.CreateAsync(new AppRole { Name = AppRole.Customer });
                if (!await roleManager.RoleExistsAsync(AppRole.Seller))
                    await roleManager.CreateAsync(new AppRole { Name = AppRole.Seller });

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                string adminUserEmail = "brayljamesamaquin@gmail.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new AppUser()
                    {
                        UserName = "BraylJames",
                        Email = adminUserEmail,
                        EmailConfirmed = true,
                        FirstName = "Brayl James",
                        LastName = "Amaquin",
                        Address = "Punta Princesa Cebu City",
                        DateJoined = DateTime.Now
                    };
                    await userManager.CreateAsync(newAdminUser, "Amaquin1234!");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }

                string customerUserEmail = "customer@gmail.com";

                var appUser = await userManager.FindByEmailAsync(customerUserEmail);
                if (appUser == null)
                {
                    var newCustomerUser = new AppUser()
                    {
                        UserName = "Customer",
                        Email = customerUserEmail,
                        EmailConfirmed = true,
                        FirstName = "Customer",
                        LastName = "Customer",
                        Address = "Cebu City",
                        DateJoined = DateTime.Now
                    };
                    await userManager.CreateAsync(newCustomerUser, "Customer1234!");
                    await userManager.AddToRoleAsync(newCustomerUser, UserRoles.Customer);
                }

                string sellerUserEmail = "seller@gmail.com";

                var sellerUser = await userManager.FindByEmailAsync(sellerUserEmail);
                if (sellerUser == null)
                {
                    var newSellerUser = new AppUser()
                    {
                        UserName = "Seller",
                        Email = sellerUserEmail,
                        EmailConfirmed = true,
                        FirstName = "Seller",
                        LastName = "Seller",
                        Address = "Cebu City",
                        DateJoined = DateTime.Now,            
                    };
                    await userManager.CreateAsync(newSellerUser, "Seller1234!");
                    await userManager.AddToRoleAsync(newSellerUser, UserRoles.Seller);

                    if (!context.Markets.Any())
                    {
                        context.Markets.AddRange(new List<Market>()
                        {
                            new Market()
                            {
                                Name = "The Market",
                                Address = "Mambaling Cebu City",
                                Location = "Near Mambaling Elementary School",
                                UserId = newSellerUser.Id,
                                DateCreated = DateTime.Now,
                                
                            }
                        });
                        context.SaveChanges();
                    }
                }
            }
        }
    }
}
