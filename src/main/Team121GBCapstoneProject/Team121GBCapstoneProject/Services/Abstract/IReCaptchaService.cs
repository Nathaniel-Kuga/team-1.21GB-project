namespace Team121GBCapstoneProject.Services.Abstract;

public interface IReCaptchaService
{
    Task<bool> IsValid(string captcha);
}
