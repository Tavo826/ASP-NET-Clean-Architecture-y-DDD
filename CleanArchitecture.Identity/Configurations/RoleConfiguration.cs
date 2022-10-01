using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Identity.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Id = "555aa169-6a64-428e-9a80-98479b3798af",
                    Name = "Administrator",
                    NormalizedName = "administrator"
                },
                new IdentityRole
                {
                    Id = "be353473-3b92-4742-be82-d729e646db06",
                    Name = "Operator",
                    NormalizedName = "operator"
                }
            );
        }
    }
}
