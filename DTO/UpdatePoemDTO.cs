using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PoetryLovers.DTO
{
    public class UpdatePoemDTO
    {
        [Required]
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [Required]
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("author")]
        public string Author { get; set; } = string.Empty;

        [JsonPropertyName("lines")]
        [Required]
        public string[] Lines { get; set; } = Array.Empty<string>();

        [JsonPropertyName("linecount")]
        public string Linecount { get; set; } = string.Empty;
    }
}
