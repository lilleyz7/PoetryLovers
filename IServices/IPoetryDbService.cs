using PoetryLovers.DTO;

namespace PoetryLovers.IServices
{
    public interface IPoetryDbService
    {
        public Task<PoemDTO?> GetRandomPoemAsync();

        public Task<PoemDTO?> GetPoemByTitleAsync(string title);

        public Task<List<PoemDTO>?> GetAuthorsPoems(string author, int count);

    }
}
