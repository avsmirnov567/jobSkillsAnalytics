using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    class HeadHunterParser: VacancyParserBase
    {
        const string XPathForLinks = @"//a[contains(@class,'search-result-item__name')]";
        const string DefSearchPageLink = @"http://perm.hh.ru/search/vacancy?enable_snippets=true&industry=7&clusters=true&area=113&page=0";
        const string Domain = "http://perm.hh.ru";
        public override List<string> GetLinks(string searchPage = null)
        {
            throw new NotImplementedException();
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
