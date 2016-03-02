using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.IO;
using System.Text.RegularExpressions;

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
            List<string> links = new List<string>();
            do
            {
                string absPath = Utils.GetAbsUrl(Domain, searchPageLink);
                HtmlDocument doc = webStream.Load(absPath);
                var jobLinks = doc.DocumentNode.SelectNodes(@"//ancestor::div[contains(@class,'title')]/a");
                var tempList = jobLinks
                   .Where(a => a.Attributes["href"] != null)
                   .Select(a => Utils.GetAbsUrl(Domain, a.Attributes["href"].Value))
                   .ToList();
                links.AddRange(tempList);
                var nextPageLink = doc.DocumentNode
                    .SelectSingleNode(@"//a[contains(@class,'next_page')]");
                searchPageLink = nextPageLink != null ? nextPageLink.Attributes["href"].Value : "";
            } while (!String.IsNullOrEmpty(searchPageLink));
            return links;
        }

        public override Vacancy Parse(string link)
        {
            HtmlWeb webStream = new HtmlWeb();
            HtmlDocument doc = webStream.Load(link);
            Vacancy vacancy = new Vacancy()
            {
                IDfromSite = GetId(link),
                Link = link,
                Title = GetTitle(doc),
                Employer = GetEmployer(doc),
                Salary = NormalizeSalary(GetSalary(doc)),
                PublishingDate = NormalizeDate(GetDate(doc)),
                ContentText = GetDescriptionText(doc),
                ContentHtml = GetDescriptionHtml(doc),
                Skills = GetSkillSet(doc)
            };
            return vacancy;
        }

        private string GetId(string link)
        {
            return Regex.Match(link, @"\d+").Value;
        }

        private string GetTitle(HtmlDocument doc)
        {
            var titleNode = doc.DocumentNode.SelectSingleNode(@"//h1[contains(@class,'title')]");
            return titleNode != null ? titleNode.InnerText : "";
        }

        private string GetSalary(HtmlDocument doc)
        {
            var salaryNode = doc.DocumentNode.SelectSingleNode(@"//span[contains(@class,'salary')]");
            return salaryNode != null ? salaryNode.InnerText : "";
        }

        private string GetDate(HtmlDocument doc)
        {
            var dateNode = doc.DocumentNode.SelectSingleNode(@"//span[contains(@class,'date')]");
            return dateNode != null ? dateNode.InnerText : "";
        }

        private string GetEmployer(HtmlDocument doc)
        {
            var employerNode = doc.DocumentNode.SelectSingleNode(@"//div[contains(@class,'company_name')]");
            return employerNode != null ? employerNode.InnerText : "";
        }

        private string GetDescriptionText(HtmlDocument doc)
        {
            var descriptionNode = doc.DocumentNode
                .SelectSingleNode(@"//div[contains(@class,'vacancy_description')]");
            return descriptionNode != null ? descriptionNode.InnerText : "";
        }

        private string GetDescriptionHtml(HtmlDocument doc)
        {
            var descriptionNode = doc.DocumentNode
                .SelectSingleNode(@"//div[contains(@class,'vacancy_description')]");
            return descriptionNode != null ? descriptionNode.InnerHtml : "";
        }

        private DateTime? NormalizeDate(string rawDate)
        {
            DateTime? date = null;
            string dateText = Regex.Match(rawDate, @"(\d+).+(\d)").Value;
            MatchCollection segments = Regex.Matches(dateText, @"\d+|[а-я]+");
            if (segments.Count == 3)
            {
                int day, year, month;
                int.TryParse(segments[0].Value, out day);
                int.TryParse(segments[2].Value, out year);
                switch (segments[1].Value.ToLower())
                {
                    case "января": month = 1; break;
                    case "февраля": month = 2; break;
                    case "марта": month = 3; break;
                    case "апреля": month = 4; break;
                    case "мая": month = 5; break;
                    case "июня": month = 6; break;
                    case "июля": month = 7; break;
                    case "августа": month = 8; break;
                    case "сентября": month = 9; break;
                    case "октября": month = 10; break;
                    case "ноября": month = 11; break;
                    case "декабря": month = 12; break;
                    default: month = DateTime.Now.Month; break;                    
                }
                try
                {
                    date = new DateTime(year, month, day);
                }
                catch
                {
                    date = null;
                }
            }
            return date;
        }

        public string NormalizeSalary(string rawSalary)
        {
            string salary = "";
            rawSalary = rawSalary.Replace(" ", "");
            List<string> salaryRange = Regex
                .Matches(rawSalary, @"\d+")
                .Cast<Match>()
                .Select(m => m.Value)
                .ToList();
            if (salaryRange.Count == 2)
            {
                salary = salaryRange[0] + ":" + salaryRange[1];                
            }
            else
            {
                salary = salaryRange.Count > 0 ? salaryRange[0] : "";
            }
            return salary;
        }

        private List<string> GetSkillSet(HtmlDocument doc)
        {
            List<string> skills = new List<string>();
            var skillNodes = doc.DocumentNode.SelectNodes(@"//a[contains(@class,'skill')]");
            if (skillNodes != null)
            {
                skills = skillNodes.Select(n => n.InnerText).ToList();
            }
            return skills;
        }

        public override Dictionary<Vacancy, bool> ParseAll(IEnumerable<string> links)
        {
            return links.Take(25).ToDictionary(l => Parse(l), l => true);
        }
    }
}
