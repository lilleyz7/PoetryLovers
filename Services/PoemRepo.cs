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
        private readonly PoemContext _context;
        private readonly PoetryDbService _apiService;

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

        public async Task<PoemResult<List<PoemDTO>>> GetPoemsByAuthor(string author, int count)
        {
            try
            {
                var poems = await _apiService.GetAuthorsPoems(author, count);
                if (poems is null)
                {
                    return new PoemResult<List<PoemDTO>>(null, "Poem not found");
                }

                return new PoemResult<List<PoemDTO>>(poems, null);
            } catch(Exception ex)
            {
                return new PoemResult<List<PoemDTO>>(null, ex.Message);
            }
        }

        public async Task<PoemResult<List<Poem>>> GetSavedPoems(string userId, int count)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return new PoemResult<List<Poem>>(null, "Invalid user");
            }

            try
            {

                var user = await _context.Users.Include(u => u.SavedPoems).FirstOrDefaultAsync(u => u.Id == userId);
                if (user is null)
                {
                    return new PoemResult<List<Poem>>(null, "User does not exist");
                }
                return new PoemResult<List<Poem>>(user.SavedPoems, null);
            }
            catch (ArgumentNullException)
            {
                return new PoemResult<List<Poem>>(null, "Invalid arguments");
            }
            catch (Exception ex)
            {
                return new PoemResult<List<Poem>>(null, ex.Message);
            }
            
        }

        public async Task SavePoem(PoemDTO poemToAdd, string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user is null) {
                throw new Exception("Invalid request");
            }

            var existingPoem = await _context.Poems.FirstOrDefaultAsync(p => p.Title == poemToAdd.Title);

            if (existingPoem is null)
            {
                var poem = new Poem
                {
                    Title = poemToAdd.Title,
                    Author = poemToAdd.Author,
                    Linecount = poemToAdd.Linecount,
                    Lines = poemToAdd.Lines,
                };

                _context.Poems.Add(poem);
                user.SavedPoems.Add(poem);

                var changes = await _context.SaveChangesAsync();
                if (changes < 1)
                {
                    throw new Exception("Unable to add");
                }
            }
            else
            {
                var alreadySaved = user.SavedPoems.Find(p => p.Title == poemToAdd.Title);
                if (alreadySaved is null)
                {
                    try
                    {
                        existingPoem.SavedByUsers.Add(user);
                        await _context.SaveChangesAsync();

                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
                else
                {
                    try
                    {

                        existingPoem.SavedByUsers.Remove(user);
                        user.SavedPoems.Remove(alreadySaved);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }

                }
            }

            
        }


    }
}
