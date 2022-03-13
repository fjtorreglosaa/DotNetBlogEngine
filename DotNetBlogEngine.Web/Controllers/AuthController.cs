using BlogEngine.App.Dto.Auth;
using BlogEngine.Business.AuthServices.Contracts;
using BlogEngine.Dto.Auth;
using BlogEngine.Dto.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BlogEngine.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserAuthService _userAuthService;

        public AuthController(IUserAuthService userAuthService)
        {
            _userAuthService = userAuthService;
        }

        /// <summary>
        /// Inicio de sesión
        /// </summary>
        /// <param name="criteria"></param>       
        /// <returns></returns>
        [HttpPost]
        [Route("signIn")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationResultDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationResultDto))]        
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SignIn([FromBody] LogInCriteriaDto criteria)
        {
            var result = await _userAuthService.SignIn(criteria);

            if (result.ValidationResultDto.Conditions.Any())
            {
                switch (result.ValidationResultDto.Conditions.First().Severity)
                {
                    case (int)HttpStatusCode.BadRequest:
                        return BadRequest(result.ValidationResultDto);
                    case (int)HttpStatusCode.NotFound:
                        return NotFound(result.ValidationResultDto);
                    case (int)HttpStatusCode.Forbidden:
                        return StatusCode((int)HttpStatusCode.Forbidden, result.ValidationResultDto);
                }
            }

            return Ok(result.User);
        }

        /// <summary>
        /// Cerrar sesión
        /// </summary>         
        /// <returns></returns>
        [HttpPost]
        [Route("signOut")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CerrarSesion()
        {
            _userAuthService.SignOut();                
            return Ok();
        }
    }
}
