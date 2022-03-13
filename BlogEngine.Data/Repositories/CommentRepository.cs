using BlogEngine.Data.Context;
using BlogEngine.Data.Entities;
using BlogEngine.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogEngine.Data.Repositories
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(BlogEngineDbContext context) : base(context)
        {

        }
    }
}
