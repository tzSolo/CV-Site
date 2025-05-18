using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GitHubController(GitHubService gitHubService, IConfiguration configuration) : ControllerBase
    {
        private readonly GitHubService _gitHubService = gitHubService;
        private readonly IConfiguration _configuration = configuration;
        // GET: api/<GitHubController>
        [HttpGet("GetAllRepositories")]
        public IEnumerable<string> GetPortfolioAsync()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/<GitHubController>
        [HttpGet("SearchBy")]
        public IEnumerable<string> SearchRepositories()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
