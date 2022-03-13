using BlogEngine.Data.Repositories.Interfaces;

namespace BlogEngine.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        IPostRepository Posts { get; }
        IAuthorRepository Authors { get; }
        int Complete();
    }
}
