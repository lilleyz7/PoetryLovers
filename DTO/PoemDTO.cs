using System.ComponentModel.DataAnnotations;

namespace PoetryLovers.DTO
{
    public class PoemDTO
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Author { get; set; } = string.Empty;

        [Required]
        public string[] Lines { get; set; } = Array.Empty<string>();

        public string Linecount { get; set; } = string.Empty;
    }
}
