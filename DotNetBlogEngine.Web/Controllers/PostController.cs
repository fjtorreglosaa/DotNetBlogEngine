using BlogEngine.Business.AuthServices.Contracts;
using BlogEngine.Business.DomainServices.Interfaces;
using BlogEngine.Dto.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetBlogEngine.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
      
        private readonly IPostDomainService _postDomainService;
        private readonly IUserAuthService  _userAuthService;


        public PostController(IPostDomainService postDomainService, IUserAuthService userAuthService)
        {
            _postDomainService = postDomainService;
            _userAuthService = userAuthService;
        }

        [HttpPost]
        [Authorize(Roles = "EDITOR")]
        [Authorize(Roles = "PUBLIC")]
        public async Task<IActionResult> AddPostAsync(AddPostCriteriaDto criteria)
        {
            var result = await _postDomainService.Add(criteria);

            if (result.ValidationResult.Conditions.Any())
            {
                return BadRequest(result.ValidationResult);
            }
            return Ok(result.Post);
        }

        [HttpGet]
        [Authorize(Roles = "WRITER")]
        public async Task<IActionResult> GetPostAsync()
        {
            var currentUser = await _userAuthService.GetcurrentUser(HttpContext.User);                    

            var posts = await _postDomainService.Get();

            return Ok(posts);
        }        

    }
}
