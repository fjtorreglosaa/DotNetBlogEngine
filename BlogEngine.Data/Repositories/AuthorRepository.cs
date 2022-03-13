using BlogEngine.Data.Context;
using BlogEngine.Data.Entities;
using BlogEngine.Data.Repositories.Interfaces;

namespace BlogEngine.Data.Repositories
{
    public class AuthorRepository : GenericRepository<Author>, IAuthorRepository
    {
        public AuthorRepository(BlogEngineDbContext context) : base(context)
        {

        }
    }
}
