using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcheryProjectApp.Data
{
    public class ReposUser
    {
        public async Task<UserDetail> GetUserProfileAsync(string token)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                string url = "https://archeryapp-214121140527.us-central1.run.app/api/UserContoller/me";

                Console.WriteLine($"Token: {token}");
                Console.WriteLine($"Requesting URL: {url}");

                try
                {
                    var response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"JSON Response: {jsonResponse}");
                        var userDetail = JsonConvert.DeserializeObject<UserDetail>(jsonResponse);
                        return userDetail;
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode}");
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                    return null;
                }
            }
        }
    }
}
