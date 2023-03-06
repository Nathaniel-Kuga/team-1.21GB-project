namespace Team121GBCapstoneProject.Models.DTO;

public class GameAndListDTO
{
    public string ListName { get; set; }
    public string GameTitle { get; set; }
    public GameAndListDTO(string gameTitle, string listName)
    {
        GameTitle= gameTitle;
        ListName = listName;
    }
}