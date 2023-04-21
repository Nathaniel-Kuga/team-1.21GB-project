using System.Diagnostics;
using Newtonsoft.Json.Linq;
using static System.Net.WebRequestMethods;
using Team121GBCapstoneProject.Services.Abstract;

namespace Team121GBCapstoneProject.Services.Concrete;

public class ReCaptchaV2Service : IReCaptchaService
{
    private readonly HttpClient _captchaClient;
    private string _secretKey;
    public ReCaptchaV2Service(string secretKey, HttpClient httpClient)
    {
        _secretKey = secretKey;
        _captchaClient = httpClient;
    }
    
    public async Task<bool> IsValid(string captcha)
    {
        try
        {
            var postTask = await _captchaClient.PostAsync($"?secret={_secretKey}&response={captcha}", new StringContent(""));
            var result = await postTask.Content.ReadAsStringAsync();
            var resultObject = JObject.Parse(result);
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
