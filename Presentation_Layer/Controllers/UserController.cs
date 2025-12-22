using Application_Layer;
using Application_Layer.DTO;
using Application_Layer.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Layer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthService authService;

        public UserController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost]
        [Route("login")]

        public async Task<IActionResult> LoginAsync([FromBody]LoginRequest loginRequest)
        {
            var response = await authService.LoginAsync(loginRequest);
            return Ok(response);
        }

        [HttpPost]
        [Route("register")]

        public async Task<IActionResult> RegisterAsync([FromBody]RegisterRequest registerRequest)
        {
            var response = await authService.RegisterAsync(registerRequest);
            return Ok(response);
        }

        [HttpPost("google")]
        public async Task<ActionResult<AuthResponse>> GoogleLogin([FromBody] GoogleLoginRequest request)
        {
            var response = await authService.GoogleLoginAsync(request.IdToken);
            return Ok(response);
        }
    }
}
