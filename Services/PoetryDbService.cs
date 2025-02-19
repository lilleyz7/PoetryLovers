using PoetryLovers.DTO;

namespace PoetryLovers.Services
{
    public class PoetryDbService
    {
        private readonly HttpClient _client;
        public PoetryDbService(HttpClient client) 
        {
            _client = client;
        }

        public async Task<PoemDTO?> GetRandomPoemAsync()
        {
            var poem = await _client.GetFromJsonAsync<PoemDTO>("random");

            return poem;
        }

        public async Task<PoemDTO?> GetPoemByTitleAsync(string title)
        {
            var poem = await _client.GetFromJsonAsync<PoemDTO>($"title/{title}");

            return poem;
        }

        public async Task<List<PoemDTO>?> GetAuthorsPoems(string author, int count)
        {
            var poems = await _client.GetFromJsonAsync<List<PoemDTO>>($"author,count/{author};{count}");

            return poems;
        }
    }
}
