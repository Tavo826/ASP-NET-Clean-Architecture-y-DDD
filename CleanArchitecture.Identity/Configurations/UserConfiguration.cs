using CleanArchitecture.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Identity.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {

            var hasher = new PasswordHasher<ApplicationUser>();

            // Data de usuarios por defecto
            builder.HasData(
                new ApplicationUser
                {
                    Id = "b01737cd-70c3-45ce-b696-3c57b63285be",
                    Email = "admin@localhost.com",
                    NormalizedEmail = "admin@localhost.com",
                    Nombre = "Igor",
                    Apellidos = "Ditto",
                    UserName = "IgorDitto",
                    NormalizedUserName = "igorditto",
                    PasswordHash = hasher.HashPassword(null, "igorditto2000$"),
                    EmailConfirmed = true
                },
                new ApplicationUser
                {
                    Id = "47e96070-e3af-4c0b-bf89-a541314651fe",
                    Email = "juanperez@localhost.com",
                    NormalizedEmail = "juanperez@localhost.com",
                    Nombre = "Juan",
                    Apellidos = "Perez",
                    UserName = "JuanPerez",
                    NormalizedUserName = "juanperez",
                    PasswordHash = hasher.HashPassword(null, "juanperez2000$"),
                    EmailConfirmed = true
                }
            );
        }
    }
}
