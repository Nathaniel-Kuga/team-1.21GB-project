namespace Team121GBCapstoneProject.Services.Abstract
{
    public interface IChatGptService
    {
        public Task<string> GetChatResponse(string prompt);
    }
}