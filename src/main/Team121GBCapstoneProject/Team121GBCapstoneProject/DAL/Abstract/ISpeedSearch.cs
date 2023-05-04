using Team121GBCapstoneProject.Models;

namespace Team121GBCapstoneProject.DAL.Abstract
{
    public interface ISpeedSearch
    {
        public Task<IEnumerable<IgdbGame>> SpeedSearchingAsync(string input);
        public Task<IgdbGame> GetFirstSearchResultAsync(string title);
        public List<string> TitleParse(string input);
    }
}
