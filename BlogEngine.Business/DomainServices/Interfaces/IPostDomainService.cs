using BlogEngine.Dto.Common;
using BlogEngine.Dto.Post;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogEngine.Business.DomainServices.Interfaces
{
    public interface IPostDomainService
    {
        Task<(PostDto Post, ValidationResultDto ValidationResult)> Add(AddPostCriteriaDto criteria);
        Task<List<PostDto>> Get();
    }
}
