using Microsoft.AspNetCore.Mvc;

namespace Pagination.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        TestDBContext _context = new TestDBContext();
        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];


        [HttpGet]
        public IActionResult GetPagedData(int pageNumber = 1, int pageSize = 10)
        {
            var data = _context.DataSet
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(data);
        }

        public class PagedResult<T>
        {
            public int CurrentPage { get; set; }
            public int PageSize { get; set; }
            public int RowCount { get; set; }
            public int PageCount { get; set; }
            public List<T> Results { get; set; }
        }


        public PagedResult<T> GetPagedData<T>(IQueryable<T> query, int page, int pageSize) where T : class
        {
            var result = new PagedResult<T>
            {
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = query.Count()
            };
            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);
            var skip = (page - 1) * pageSize;
            result.Results = query.Skip(skip).Take(pageSize).ToList();
            return result;
        }


        public async Task<IActionResult> GetInfiniteScrollData(int lastItemId, int pageSize = 10)
        {
            var data = await _context.DataEntities
                .Where(d => d.Id > lastItemId)
                .OrderBy(d => d.Id)
                .Take(pageSize)
                .ToListAsync();

            return Ok(data);
        }




        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
