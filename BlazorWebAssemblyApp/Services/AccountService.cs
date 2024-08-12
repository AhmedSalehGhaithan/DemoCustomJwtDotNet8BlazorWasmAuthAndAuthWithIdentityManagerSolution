using Blazored.LocalStorage;
using SharedClassLibrary.Contracts;
using SharedClassLibrary.DTOs;
using SharedClassLibrary.GenericModels;
using static SharedClassLibrary.DTOs.ServiceResponses;

namespace BlazorWebAssemblyApp.Services
{
    public class AccountService(ILocalStorageService localStorageService, HttpClient httpClient) : IUserAccount, IWeather
    {
       
        private const string BaseUrl = "api/Account";

        public async Task<GeneralResponse> CreateAccount(UserDTO userDTO)
        {
            var response = await httpClient
                 .PostAsync($"{BaseUrl}/register",
                 Generics.GenerateStringContent(
                 Generics.SerializeObj(userDTO)));

            //Read Response
            if (!response.IsSuccessStatusCode) return new GeneralResponse(false, "Error occurred. Try again later...");

            var apiResponse = await response.Content.ReadAsStringAsync();
            return Generics.DeserializeJsonString<GeneralResponse>(apiResponse);
        }

        public async Task<LoginResponse> LoginAccount(LoginDTO loginDTO)
        {
            //return HttpResponseMessage object,Represents a HTTP response message including the status code and data.
            var response = await httpClient.PostAsync($"{BaseUrl}/login",Generics.GenerateStringContent(
               Generics.SerializeObj(loginDTO)));

            if (!response.IsSuccessStatusCode) return new LoginResponse(false, null!, "Error occurred. try again later...");

            //Read Response
            var apiResponse = await response.Content.ReadAsStringAsync();
            return Generics.DeserializeJsonString<LoginResponse>(apiResponse);
        }

        public async Task<WeatherForecast[]> GetWeatherForecast()
        {
            string token = await localStorageService.GetItemAsStringAsync("token");
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await httpClient.GetAsync("api/WeatherForecast/user");


            //Read Response
            if (!response.IsSuccessStatusCode)
                return null!;

            var result = await response.Content.ReadAsStringAsync();
            return [.. Generics.DeserializeJsonStringList<WeatherForecast>(result)];
        }
    }
}
