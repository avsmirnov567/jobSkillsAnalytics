using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.IO;

namespace Parser
{
    class MoiKrugParser : VacancyParserBase
    {
        const string DefSearchPageLink = @"/vacancies?page=1";
        const string Domain = @"http://moikrug.ru";

        public override List<string> GetLinks(string searchPage = null)
        {
            string searchPageLink = string.IsNullOrEmpty(searchPage) ? DefSearchPageLink : searchPage;
            HtmlWeb webStream = new HtmlWeb();
            Uri baseUri = new Uri(Domain); //domain URI
            List<string> links = new List<string>();
            do
            {
                string absPath = new Uri(baseUri, searchPageLink).ToString();
                HtmlDocument doc = webStream.Load(absPath);
                var jobLinks = doc.DocumentNode.SelectNodes(@"//ancestor::div[contains(@class,'title')]/a");
                var tempList = jobLinks
                   .Where(a => a.Attributes["href"] != null)
                   .Select(a => new Uri(baseUri, a.Attributes["href"].Value)) //returns the abs. path
                   .ToList();
                links.AddRange(tempList.Select(u => u.ToString())); //converts Uri to string
                var nextPageLink = doc.DocumentNode
                    .SelectSingleNode(@"//a[contains(@class,'next_page')]");
                searchPageLink = nextPageLink != null ? nextPageLink.Attributes["href"].Value : "";
            } while (!String.IsNullOrEmpty(searchPageLink));
            return links;
        }

        public override Vacancy Parse(string link)
        {
            throw new NotImplementedException();
        }

        public override Dictionary<Vacancy, bool> ParseAll(IEnumerable<string> links)
        {
            throw new NotImplementedException();
        }
    }
}
