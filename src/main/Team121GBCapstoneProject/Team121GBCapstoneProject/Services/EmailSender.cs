using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using Team121GBCapstoneProject.Services;

namespace Team121GBCapstoneProject.Services;

public class EmailSender : IEmailSender
{
    private readonly ILogger _logger;

    public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor,
                       ILogger<EmailSender> logger)
    {
        Options = optionsAccessor.Value;
        _logger = logger;
    }

    public AuthMessageSenderOptions Options { get; } //Set with Secret Manager.

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        if (string.IsNullOrEmpty(Options.SendGridKey))
        {
            throw new Exception("Null SendGridKey");
        }
        await Execute(Options.SendGridKey, subject, message, toEmail);
    }

    public async Task Execute(string apiKey, string subject, string message, string toEmail)
    {
        var newmessage = "<h1 style=\"text-align: inherit; font-family: inherit\"><span style=\"font-family: &quot;arial black&quot;, helvetica, sans-serif; font-size: 40px; color: #d89816\">Welcome to the Ultimate Gaming Hub!</span></h1>"
               + "<div style=\"font-family: inherit; text-align: inherit\"><span style=\"font-family: &quot;times new roman&quot;, times, serif; font-size: 18px\">Thank you for creating a account.</span></div>"
               + message;
        //subject = "Welcome to Gaming Platform!: Please Confirm your email";

        var client = new SendGridClient(apiKey);
        var msg = new SendGridMessage()
        {
            From = new EmailAddress("team121gb@gmail.com", "Account Confirmation"),
            Subject = subject,
            PlainTextContent = message,
            HtmlContent = message
        };
        msg.AddTo(new EmailAddress(toEmail));

        // Disable click tracking.
        // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
        msg.SetClickTracking(false, false);
        var response = await client.SendEmailAsync(msg);
        var resposeBody = await response.Body.ReadAsStringAsync();
        _logger.LogInformation(response.IsSuccessStatusCode
                               ? $"Email to {toEmail} queued successfully!"
                               : $"Failure Email to {toEmail}");
    }
}