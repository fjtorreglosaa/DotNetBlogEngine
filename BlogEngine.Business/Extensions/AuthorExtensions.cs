using BlogEngine.Data.Entities;
using BlogEngine.Dto.Author;

namespace BlogEngine.Business.Extensions
{
    public static class AuthorExtensions
    {
        public static Author Convert(this AddAuthorCriteriaDto criteria)
        {
            if(criteria == null)
            {
                return new Author();
            }

            return new Author
            {
                Name = criteria.Name
            };
        }

        public static AuthorDto Convert(this Author entity)
        {
            if(entity == null)
            {
                return new AuthorDto();
            }

            return new AuthorDto
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }
    }
}
