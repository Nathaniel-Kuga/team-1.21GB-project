namespace Team121GBCapstoneProject.Services.Abstract;
/// <summary>
/// Interface for reCAPTCHA V2 
/// </summary>
/// <typeparam name="captcha">This is response received back from google.</typeparam>
public interface IReCaptchaService
{
    Task<bool> IsValid(string captcha);
}
