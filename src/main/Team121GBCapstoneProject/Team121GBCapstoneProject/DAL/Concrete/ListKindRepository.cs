using Team121GBCapstoneProject.DAL.Abstract;
using Team121GBCapstoneProject.Models;

namespace Team121GBCapstoneProject.DAL.Concrete;

public class ListKindRepository : Repository<ListKind>, IListKindRepository
{
    public ListKindRepository(GPDbContext ctx) : base(ctx)
    { }
    
}
