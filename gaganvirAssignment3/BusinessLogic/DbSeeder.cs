

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
        }


        private static async Task CreateAdmin(UserManager<IdentityUser> userManager)
        {
            // Seed an admin user (adjust email/password!)
            var adminEmail = "admin@example.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(adminUser, "Admin#12345"); // use strong secrets!
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    await userManager.AddToRoleAsync(adminUser, "Customer");
                }
            }
        }

        private static async Task CreateCustomer(UserManager<IdentityUser> userManager)
        {
            // Seed a customer user (adjust email/password!)
            var customerEmail = "customer@example.com";
            var customerUser = await userManager.FindByEmailAsync(customerEmail);
            if (customerUser == null)
            {
                customerUser = new IdentityUser
                {
                    UserName = customerEmail,
                    Email = customerEmail,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(customerUser, "Customer#12345"); // use strong secrets!
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(customerUser, "Customer");
                }
            }
        }
    }
}
