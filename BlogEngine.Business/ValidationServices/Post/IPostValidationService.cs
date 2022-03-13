using BlogEngine.Dto.Common;
using BlogEngine.Dto.Post;

namespace BlogEngine.Business.ValidationServices.Post
{
    public interface IPostValidationService
    {
        ValidationResultDto ValidateForCreate(AddPostCriteriaDto criteria);
    }
}
