using BlogEngine.Dto.Author;
using BlogEngine.Dto.Common;

namespace BlogEngine.Business.ValidationServices.Author
{
    public interface IAuthorValidationService
    {
        ValidationResultDto ValidateForCreate(AddAuthorCriteriaDto criteria);
    }
}
