using Newtonsoft.Json;
using POSUNO.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace POSUNO.Helpers
{
    public class APIService
    {
        public static async Task<APIResponse> LoginAsync(LoginRequest loginModel)
        {
            try
            {
                string request = JsonConvert.SerializeObject(loginModel);
                StringContent content = new StringContent(request, Encoding.UTF8, "application/json");
                HttpClientHandler handler = new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };

                HttpClient client = new HttpClient(handler)
                {
                    BaseAddress = new Uri("https://localhost:44388/")
                };
                HttpResponseMessage response = await client.PostAsync("api/account/login", content);
                string result = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new APIResponse()
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                User user = JsonConvert.DeserializeObject<User>(result);
                return new APIResponse()
                {
                    IsSuccess = true,
                    Result = user,
                };
            }
            catch (Exception ex)
            {
                return new APIResponse()
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
    }
}
