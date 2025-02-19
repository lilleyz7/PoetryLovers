using PoetryLovers.Data;

namespace PoetryLovers.Services
{
    public class PoemRepo
    {
        public readonly PoemContext _context;

        public PoemRepo(PoemContext context)
        {
            _context = context;
        }


    }
}
