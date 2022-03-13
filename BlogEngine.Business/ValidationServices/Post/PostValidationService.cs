using BlogEngine.Data.UnitOfWork;
using BlogEngine.Dto.Common;
using BlogEngine.Dto.Post;
using System.Net;

namespace BlogEngine.Business.ValidationServices.Post
{
    public class PostValidationService : IPostValidationService
    {
        private IUnitOfWork _unitOfWork { get; set; }

        public PostValidationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ValidationResultDto ValidateForCreate(AddPostCriteriaDto criteria)
        {
            var validationResult = new ValidationResultDto();

            if (string.IsNullOrEmpty(criteria.PostContent))
            {
                validationResult.Conditions.Add(new ValidationConditionDto
                {
                    ErrorMessage = "The post content field is mandatory",
                    Severity = (int)HttpStatusCode.BadRequest
                });

                return validationResult;
            }

            return validationResult;
        }

    }
}
