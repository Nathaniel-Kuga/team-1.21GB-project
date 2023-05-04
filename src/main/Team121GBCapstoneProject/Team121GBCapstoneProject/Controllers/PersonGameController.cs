using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Team121GBCapstoneProject.Areas.Identity.Data;
using Team121GBCapstoneProject.DAL.Abstract;
using Team121GBCapstoneProject.DAL.Concrete;
using Team121GBCapstoneProject.Models;
using Team121GBCapstoneProjects.Controllers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Team121GBCapstoneProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonGameController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        IRepository<PersonGame> _personGameRepository;
        private IPersonRepository _personRepository;
        private IPersonListRepository _personListRepository;

        public PersonGameController(UserManager<ApplicationUser> userManager, 
            IRepository<PersonGame> personGameRepository, IPersonRepository personRepository, IPersonListRepository personListRepository)
        {
            _userManager = userManager;
            _personGameRepository = personGameRepository;
            _personRepository = personRepository;
            _personListRepository = personListRepository;
        }

        // DELETE api/<PersonGameController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            string authorizationId = _userManager.GetUserId(User);

            IQueryable<PersonList> userLists = _personListRepository.GetAll()
                .Join(_personRepository.GetAll(), pl => pl.PersonId, p => p.Id, (pl, p) => new { pl, p })
                .Where(x => x.p.AuthorizationId == authorizationId)
                .Select(x => x.pl);

            // gets all of the users games from every lists
            List<PersonGame> personGames = _personGameRepository.GetAll()
                .Where(pg => pg.PersonList.Person.AuthorizationId == authorizationId)
                .ToList();

            var selectGame = personGames.FirstOrDefault(g => g.Id == id);

           // var personGame = _personGameRepository.GetAll().FirstOrDefault(g => g.Id == id);

            if (selectGame == null)
            {
                Console.WriteLine("No game found");
            }
            else
            {
                _personGameRepository.Delete(selectGame);
            }
        }

        // GET: api/<PersonGameController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<PersonGameController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    Console.WriteLine("Get PersonGame...");
        //    return "value";
        //}

        // POST api/<PersonGameController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        // PUT api/<PersonGameController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

    }
}
