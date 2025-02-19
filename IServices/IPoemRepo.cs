using PoetryLovers.Data;
using PoetryLovers.Entities;
using PoetryLovers.DTO;
using PoetryLovers.Utilities;

namespace PoetryLovers.IServices
{
    public interface IPoemRepo
    {
        public Task<List<PoemDTO>> GetPoemsByAuthor(string author);
        public Task<PoemResult<Poem>> GetPoemByTitle(string title);
        public Task<PoemResult<PoemDTO>> GetRandomPoem();
    }
}
