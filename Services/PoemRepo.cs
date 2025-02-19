using Microsoft.EntityFrameworkCore;
using PoetryLovers.Data;
using PoetryLovers.DTO;
using PoetryLovers.Entities;
using PoetryLovers.Utilities;
using PoetryLovers.IServices;

namespace PoetryLovers.Services
{

    public class PoemRepo: IPoemRepo
    {
        public readonly PoemContext _context;
        public readonly PoetryDbService _apiService;

        public PoemRepo(PoemContext context, PoetryDbService apiService)
        {
            _context = context;
            _apiService = apiService;
        }

        public async Task<PoemResult<Poem>> GetPoemByTitle(string title)
        {
            var existingPoem = await _context.Poems.FirstOrDefaultAsync(p => p.Title == title);
            if (existingPoem is not null)
            {
                return new PoemResult<Poem>(existingPoem, null);
            }

            var newPoem = await _apiService.GetPoemByTitleAsync(title);
            if (newPoem is null)
            {
                return new PoemResult<Poem>(null, "Poem does not exist");
            }

            var poemToAdd = new Poem
            {
                Linecount = newPoem.Linecount,
                Lines = newPoem.Lines,
                Title = newPoem.Title,
                Author = newPoem.Author,
            };
            
            await _context.Poems.AddAsync(poemToAdd);
            int result = await _context.SaveChangesAsync();
            if (result == 0)
            {
                return new PoemResult<Poem>(null, "Unable to add poem");
            }

            return new PoemResult<Poem>(poemToAdd, null);
        }

        public async Task<PoemResult<PoemDTO>> GetRandomPoem()
        {
            var randomPoem = await _apiService.GetRandomPoemAsync();
            if (randomPoem is null)
            {
                return new PoemResult<PoemDTO>(null, "Failed to get poem");
            }

            return new PoemResult<PoemDTO>(randomPoem, null);
        }

        public async Task<List<PoemDTO>> GetPoemsByAuthor(string author, int count)
        {
            var poems = await _apiService.GetAuthorsPoems(author, count);
            if (poems is null)
            {
                return new List<PoemDTO>();
            }
            return poems;
        }
    }
}
