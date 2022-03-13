using BlogEngine.Data.Context;
using BlogEngine.Data.Entities;
using BlogEngine.Data.Repositories.Interfaces;

namespace BlogEngine.Data.Repositories
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        public PostRepository(BlogEngineDbContext context) : base(context)
        {
        }
    }
}
