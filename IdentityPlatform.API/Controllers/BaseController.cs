using IdentityPlatform.Core.Common;
using Microsoft.AspNetCore.Mvc;

namespace IdentityPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected IActionResult CreateActionResult<T>(Result<T> result)
        {
            return result.StatusCode switch
            {
                200 => Ok(result),
                201 => Created("", result),
                401 => Unauthorized(result),
                403 => Forbid(),
                404 => NotFound(result),
                _ => BadRequest(result)
            };
        }
    }
}
