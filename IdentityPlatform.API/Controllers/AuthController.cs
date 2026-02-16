using IdentityPlatform.Application.Dtos;
using IdentityPlatform.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IdentityPlatform.API.Controllers
{

    [Route("api/[controller]")]
    public class AuthController : BaseController
    {

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// 1. The user login and receives an Authorization Code.
        /// </summary>
        [HttpPost("authorize")]
        public async Task<IActionResult> Authorize([FromBody] LoginRequestDto request)
        {
            var result = await _authService.GenerateAuthorizationCodeAsync(request);
            return CreateActionResult(result);
        }

        /// <summary>
        /// 2. Tokens (Access, ID, Refresh) are obtained through the received code.
        /// </summary>
        [HttpPost("token")]
        public async Task<IActionResult> Token([FromBody] TokenRequestDto request)
        {
            var result = await _authService.ExchangeCodeForTokensAsync(request);
            return CreateActionResult(result);
        }

        /// <summary>
        /// 3. When the Access Token expires, it is renewed with a Refresh Token.
        /// </summary>
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto request)
        {
            var result = await _authService.RefreshAsync(request);
            return CreateActionResult(result);
        }


        [HttpGet("validate-client")]
        public async Task<IActionResult> ValidateClient(string clientId, string redirectUri)
        {
            var result = await _authService.ValidateClientAsync(clientId, redirectUri);
            return CreateActionResult(result);
        }
    }
}
