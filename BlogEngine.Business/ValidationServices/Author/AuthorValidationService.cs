using BlogEngine.Data.UnitOfWork;
using BlogEngine.Dto.Author;
using BlogEngine.Dto.Common;
using System.Linq;
using System.Net;

namespace BlogEngine.Business.ValidationServices.Author
{
    public class AuthorValidationService : IAuthorValidationService
    {
        private IUnitOfWork _unitOfWork { get; set; }

        public AuthorValidationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ValidationResultDto ValidateForCreate(AddAuthorCriteriaDto criteria)
        {
              var validationResult = new ValidationResultDto();

            var nameConditions = ValidateName(criteria).Conditions;

            validationResult.Conditions.AddRange(nameConditions);

            if (validationResult.Conditions.Any())
            {
                return validationResult;
            }

            var existingNameConditions = ValidateExistingName(criteria).Conditions;

            validationResult.Conditions.AddRange(existingNameConditions);        

            return validationResult;
        }
        
        public ValidationResultDto ValidateName(AddAuthorCriteriaDto criteria) 
        {
            var validationResult = new ValidationResultDto();

            if (string.IsNullOrEmpty(criteria.Name))
            {
                validationResult.Conditions.Add(new ValidationConditionDto
                {
                    ErrorMessage = "The author name must be included",
                    Severity = (int)HttpStatusCode.BadRequest
                });
            }

            return validationResult;
        }

        public ValidationResultDto ValidateExistingName(AddAuthorCriteriaDto criteria)
        {
            var validationResult = new ValidationResultDto();

            var author = _unitOfWork.Authors.GetAll().Where(a => a.Name == criteria.Name).FirstOrDefault();

            if (author != null)
            {
                validationResult.Conditions.Add(new ValidationConditionDto
                {
                    ErrorMessage = "The author alredy exists",
                    Severity = (int)HttpStatusCode.BadRequest
                });
            }

            return validationResult;
        }
    }
}
