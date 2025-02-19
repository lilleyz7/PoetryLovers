using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace PoetryLovers.Entities
{
    public class Poem
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Author { get; set; } = string.Empty;

        [Required]
        public string[] Lines { get; set; } = Array.Empty<string>();

        public string Linecount { get; set; } = string.Empty;

        public ICollection<User> SavedByUsers { get; set; } = new List<User>();
    }
}
