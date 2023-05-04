using Team121GBCapstoneProject.Areas.Identity.Data;

namespace Team121GBCapstoneProject.Models
{
    public class FindFriendsVM
    {
        public ApplicationUser User { get; set; }
        public bool PersonNotFound { get; set; } = false;
        public List<PersonGame> Games { get; set; }
    }

}
