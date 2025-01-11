using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CodeChallenge.Services;
using CodeChallenge.Models;

namespace CodeChallenge.Controllers
{
    // Compensation Controller
    [ApiController]
    [Route("api/compensation")]
    public class CompensationController : ControllerBase
    {
        private readonly ICompensationService _compService;
        private readonly ILogger<CompensationController> _logger;

        public CompensationController(
            ICompensationService compService,
            ILogger<CompensationController> logger)
        {
            _compService = compService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompensation([FromBody] Compensation compensation)
        {
            _logger.LogDebug("Received compensation create request.");
            var created = _compService.Create(compensation);

            // Then persist to DB (JSON) if your pattern requires an immediate save:
            await _compService.SaveAsync();

            // Return a 201 Created with a reference to a GET endpoint
            return CreatedAtRoute(
                "getCompensationByEmployeeId",
                new { employeeId = compensation.EmployeeId },
                created
            );
        }

        [HttpGet("{employeeId}", Name = "getCompensationByEmployeeId")]
        public IActionResult GetCompensationByEmployeeId(string employeeId)
        {
            _logger.LogDebug($"Received compensation get request for employeeId '{employeeId}'");

            // Retrieve from the service
            var comp = _compService.GetByEmployeeId(employeeId);
            if (comp == null)
            {
                return NotFound();
            }

            return Ok(comp);
        }
    }

}