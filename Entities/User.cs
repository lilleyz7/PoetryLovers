using Microsoft.AspNetCore.Identity;

namespace PoetryLovers.Entities
{
    public class User: IdentityUser
    {
        public List<Poem> SavedPoems { get; } = new List<Poem>();
    }
}
