using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Talabat.APIs.Controllers
{
    [EnableCors("talabat")]
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
    }
}
