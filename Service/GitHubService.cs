using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class GitHubService
    {
        private readonly GitHubClient _client = new GitHubClient(new ProductHeaderValue("cv-site"));

        public void ContactUser(string UserName,string Token)
        {
            var getUserDetails = _client.User.Get(UserName);
        }
    }
}
