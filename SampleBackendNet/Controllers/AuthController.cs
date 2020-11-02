using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleBackendNet.Helpers;
using SampleBackendNet.Models;
using SampleBackendNet.Services;

namespace SampleBackendNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IVirgilService _virgilService;

        public AuthController(IAuthService authService, IVirgilService virgilService)
        {
            _authService = authService;
            _virgilService = virgilService;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthRequest model)
        {
            var response = _authService.Authenticate(model, HttpContext);

            if (response == null)
                return BadRequest(new { message = "Identity or password is incorrect" });

            return Ok(response);
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _authService.GetAll();
            return Ok(users);
        }

        [Authorize]
        [HttpGet]
        [Route("virgil-jwt")]
        public IActionResult GetVirgilToken()
        {
            var currentIdentity = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var token = _virgilService.GenerateVirgilJwt(currentIdentity);
            return Ok(token);
        }
    }
}
