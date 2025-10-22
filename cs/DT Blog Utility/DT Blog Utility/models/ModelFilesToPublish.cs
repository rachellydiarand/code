using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DT_Blog_Utility.models
{
    public class ModelFilesToPublish
    {
        public string SiteUrl { get; set; }
        public string PublishDirectory { get; set; }
        public List<string> Mutations { get; set; }
        public List<string> UrlMutations { get; set; }
        public List<string> Pages { get; set; }
    }
}
