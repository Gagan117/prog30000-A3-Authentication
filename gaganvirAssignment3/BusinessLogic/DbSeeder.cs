

using gaganvirAssignment3.Data;
using gaganvirAssignment3.Models;
using Microsoft.AspNetCore.Identity;

namespace gaganvirAssignment3.BusinessLogic
{
    public class DbSeeder
    {
        // add roles (Admin , Customer) and default admin user
        public static async Task SeedAsync(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var context = services.GetRequiredService<ApplicationDbContext>();

            string[] roles = { "Admin", "Customer" };
            foreach (var r in roles)
            {
                if (!await roleManager.RoleExistsAsync(r))
                {
                    await roleManager.CreateAsync(new IdentityRole(r));
                }
            }

            await CreateAdmin(userManager);
            await CreateCustomer(userManager);
            await AddProducts(context);
        }


        private static async Task CreateAdmin(UserManager<IdentityUser> userManager)
        {
            var adminEmail = "admin@store.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(adminUser, "Admin#12345");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    await userManager.AddToRoleAsync(adminUser, "Customer");
                }
            }
        }

        private static async Task CreateCustomer(UserManager<IdentityUser> userManager)
        {
            var customerEmail = "customer@store.com";
            var customerUser = await userManager.FindByEmailAsync(customerEmail);
            if (customerUser == null)
            {
                customerUser = new IdentityUser
                {
                    UserName = customerEmail,
                    Email = customerEmail,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(customerUser, "Customer#12345");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(customerUser, "Customer");
                }
            }
        }

        private static async Task AddProducts(ApplicationDbContext context)
        {
            if (!context.Products.Any())
            {
                var products = new List<Product>
                {
                    new Product { Name="Iphone 12", Description="Apple", Category=Category.Apple, Price=1199, Supports5G=true, CreatedAt = DateTime.UtcNow },
                    new Product { Name="Iphone 14", Description="Apple", Category=Category.Apple, Price=1599, Supports5G=true, CreatedAt = DateTime.UtcNow },
                    new Product { Name="Samsumng Galaxy s22", Description="Samsung", Category=Category.Samsung, Price=899, Supports5G=true, CreatedAt = DateTime.UtcNow },
                    new Product { Name="Samsumng Galaxy s23", Description="Samsung", Category=Category.Samsung, Price=899, Supports5G=true, CreatedAt = DateTime.UtcNow },
                    new Product { Name="Samsumng Galaxy s24", Description="Samsung", Category=Category.Samsung, Price=899, Supports5G=true, CreatedAt = DateTime.UtcNow },
                    new Product { Name="Samsumng Galaxy s25", Description="Samsung", Category=Category.Samsung, Price=899, Supports5G=true, CreatedAt = DateTime.UtcNow },
                };
                context.Products.AddRange(products);
                await context.SaveChangesAsync();
            }
        }
    }
}
