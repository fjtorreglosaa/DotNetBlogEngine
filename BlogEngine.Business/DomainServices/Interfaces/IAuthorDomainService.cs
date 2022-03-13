using BlogEngine.Dto.Author;
using BlogEngine.Dto.Common;
using System.Threading.Tasks;

namespace BlogEngine.Business.DomainServices.Interfaces
{
    public interface IAuthorDomainService
    {
        Task<(AuthorDto Author, ValidationResultDto ValidationResult)> Add(AddAuthorCriteriaDto criteria);

    }
}
