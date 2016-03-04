using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;

namespace Parser
{
    public sealed class VacancyCsvMapping : CsvClassMap<VacancyView>
    {
        public VacancyCsvMapping()
        {
            Map(v => v.InnerId).Name("IdFromSite");
            Map(v => v.Title).Name("Title");
            Map(v => v.Link).Name("Link");
            Map(v => v.ContentHtml).Name("ContentHtml");
            Map(v => v.ContentText).Name("ContentText");
            Map(v => v.Employer).Name("Employer");
            Map(v => v.Salary).Name("Salary");
            Map(v => v.PublishingDate).Name("PublishingDate");
            //Map(v => v.Skills.Count).Name("ScillsCount"); //не придумал как вывести пока
        }
    }
}
