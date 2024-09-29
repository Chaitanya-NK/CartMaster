using CartMaster.Business.IServices;
using CartMaster.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Text;
#pragma warning disable CS1591

namespace CartMaster.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardCountsController : ControllerBase
    {
        private readonly IDashboardCountsService _dashboardCountsService;
        public DashboardCountsController(IDashboardCountsService dashboardCountsService)
        {
            _dashboardCountsService = dashboardCountsService;
        }

        [HttpGet("GetDashboardData")]
        public async Task<IActionResult> GetDashboardData()
        {
            try
            {
                var dashboardData = await _dashboardCountsService.GetDashboardDataAsync();
                if(dashboardData == null)
                {
                    return NotFound("Dashboard Data Not Found");
                }
                return Ok(new { success = true, dashboardData });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
