using BlogEngine.Business.AuthServices.Contracts;
using BlogEngine.Dto.Auth;
using BlogEngine.Dto.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BlogEngine.Api.Controllers
{
    [ApiController]
    [Route("api/Rol")]
    public class RolController: ControllerBase
    {
        private readonly IRoleAuthService _roleAuthService;

        public RolController(IRoleAuthService roleAuthService)
        {
            _roleAuthService = roleAuthService;
        }

        /// <summary>
        /// Crear Rol
        /// </summary>
        /// <param name="rol"></param>
        /// <returns></returns>
        [HttpPost] 
        public async Task<IActionResult> CreateAsync([FromQuery] string rol)
        {            
            var result = await _roleAuthService.Create(rol);

            if (result.ValidationResultDto.Conditions.Any())
            {
                switch (result.ValidationResultDto.Conditions.First().Severity)
                {
                    case (int)HttpStatusCode.BadRequest:
                        return BadRequest(result.ValidationResultDto);
                    case (int)HttpStatusCode.InternalServerError:
                        return StatusCode((int)HttpStatusCode.InternalServerError, result.ValidationResultDto);
                }                
            }

            return Created( "api/rol", result.Role);
        }

        /// <summary>
        /// Obtener todos los Roles
        /// </summary>        
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<RoleDto>))]                
        public IActionResult GetByNameAsync()
        {
            var result = _roleAuthService.Get();                       

            return Ok(result);
        }

        /// <summary>
        /// Obtener un Rol
        /// </summary>
        /// <param name="rol"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetRoleByName", Name = "GetRoleByName")]      
        public async Task<IActionResult> GetByNameAsync([FromQuery] string rol)
        {
            var result = await _roleAuthService.GetByName(rol);
                        
            if (result.ValidationResultDto.Conditions.Any())
            {
                switch (result.ValidationResultDto.Conditions.First().Severity)
                {
                    case (int)HttpStatusCode.BadRequest:
                        return BadRequest(result.ValidationResultDto);
                    case (int)HttpStatusCode.NotFound:
                        return NotFound(result.ValidationResultDto);
                }
            }

            return Ok(result.Role);
        }
    }
}
