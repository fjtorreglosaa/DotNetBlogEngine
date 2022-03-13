using BlogEngine.Data.Entities;
using BlogEngine.Dto.Post;
using System;

namespace BlogEngine.Business.Extensions
{
    public static class PostExtensions
    {
        public static Post Convert(this AddPostCriteriaDto criteria)
        {
            if (criteria == null)
            {
                return new Post();
            }

            return new Post
            {
                PostContent = criteria.PostContent,
                DatePublished = DateTime.Now,
                AuthorId = criteria.AuthorId
            };
        }

        public static PostDto Convert(this Post entity)
        {
            if (entity == null)
            {
                return new PostDto();
            }

            return new PostDto
            {
                Id = entity.Id,
                PostContent = entity.PostContent,
                AuthorId = entity.AuthorId
            };
        }
    }
}
