using Microsoft.AspNetCore.Identity;

namespace Team121GBCapstoneProject.Areas.Identity.Data;

public class ApplicationUser : IdentityUser
{
    [PersonalData]
    public string FirstName { get; set; }
    [PersonalData]
    public string LastName { get; set; }
    [PersonalData]
    public byte[] ProfilePicture { get; set; }
    [PersonalData]
    public string ProfileBio { get; set; }
}