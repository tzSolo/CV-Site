using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IGitHubService
    {
        Task<PortfolioDetails> GetUserRepositories(string ownerName,string token);
        Task<RepositoryDetails> GetRepositoryDetails(Repository repository);
        Task<LanguagesDetails> GetRepositoryLanguages(Repository repository);
        Task<Commit> GetRepositoryLastCommit(Repository repository);
        Task<int> GetPullRequestCount(Repository repository);
        Task<IEnumerable<Repository>> SearchForAllRelevant(string? repositoryName, string? language, string? ownerName);
    }
}
