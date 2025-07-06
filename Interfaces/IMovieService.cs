using MovieApi.Models;

namespace MovieApi.Interfaces;
public interface IMovieService
{
    Task<MovieDto?> GetMovieInfoAsync(string title);
}
