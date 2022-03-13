using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BlogEngine.Business.AuthServices.Contracts;
using BlogEngine.Dto.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogEngine.Api.Controllers
{
    [ApiController]
    [Route("api/User")]
    public class UserController : ControllerBase
    {
        private readonly IUserAuthService _userAuthService;

        public UserController(IUserAuthService userAuthService)
        {
            _userAuthService = userAuthService;
        }

        /// <summary>
        /// Crear Usuario
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateUserCriteriaDto criteria)
        {
            var result = await _userAuthService.Create(criteria);

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

            return Created("api/User", result.User);
        }

        /// <summary>
        /// Obtener todos los usuarios
        /// </summary>        
        /// <returns></returns>
        [HttpGet]
        
        public IActionResult Get()
        {
            var result = _userAuthService.Get();

            return Ok(result);
        }

        /// <summary>
        /// Buscar un usuario por email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetByEmail")]
        public async Task<IActionResult> GetByEmail([FromBody] string email)
        {
            var result = await _userAuthService.GetByEmail(email);

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

            return Ok(result.User);
        }

        /// <summary>
        /// Buscar un usuario por UserName
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetByUserName")]
        public async Task<IActionResult> GetByUserName([FromBody] string userName)
        {
            var result = await _userAuthService.GetByUserName(userName);

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

            return Ok(result.User);
        }

        /// <summary>
        /// Adicionar un rol al usuario
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="rol"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddRol")]
        public async Task<IActionResult> AddRole(string userName, string rol)
        {
            var result = await _userAuthService.AddRole(userName, rol);

            if (result.ValidationResultDto.Conditions.Any())
            {
                switch (result.ValidationResultDto.Conditions.First().Severity)
                {
                    case (int)HttpStatusCode.BadRequest:
                        return BadRequest(result.ValidationResultDto);
                    case (int)HttpStatusCode.NotFound:
                        return NotFound(result.ValidationResultDto);
                    case (int)HttpStatusCode.InternalServerError:
                        return StatusCode((int)HttpStatusCode.InternalServerError, result.ValidationResultDto);
                }
            }

            return Ok(result.User);
        }

        /// <summary>
        /// Quitar un rol del usuario
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="rol"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("RemoveRol")]
        public async Task<IActionResult> RemoveRol([FromBody] string userName, string rol)
        {
            var result = await _userAuthService.RemoveRole(userName, rol);

            if (result.ValidationResultDto.Conditions.Any())
            {
                switch (result.ValidationResultDto.Conditions.First().Severity)
                {
                    case (int)HttpStatusCode.BadRequest:
                        return BadRequest(result.ValidationResultDto);
                    case (int)HttpStatusCode.NotFound:
                        return NotFound(result.ValidationResultDto);
                    case (int)HttpStatusCode.InternalServerError:
                        return StatusCode((int)HttpStatusCode.InternalServerError, result.ValidationResultDto);
                }
            }

            return Ok(result.User);
        }

    }
}
