using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class GitHubService(IOptions<GitHubIntegrationOptions> options) : IGitHubService
    {
        private readonly GitHubClient _client = new(new ProductHeaderValue("cv-site"))
        {
            Credentials = new Credentials(options.Value.PersonalAccessToken)
        };


        public async Task<PortfolioDetails> GetUserRepositories(string ownerName, string token)
        {
            var allUserRepositories = await _client.Repository.GetAllForUser(ownerName);

            var repositoriesDetails = new List<RepositoryDetails>();

            foreach (var repository in allUserRepositories)
            {
                if (repository != null)
                {
                    var repositoryDetails = await GetRepositoryDetails(repository);

                    repositoriesDetails.Add(repositoryDetails);
                }
            }

            var ownerPortfolioDetails = new PortfolioDetails()
            {
                AllRepositoriesDetails = repositoriesDetails
            };

            return ownerPortfolioDetails;
        }


        public async Task<RepositoryDetails> GetRepositoryDetails(Repository repository)
        {
            var repositoryDetails = new RepositoryDetails()
            {
                Name = repository.Name,
                LastCommit = await GetRepositoryLastCommit(repository),
                LanguagesDetails = await GetRepositoryLanguages(repository),
                StargazersCount = repository.StargazersCount,
                PullRequestsCount = await GetPullRequestCount(repository),
                DeploymentUrl = await GetRepositoryLinkToDeployment(repository)
            };

            return repositoryDetails;
        }


        public async Task<LanguagesDetails> GetRepositoryLanguages(Repository repository)
        {
            var portfolioDetails = new LanguagesDetails();

            var allRepositoryLanguages = await _client.Repository.GetAllLanguages(repository.Owner.Login, repository.Name);

            portfolioDetails.Languages = allRepositoryLanguages;

            return portfolioDetails;
        }


        public async Task<Commit> GetRepositoryLastCommit(Repository repository)
        {
            var allRepositoriesCommits = await _client.Repository.Commit.GetAll(repository.Owner.Login, repository.Name);

            if (allRepositoriesCommits.Count > 0)
            {
                return allRepositoriesCommits[0].Commit;
            }

            return new Commit();
        }


        public async Task<int> GetPullRequestCount(Repository repository)
        {
            var pullRequests = await _client.PullRequest.GetAllForRepository(repository.Owner.Login, repository.Name);

            return pullRequests.Count;
        }


        public async Task<string> GetRepositoryLinkToDeployment(Repository repository)
        {
            var deploymentDetails = await _client.Repository.Deployment.GetAll(repository.Owner.Login, repository.Name);
            
            if (deploymentDetails.Count > 0)
            {
                var deploymentUrl = deploymentDetails[0].Url;
                return deploymentUrl;
            }

            return "There is no deployment url.";
        }

        public async Task<IEnumerable<Repository>> SearchForAllRelevant(string? repositoryName, string? language, string? ownerName)
        {
            var searchQuery = "";
            if (!string.IsNullOrEmpty(repositoryName))
            {
                searchQuery += $"{repositoryName} in:name";
            }
            if (!string.IsNullOrEmpty(ownerName))
            {
                searchQuery += $"user:{ownerName} ";
            }
            if (!string.IsNullOrEmpty(language))
            {
                searchQuery += $"language:{language}";
            }

            var allRelevantRepositories = await _client.Search.SearchRepo(new SearchRepositoriesRequest(searchQuery)
            {
                SortField = RepoSearchSort.Stars
            });

            return allRelevantRepositories.Items;
        }
    }
}
