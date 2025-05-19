using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Octokit;
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
        [HttpGet("AllRepos")]
        public async Task<PortfolioDetails> GetPortfolioAsync()
        {
            var allUserRepositories = await _gitHubService.GetUserRepositories(_configuration["MyUserName"]);

            return allUserRepositories;
        }

        // GET: api/<GitHubController>
        [HttpGet("SearchBy")]
        public async Task<IEnumerable<Repository>> SearchRepositories(string? repositoryName, string? language, string? ownerName)
        {
            var relevantRepositories = await _gitHubService.SearchForAllRelevant(repositoryName, language, ownerName);
           
            return relevantRepositories;
        }
    }
}
