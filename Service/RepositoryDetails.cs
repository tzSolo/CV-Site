using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class RepositoryDetails
    {
        public string Name { get; set; }
        public LanguagesDetails LanguagesDetails { get; set; }
        public Commit LastCommit { get; set; }
        public int StargazersCount { get; set; }
        public int PullRequestsCount { get; set; }
        public string LinkToRepository { get; set; }
    }
}
