namespace Team121GBCapstoneProject.Models;

public class PersonListVM
{
    public string ListKind { get; set; }
    public List<PersonGame> PersonGames { get; set; }
    public List<Game> CurratedList { get; set; } = null;
    public PersonListVM(string listKind, List<PersonGame> games)
    {
        ListKind = listKind;
        PersonGames = new List<PersonGame>();
        games.ForEach(g => PersonGames.Add(g));
    }

    //Leave Helpful Comment later
    public PersonListVM(List<Game> curratedList)
    {
        CurratedList = curratedList;
    }
}