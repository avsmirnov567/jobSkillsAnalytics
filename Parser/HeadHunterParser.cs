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
