using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TKMS.Abstraction.ComplexModels;
using TKMS.Abstraction.Enums;
using TKMS.Service.Interfaces;

namespace TKMS.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class IdentityController : ControllerBase
    {
        private readonly IUserService _userService;

        public IdentityController(
            IUserService userService
         )
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpPost("Authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate(LoginModel model)
        {
            model.RoleTypeId = RoleTypes.BRANCH.GetHashCode();
            var result = await _userService.AuthenticateUser(model);
            return Ok(result);
        }

        [HttpGet("Verify")]
        public string Verify()
        {
            return "Success";
        }
    }
}
