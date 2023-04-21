namespace Team121GBCapstoneProject.Models.DTO;

/// <summary>
/// GameDto class is for sending a game object to be added to a list
/// </summary>
public class GameDto
{
    public string GameTitle { get; set; }
    public string ImageSrc { get; set; }
    public string ListKind { get; set; }
    public int? IgdbID { get; set; }
}
