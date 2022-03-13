using BlogEngine.Business.DomainServices.Interfaces;
using BlogEngine.Dto.Author;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace BlogEngine.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorDomainService _authorDomainService;
        public AuthorController(IAuthorDomainService authorDomainService)
        {
            _authorDomainService = authorDomainService;
        }

        [HttpPost]
        //[Route("Add")]
        public async Task<IActionResult> AddAuthorAsync(AddAuthorCriteriaDto criteria)
        {
            var result = await _authorDomainService.Add(criteria);

            if (result.ValidationResult.Conditions.Any())
            {
                return BadRequest(result.ValidationResult);
            }
            return Ok(result.Author);
        }
    }
}
