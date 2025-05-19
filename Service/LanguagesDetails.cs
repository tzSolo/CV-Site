using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class LanguagesDetails
    {
        public Repository Repository { get; set; }
        public IEnumerable<RepositoryLanguage> Languages { get; set; }
    }
}
