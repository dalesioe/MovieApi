using MovieApi.Models;

namespace MovieApi.Services;
public interface IMovieService
{
    Task<MovieDto?> GetMovieInfoAsync(string title);
}
