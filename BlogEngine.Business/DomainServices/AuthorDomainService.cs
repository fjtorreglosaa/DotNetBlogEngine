using BlogEngine.Business.DomainServices.Interfaces;
using BlogEngine.Business.Extensions;
using BlogEngine.Business.ValidationServices.Author;
using BlogEngine.Data.UnitOfWork;
using BlogEngine.Dto.Author;
using BlogEngine.Dto.Common;
using System.Linq;
using System.Threading.Tasks;

namespace BlogEngine.Business.DomainServices
{
    public class AuthorDomainService : IAuthorDomainService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthorValidationService _authorValidationService;
        public AuthorDomainService(IUnitOfWork unitOfWork, IAuthorValidationService authorValidationService)
        {
            _unitOfWork = unitOfWork;
            _authorValidationService = authorValidationService;
        }

        public async Task<(AuthorDto Author, ValidationResultDto ValidationResult)> Add(AddAuthorCriteriaDto criteria)
        {
            var validationResult = _authorValidationService.ValidateForCreate(criteria);

            if (validationResult.Conditions.Any())
            {
                return (null, validationResult);
            }

            var entity = criteria.Convert();

            await _unitOfWork.Authors.AddAsync(entity);

            _unitOfWork.Complete();

            var dto = entity.Convert();

            return (dto, validationResult);
        }
    }
}
