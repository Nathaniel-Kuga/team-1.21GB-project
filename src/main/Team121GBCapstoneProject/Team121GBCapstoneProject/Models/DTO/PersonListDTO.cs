namespace Team121GBCapstoneProject.Models.DTO;

public class PersonListDTO
{
    public string ListKind { get; set; }
    public PersonListDTO()
    {}
    public PersonListDTO(string listKind)
    {
        ListKind = listKind;
    }
}
