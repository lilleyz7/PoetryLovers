using PoetryLovers.Entities;

namespace PoetryLovers.Utilities
{
    public class PoemResult<T>
    {
        public T? poem { get; set; }
        public string? error { get; set; }

        public PoemResult(T? poem, string? error)
        {
            this.poem = poem;
            this.error = error;
        }
    }
}
