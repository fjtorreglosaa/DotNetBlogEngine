using BlogEngine.Business.DomainServices.Interfaces;
using BlogEngine.Business.Extensions;
using BlogEngine.Business.ValidationServices.Post;
using BlogEngine.Data.Identity;
using BlogEngine.Data.UnitOfWork;
using BlogEngine.Dto.Common;
using BlogEngine.Dto.Post;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogEngine.Business.DomainServices
{
    public class PostDomainService : IPostDomainService
    {
        private IUnitOfWork _unitOfWork;
        private readonly IPostValidationService _postValidationService;

        public PostDomainService(IUnitOfWork unitOfWork, IPostValidationService postValidationService)
        {
            _postValidationService = postValidationService;
            _unitOfWork = unitOfWork;

        }

        public async Task<(PostDto Post, ValidationResultDto ValidationResult)> Add(AddPostCriteriaDto criteria)
        {
            var validationResult = _postValidationService.ValidateForCreate(criteria);

            if (validationResult.Conditions.Any())
            {
                return (null, validationResult);
            }

            var entity = criteria.Convert();

            await _unitOfWork.Posts.AddAsync(entity);

            _unitOfWork.Complete();

            var dto = entity.Convert();

            return (dto, validationResult);
        }

        public async Task<List<PostDto>> Get()
        {
            var entities = await _unitOfWork.Posts.GetAllAsync();

            var dtos = entities.Select(p => p.Convert()).ToList();

            return dtos;
        }

        
       
    }
}
