using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ArcheryProjectApp.Data
{
    public class LoginService
    {
        private readonly HttpClient _httpClient;
        private static readonly HttpClient _client = new HttpClient();
        private string url = "https://archeryapp-214121140527.us-central1.run.app/api/Auth/Login";

        private static readonly JsonSerializerSettings _serializerOptions = new JsonSerializerSettings
        {
            ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
            Formatting = Formatting.Indented
        };

        public LoginService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> LoginAsync(string username, string password)
        {

            var user = new { userName = username, password = password };
            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                bool isTokenStored = await IsTokenStoredAsync();
                if (isTokenStored)
                {
                    Debug.WriteLine($"Token is already stored: {token}");
                }
                else
                {
                    await SecureStorage.SetAsync("token", token);
                    Debug.WriteLine("Token has been stored.");
                }

                return token;
            }

            return null;
        }

        public async Task<bool> IsTokenStoredAsync()
        {
            try
            {
                var token = await SecureStorage.GetAsync("token");
                return !string.IsNullOrEmpty(token);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error retrieving token: {ex.Message}");
                return false;
            }
        }
        public async Task<string> GetTokenAsync()
        {
            return await SecureStorage.GetAsync("token");
        }

        public async Task<bool> IsLoggedInAsync()
        {
            var token = await GetTokenAsync();
            return !string.IsNullOrEmpty(token);
        }
    }
}
