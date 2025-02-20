using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PoetryLovers.DTO
{
    public class PoemDTO
    {
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

        public PoemDTO(string title, string author, string lineCount, string[] lines) 
        {
            Title = title;
            Author = author;
            Linecount = lineCount;
            Lines = lines;
        }
    }
}
