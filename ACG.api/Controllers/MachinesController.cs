using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ACG.api.Controllers
{
    [ApiController]
    [Route("/machines")]
    public class MachinesController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public MachinesController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet("import/{user}/producer")]
        public async Task<IActionResult> ImportMachines(string producer, string user)
        {
            
            return NotFound();
        }

        [HttpGet("{machineId}")]
        public async Task<IActionResult> GetMachine(string machineId)
        {
           
            return BadRequest();
        }

    }
}

