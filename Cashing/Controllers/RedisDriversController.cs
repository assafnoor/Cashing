using Cashing.Models;
using Cashing.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cashing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedisDriversController : ControllerBase
    {
        private readonly ILogger<RedisDriversController> _logger;
        private readonly ICacheServic _redisService;
        private readonly AppDbContext _dbContext;

        public RedisDriversController(ILogger<RedisDriversController> logger,ICacheServic rediscahce,AppDbContext dbContext)
        {
            _logger = logger;
            _redisService = rediscahce;
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
           var redis = _redisService.GetData<IEnumerable<Driver>>("drivers");
            if (redis != null && redis.Count()>0)
            {
                return Ok(redis);
            }
            redis = await _dbContext.Drivers.ToListAsync();
            var expirytime = DateTimeOffset.Now.AddSeconds(30);
            _redisService.SetData<IEnumerable<Driver>>("drivers",redis,expirytime);
            return Ok(redis);
        }
        [HttpPost]
        public async Task<IActionResult> Add(Driver driver)
        {
            var data = await _dbContext.Drivers.AddAsync(driver);
            var expirytime = DateTimeOffset.Now.AddSeconds(30);
            _redisService.SetData<Driver>($"driver{driver.Id}",data.Entity, expirytime);
            await _dbContext.SaveChangesAsync();
            return Ok(data.Entity);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _dbContext.Drivers.FirstOrDefaultAsync(x => x.Id == id);
            if(data is not null)
            {
                _dbContext.Drivers.Remove(data);
                _redisService.RemoveData($"driver{id}");
                await _dbContext.SaveChangesAsync();
                return NoContent();
            }
            return NotFound();
        }
    }
}
