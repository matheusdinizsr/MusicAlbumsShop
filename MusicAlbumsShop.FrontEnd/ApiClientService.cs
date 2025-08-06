using Microsoft.AspNetCore.Mvc;
using MusicAlbumsShop.FrontEnd.Components.Pages;
using MusicAlbumsShop.Shared.DTOs;
using MusicAlbumsShop.Shared;
using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;

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
        public async Task<ResultWrapper<BandWithNameAndGenre[]?>> GetBands()
        {
            ResultWrapper<BandWithNameAndGenre[]> wrapper = new();

            wrapper.SetError("Error. Try again.");

            try
            {
                var result = await _httpClient.GetFromJsonAsync<BandWithNameAndGenre[]>($"{_apiAddress}/band/bands");

                if (result != null)
                {
                    wrapper.SetSuccess(result);
                }
            }
            catch (Exception)
            {
            }

            return wrapper;
        }

        public async Task<ResultWrapper<BandDetails>> GetBandDetails(int bandId)
        {
            ResultWrapper<BandDetails> wrapper = new();

            wrapper.SetError("Error. Try again.");

            try
            {
                var result = await _httpClient.GetFromJsonAsync<BandDetails>($"{_apiAddress}/band/{bandId}/details");

                if (result != null)
                {
                    wrapper.SetSuccess(result);
                }

            }
            catch (HttpRequestException e)
            {

                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    wrapper.SetError("Band not found.");
                }

            }
            catch (Exception)
            {
            }

            return wrapper;
        }

        public async Task<ResultWrapper> AddOrUpdateBandAsync(int? bandId, string name, string origin, string yearsActive, int genreId)
        {
            var result = new HttpResponseMessage();
            var wrapper = new ResultWrapper();

            try
            {
                result = await _httpClient.PostAsync($"{_apiAddress}/band?bandId={bandId}&name={name}&origin={origin}&yearsActive={yearsActive}&genreId={genreId}", null);
                if (result.IsSuccessStatusCode)
                {
                    //var resultMessage = result.Content.ReadAsStringAsync().ToString();

                    wrapper.SetSuccess();
                }

            }
            catch (Exception)
            {
                wrapper.SetError("Error. Try again.");
            }

            return wrapper;
        }

        public async Task<ResultWrapper<GenreWithTitle[]?>> GetGenres()
        {
            ResultWrapper<GenreWithTitle[]> wrapper = new();
            var result = new GetGenresResponse();

            wrapper.SetError("Error. Try again.");

            try
            {
                result = await _httpClient.GetFromJsonAsync<GetGenresResponse>($"{_apiAddress}/genre/");

                if (result != null)
                {
                    wrapper.SetSuccess(result.GenresAndIds);
                }

            }
            catch (HttpRequestException e)
            {

                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    wrapper.SetError("There are no genres in the database.");
                }

            }
            catch (Exception)
            {
            }

            return wrapper;
        }

        public async Task<ResultWrapper> DeleteBand(int id)
        {
            var result = new HttpResponseMessage();
            var wrapper = new ResultWrapper();

            wrapper.SetError("Error. Try again.");

            try
            {
                result = await _httpClient.DeleteAsync($"{_apiAddress}/band?id={id}");

                if (result != null)
                {
                    var resultMessage = result.Content.ReadAsStringAsync().ToString();

                    wrapper.SetError(resultMessage);
                }

            }
            catch (Exception)
            {
            }

            return wrapper;
        }

        public async Task<ResultWrapper<AlbumWithTitle[]?>> GetAlbums()
        {
            ResultWrapper<AlbumWithTitle[]> wrapper = new();

            wrapper.SetError("Error. Try again.");

            try
            {
                var result = await _httpClient.GetFromJsonAsync<AlbumWithTitle[]>($"{_apiAddress}/album");

                if (result != null)
                {
                    wrapper.SetSuccess(result);
                }
            }
            catch (Exception)
            {
            }

            return wrapper;
        }
    }
}
