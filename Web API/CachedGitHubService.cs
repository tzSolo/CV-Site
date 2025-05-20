using Microsoft.Extensions.Caching.Memory;
using Octokit;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_API
{
    public class CachedGitHubService(IGitHubService gitHubService, IMemoryCache memoryCache) : IGitHubService
    {
        private readonly IGitHubService _gitHubService = gitHubService;
        private readonly IMemoryCache _memoryCache = memoryCache;
        private const string UserPortfolio = "UserPortfolio";

        public async Task<PortfolioDetails> GetUserRepositories(string ownerName, string token)
        {
            if (_memoryCache.TryGetValue("UserPortfolio", out PortfolioDetails portfolio))
            {
                return portfolio;
            }

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(45))
                .SetSlidingExpiration(TimeSpan.FromSeconds(20));

            portfolio = await _gitHubService.GetUserRepositories(ownerName, token);

            _memoryCache.Set(UserPortfolio, portfolio, cacheOptions);

            return portfolio;
        }

        public Task<RepositoryDetails> GetRepositoryDetails(Repository repository)
        {
            return _gitHubService.GetRepositoryDetails(repository);
        }

        public Task<LanguagesDetails> GetRepositoryLanguages(Repository repository)
        {
            return _gitHubService.GetRepositoryLanguages(repository);
        }

        public Task<Commit> GetRepositoryLastCommit(Repository repository)
        {
            return _gitHubService.GetRepositoryLastCommit(repository);
        }

        public Task<int> GetPullRequestCount(Repository repository)
        {
            return _gitHubService.GetPullRequestCount(repository);
        }

        public Task<IEnumerable<Repository>> SearchForAllRelevant(string? repositoryName, string? language, string? ownerName)
        {
            return _gitHubService.SearchForAllRelevant(repositoryName, language, ownerName);
        }
    }
}
