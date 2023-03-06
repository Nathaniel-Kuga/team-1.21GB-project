namespace Team121GBCapstoneProject.Services
{
    public interface IDalleService
    {
        public Task<string> GetImages(string prompt);
    }
}
