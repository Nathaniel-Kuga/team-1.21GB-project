namespace Team121GBCapstoneProject.Services.Abstract;

public interface IReCaptchaV3Service
{
    public Task<bool> IsValid(string captcha, string url);
}
