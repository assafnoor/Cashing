using Cashing.Models;
using Cashing.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cashing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriversController : ControllerBase
    {
        private readonly ICacheServic _cacheServic;
        private readonly AppDbContext _dbContext;

        public DriversController(ICacheServic cacheServic, AppDbContext dbContext)
        {
            _cacheServic = cacheServic;
            _dbContext = dbContext;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var cashdriver = _cacheServic.GetData<IEnumerable<Driver>>("drivers");
            if (cashdriver != null && cashdriver.Count() > 0)
            {
                return Ok(cashdriver);
            }
            var driver = await _dbContext.Drivers.ToListAsync();
            var expiryTime = DateTimeOffset.Now.AddMinutes(2);
            _cacheServic.SetData<IEnumerable<Driver>>("drivers", driver, expiryTime);
            return  Ok(driver);
        }
        [HttpPost]
        public async Task<IActionResult> post(Driver driver)
        {
            await _dbContext.Drivers.AddAsync(driver);
           await _dbContext.SaveChangesAsync();
            return Ok(driver);
        }
    }
}
