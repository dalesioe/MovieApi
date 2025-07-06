using MovieApi.Models;
using MovieApi.Interfaces;

namespace MovieApi.Services;

public class MovieService : IMovieService
{
    private readonly IMovieProvider _movieProvider;

    public MovieService(IMovieProvider movieProvider)
    {
        _movieProvider = movieProvider;
    }

    public Task<MovieDto?> GetMovieInfoAsync(string title)
    {
        return _movieProvider.GetMovieByTitleAsync(title);
    }
}
