using Microsoft.AspNetCore.Mvc;

namespace AgenticRAG.REACT.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(new[]
            {
            new { Id = 1, Name = "John" },
            new { Id = 2, Name = "Sara" }
        });
        }
    }
}
