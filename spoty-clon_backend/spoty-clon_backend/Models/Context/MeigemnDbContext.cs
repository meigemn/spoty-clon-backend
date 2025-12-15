using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace spoty_clon_backend.Models.Context
{
    public class MeigemnDbContext : IdentityDbContext<IdentityUser>
    {
        public MeigemnDbContext (DbContextOptions<MeigemnDbContext>options)
            : base (options)
            {
            }
            
    }
}
