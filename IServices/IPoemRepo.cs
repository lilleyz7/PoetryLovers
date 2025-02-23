using PoetryLovers.Data;
using PoetryLovers.Entities;
using PoetryLovers.DTO;
using PoetryLovers.Utilities;

namespace PoetryLovers.IServices
{
    public interface IPoemRepo
    {
        public Task<PoemResult<List<PoemDTO>>> GetPoemsByAuthor(string author, int count);

        public Task<PoemResult<List<Poem>>> GetSavedPoems(string userId, int count);
        public Task<PoemResult<Poem>> GetPoemByTitle(string title);
        public Task<PoemResult<PoemDTO>> GetRandomPoem();
        public Task SavePoem(PoemDTO poemToAdd, string userId);
    }
}
