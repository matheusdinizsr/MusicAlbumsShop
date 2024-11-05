using MusicAlbumsShop.FrontEnd.Components.Pages;
using MusicAlbumsShop.Shared.DTOs;

namespace MusicAlbumsShop.FrontEnd
{
    public class ApiClientService
    {
        private HttpClient _httpClient;
        private const string _apiAddress = "https://localhost:7151/";
        public ApiClientService()
        {
            _httpClient = new HttpClient();
        }
        public async Task<BandWithName[]?> GetBands()
        {
            var result = await _httpClient.GetFromJsonAsync<BandWithName[]>($"{_apiAddress}/bands");

            return result;
        }
    }
}
