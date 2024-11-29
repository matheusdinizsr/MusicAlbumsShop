using Microsoft.AspNetCore.Mvc;
using MusicAlbumsShop.FrontEnd.Components.Pages;
using MusicAlbumsShop.Shared.DTOs;

namespace MusicAlbumsShop.FrontEnd
{
    public class ApiClientService
    {
        private HttpClient _httpClient;
        private const string _apiAddress = "http://localhost:5003";
        public ApiClientService()
        {
            _httpClient = new HttpClient();
        }
        public async Task<BandWithName[]?> GetBands()
        {
            var result = await _httpClient.GetFromJsonAsync<BandWithName[]>($"{_apiAddress}/band/bands");

            return result;
        }

        public async Task<BandDetails?> GetBandDetails(int bandId)
        {
            var result = await _httpClient.GetFromJsonAsync<BandDetails>($"{_apiAddress}/band/{bandId}/details");

            return result;
        }

        public async Task AddBandAsync(string name, string origin, string yearsActive, int genreId)
        {
            await _httpClient.PostAsync($"{_apiAddress}/band?name={name}&origin={origin}&yearsActive={yearsActive}&genreId={genreId}", null);
        }

        public async Task<GenreWithTitle[]> GetGenres()
        {
            var result = await _httpClient.GetFromJsonAsync<GetGenresResponse>($"{_apiAddress}/genre/");

            return result?.GenresAndIds ?? [];
        }

        public async Task DeleteBand(int id)
        {
            var delete = await _httpClient.DeleteAsync($"{_apiAddress}/band?id={id}");
        }
    }
}
