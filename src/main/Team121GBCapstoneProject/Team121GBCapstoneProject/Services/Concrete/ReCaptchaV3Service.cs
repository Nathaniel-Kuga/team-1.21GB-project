using System.Diagnostics;
using Azure.Core;
using Newtonsoft.Json.Linq;
using static System.Net.WebRequestMethods;
using Team121GBCapstoneProject.Services.Abstract;

namespace Team121GBCapstoneProject.Services.Concrete;

public class ReCaptchaV3Service : IReCaptchaV3Service
{
    private readonly string _secretKey;
    private readonly IHttpClientFactory _httpClientFactory;
    private string _url;
    public ReCaptchaV3Service(string secretKey, IHttpClientFactory httpClientFactory)
    {
        _secretKey = secretKey;
        _httpClientFactory = httpClientFactory;
    }
    
    public async Task<bool> IsValid(string captcha, string url)
    {
        try
        {
            _url = url + $"?secret={_secretKey}&response={captcha}";
            HttpClient httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(_url);
            HttpResponseMessage postTask = await httpClient.PostAsync(_url, new StringContent(""));
            string result = await postTask.Content.ReadAsStringAsync();
            JObject resultObject = JObject.Parse(result);
            dynamic success = resultObject["success"];
            return (bool)success;
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.ToString());
            return false;
        }
    }
}
