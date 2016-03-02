using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    internal class Utils
    {
        public static string GetAbsUrl(string domain, string link)
        {
            Uri domainUri = new Uri(domain);
            Uri fullUri = new Uri(domainUri, link);
            return fullUri.ToString();
        }
    }
}
