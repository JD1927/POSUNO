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
                string url = Settings.GetApiUrl();
                HttpClient client = new HttpClient(handler)
                {
                    BaseAddress = new Uri(url)
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

        public static async Task<APIResponse> GetListAsync<T>(string controller)
        {
            try
            {
                HttpClientHandler handler = new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
                string url = Settings.GetApiUrl();
                HttpClient client = new HttpClient(handler)
                {
                    BaseAddress = new Uri(url)
                };
                HttpResponseMessage response = await client.GetAsync($"api/{controller}");
                string result = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new APIResponse()
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                List<T> list = JsonConvert.DeserializeObject<List<T>>(result);
                return new APIResponse()
                {
                    IsSuccess = true,
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new APIResponse()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public static async Task<APIResponse> PostAsync<T>(string controller, T model)
        {
            try
            {
                string request = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(request, Encoding.UTF8, "application/json");

                HttpClientHandler handler = new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
                string url = Settings.GetApiUrl();
                HttpClient client = new HttpClient(handler)
                {
                    BaseAddress = new Uri(url)
                };
                HttpResponseMessage response = await client.PostAsync($"api/{controller}", content);
                string result = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new APIResponse()
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                T item = JsonConvert.DeserializeObject<T>(result);
                return new APIResponse()
                {
                    IsSuccess = true,
                    Result = item,
                };
            }
            catch (Exception ex)
            {
                return new APIResponse()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public static async Task<APIResponse> PutAsync<T>(string controller, T model, int id)
        {
            try
            {
                string request = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(request, Encoding.UTF8, "application/json");

                HttpClientHandler handler = new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
                string url = Settings.GetApiUrl();
                HttpClient client = new HttpClient(handler)
                {
                    BaseAddress = new Uri(url)
                };
                HttpResponseMessage response = await client.PutAsync($"api/{controller}/{id}", content);
                string result = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new APIResponse()
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                T item = JsonConvert.DeserializeObject<T>(result);
                return new APIResponse()
                {
                    IsSuccess = true,
                    Result = item,
                };
            }
            catch (Exception ex)
            {
                return new APIResponse()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public static async Task<APIResponse> DeleteAsync(string controller, int id)
        {
            try
            {
                HttpClientHandler handler = new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };
                string url = Settings.GetApiUrl();
                HttpClient client = new HttpClient(handler)
                {
                    BaseAddress = new Uri(url)
                };
                HttpResponseMessage response = await client.DeleteAsync($"api/{controller}/{id}");
                string result = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new APIResponse()
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                return new APIResponse()
                {
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                return new APIResponse()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }
}