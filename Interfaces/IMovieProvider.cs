using MovieApi.Models;

namespace MovieApi.Interfaces
{
    public interface IMovieProvider
    {
        Task<MovieDto?> GetMovieByTitleAsync(string title);
    }
}
