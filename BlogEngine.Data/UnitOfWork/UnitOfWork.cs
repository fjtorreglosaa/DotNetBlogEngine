using BlogEngine.Data.Context;
using BlogEngine.Data.Repositories;
using BlogEngine.Data.Repositories.Interfaces;

namespace BlogEngine.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BlogEngineDbContext _context;

        public UnitOfWork(BlogEngineDbContext context)
        {
            _context = context;
            Posts = new PostRepository(_context);
            Authors = new AuthorRepository(_context);
            Comments = new CommentRepository(_context);
        }

        public IPostRepository Posts { get; private set; }
        public IAuthorRepository Authors { get; private set; }
        public ICommentRepository Comments { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
