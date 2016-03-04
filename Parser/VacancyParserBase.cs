using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    /// <summary>
    /// Abstract class to define the common behavior for all parsers
    /// </summary>
    public abstract class VacancyParserBase
    {
        /// <summary>
        /// Get all vacancies URLs
        /// </summary>
        /// <param name="searchPage">In not null, then this URL must be handled</param>
        /// <returns>URLs</returns>
        public abstract List<string> GetLinks(string searchPage = null);

        /// <summary>
        /// Parse vacancy if content is valid
        /// </summary>
        /// <param name="link">Link to vacancy's page</param>
        /// <returns>Parser vacancy</returns>
        public abstract VacancyView Parse(string link);


        /// <summary>
        /// Parse all vacancies
        /// </summary>
        /// <param name="links">URLs</param>
        /// <returns>Dictionary where key is parsed vacancy, and value is flag showing was everything
        /// fine or data was not valid (or other errors fired)
        /// </returns>
        public abstract Dictionary<VacancyView, bool> ParseAll(IEnumerable<string> links);
    }
}
