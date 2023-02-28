namespace Team121GBCapstoneProject.Services;

public interface IReCaptchaService
{
    Task<bool> IsValid(string captcha);
}
