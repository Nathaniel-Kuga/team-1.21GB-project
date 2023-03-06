namespace Team121GBCapstoneProject.Models.DTO;

public class PersonGameListDTO
{
    public string ListName { get; set; }
   
    public PersonGameListDTO(string listName)
    {
        ListName = listName;
    }
}