using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Identity.Configurations
{
    public class UserRolConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "555aa169-6a64-428e-9a80-98479b3798af",
                    UserId = "b01737cd-70c3-45ce-b696-3c57b63285be"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "be353473-3b92-4742-be82-d729e646db06",
                    UserId = "47e96070-e3af-4c0b-bf89-a541314651fe"
                }
            );
        }
    }
}
