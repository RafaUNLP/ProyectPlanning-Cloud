using System.Net.Http.Headers;
using backend.DTOs;

namespace ApiACEAPP.Helpers;

public static class HttpClientHelper
{
    private static HttpClient _httpClient = new HttpClient();
    
    public static async Task<RespuestaHttp> SendRequest(string url, string method, object body, string? token)
    {
        try
        {
            var result = new HttpResponseMessage();
            
            using (_httpClient = new HttpClient())
            {
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                if (token != null)
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                JsonContent? json;

                switch (method)
                {
                    case "GET":
                        result = await _httpClient.GetAsync(url);
                        break;
                    case "POST":
                        json = JsonContent.Create(body);
                        result = await _httpClient.PostAsync(url, json);
                        break;
                    case "PUT":
                        json = JsonContent.Create(body);
                        result = await _httpClient.PutAsync(url, json);
                        break;
                }
                
                RespuestaHttp response = new RespuestaHttp(){
                    Contenido = await result.Content.ReadAsStringAsync(),
                    Status = result.StatusCode
                };

                return response;
            }   
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}