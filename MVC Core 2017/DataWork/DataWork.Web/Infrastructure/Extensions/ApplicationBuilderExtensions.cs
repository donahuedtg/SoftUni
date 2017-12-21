namespace DataWork.Web.Infrastructure.Extensions
{
    using Data;
    using Data.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System.Threading.Tasks;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDatabaseMigration(this IApplicationBuilder app)
        {

            using (var service = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                service.ServiceProvider.GetService<DataWorkDbContext>().Database.Migrate();

                var userManager = service.ServiceProvider.GetService<UserManager<User>>();
                var roleManager = service.ServiceProvider.GetService<RoleManager<IdentityRole>>();

                Task
                    .Run(async () => 
                    {
                        string adminName = GlobalConstString.AdministratorRole;

                        string[] roles = new[]
                        {
                            adminName,
                            GlobalConstString.WorkerRole,
                            GlobalConstString.ManagerRole
                        };

                        foreach (var role in roles)
                        {
                            bool roleExist = await roleManager.RoleExistsAsync(role);

                            if (!roleExist)
                            {
                                IdentityRole newRole = new IdentityRole();
                                newRole.Name = role;
                                await roleManager.CreateAsync(newRole);
                            }
                        }

                        string adminEmail = "admin@admin.test";

                        var adminUser = await userManager.FindByEmailAsync(adminEmail);

                        if (adminUser == null)
                        {
                            adminUser = new User();
                            adminUser.UserName = adminName;
                            adminUser.Email = adminEmail;
                            adminUser.IdNumber = "1234567890";

                            await userManager.CreateAsync(adminUser, "123Asd$");

                            await userManager.AddToRoleAsync(adminUser, adminName);
                        }


                    })
                    .Wait();
            }

            return app;

        }
    }
}
