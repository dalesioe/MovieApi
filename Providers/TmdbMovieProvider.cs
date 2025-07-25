﻿using MovieApi.Models;
using MovieApi.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace MovieApi.Services;

public class TmdbMovieProvider : IMovieProvider
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IConfiguration _config;
    private readonly IDistributedCache _cache;

    public TmdbMovieProvider(IHttpClientFactory clientFactory, IConfiguration config, IDistributedCache cache)
    {
        _clientFactory = clientFactory;
        _config = config;
        _cache = cache;
    }

    public async Task<MovieDto?> GetMovieByTitleAsync(string title)
    {
        var cacheKey = $"movie:{title.ToLower()}";
        var cached = await _cache.GetStringAsync(cacheKey);
        if (!string.IsNullOrEmpty(cached))
            return JsonSerializer.Deserialize<MovieDto>(cached);

        var apiKey = _config["Tmdb:ApiKey"];
        var client = _clientFactory.CreateClient("tmdb");

        var search = await client.GetFromJsonAsync<JsonElement>($"search/movie?query={Uri.EscapeDataString(title)}&api_key={apiKey}");
        var firstResult = search.GetProperty("results").EnumerateArray().FirstOrDefault();
        if (firstResult.ValueKind == JsonValueKind.Undefined) return null;

        var movieId = firstResult.GetProperty("id").GetInt32();
        var movieDetails = await client.GetFromJsonAsync<JsonElement>($"movie/{movieId}?api_key={apiKey}");
        var similar = await client.GetFromJsonAsync<JsonElement>($"movie/{movieId}/similar?api_key={apiKey}");

        var movieDto = new MovieDto
        {
            Title = movieDetails.GetProperty("title").GetString() ?? "Sin título",
            OriginalTitle = movieDetails.GetProperty("original_title").GetString() ?? "",
            VoteAverage = movieDetails.GetProperty("vote_average").GetDouble(),
            ReleaseDate = movieDetails.GetProperty("release_date").GetString() ?? "",
            Overview = movieDetails.GetProperty("overview").GetString() ?? "",
            SimilarMovies = similar.GetProperty("results")
                .EnumerateArray()
                .Take(5)
                .Select(x =>
                {
                    var simTitle = x.GetProperty("title").GetString() ?? "Sin título";
                    var date = x.GetProperty("release_date").GetString();
                    var year = (date?.Length >= 4) ? date[..4] : "????";
                    return $"{simTitle} ({year})";
                }).ToList()
        };

        var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
        await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(movieDto), options);

        return movieDto;
    }
}
