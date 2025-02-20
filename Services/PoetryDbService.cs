using PoetryLovers.DTO;

namespace PoetryLovers.Services
{
    public class PoetryDbService
    {
        public readonly HttpClient _client;
        public PoetryDbService(HttpClient client) 
        {
            _client = client;
        }

        public async Task<PoemDTO?> GetRandomPoemAsync()
        {

            var response = await _client.GetFromJsonAsync<List<PoemDTO>>("random");
            if (response is null)
            {
                return null;
            }

            return response[0];
        }

        public async Task<PoemDTO?> GetPoemByTitleAsync(string title)
        {
            var response = await _client.GetFromJsonAsync<List<PoemDTO>>($"title/{title}");
            if (response is null)
            {
                return null;
            }

            return response[0];
        }

        public async Task<List<PoemDTO>?> GetAuthorsPoems(string author, int count)
        {
            var poems = await _client.GetFromJsonAsync<List<PoemDTO>>($"author,count/{author};{count}");

            return poems;
        }
    }
}
