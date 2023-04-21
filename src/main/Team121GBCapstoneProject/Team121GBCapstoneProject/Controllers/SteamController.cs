    using Microsoft.AspNetCore.Mvc;
    using Team121GBCapstoneProject.Models;
    using Team121GBCapstoneProject.Services.Abstract;

    namespace Team121GBCapstoneProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SteamController : ControllerBase
    {
        private readonly IsteamService _steamService;

        public SteamController(IsteamService steamService)
        {
            _steamService = steamService;
        }

        [HttpGet("GetSteamUser")]
        public ActionResult<SteamUser> GetSteamUser(string id)
        {
            return Ok(_steamService.GetSteamUser(id));
        }
    }
}
