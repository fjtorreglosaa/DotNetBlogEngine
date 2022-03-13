using System.Collections.Generic;

namespace BlogEngine.Dto.Common
{
    public class ValidationResultDto
    {
        public List<ValidationConditionDto> Conditions { get; set; } = new List<ValidationConditionDto>();
    }
}
