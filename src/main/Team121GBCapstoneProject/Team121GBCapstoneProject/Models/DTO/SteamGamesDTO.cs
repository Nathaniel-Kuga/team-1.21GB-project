namespace Team121GBCapstoneProject.Models.DTO;
//Cite Justin's team on this work
public class SteamGamesDTO
{
    public LibraryResponse? response { get; set; }
}
public class Games
{
    public int appid { get; set; }
    public string? name { get; set; }
    public int? playtime_forever { get; set; }
    public string? img_icon_url { get; set; }
    public int? rtime_last_played { get; set; }
}

public class LibraryResponse
{
    public List<Games>? games { get; set; }
}