using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PoetryLovers.Entities;

namespace PoetryLovers.Data
{
    public class PoemContext: IdentityDbContext<User>
    {
        public DbSet<Poem> Poems { get; set; }

        public PoemContext(DbContextOptions<PoemContext> options) : base(options)
        {
        }
    }
}
