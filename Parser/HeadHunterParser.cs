using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Parser
{
    class HeadHunterParser: VacancyParserBase
    {
        const string XPathForLinks = @"//a[contains(@class,'search-result-item__name')]";
        const string DefSearchPageLink = @"http://perm.hh.ru/search/vacancy?enable_snippets=true&industry=7&clusters=true&area=113&page=0";
        readonly string[] DefSearchPageLinkList =
            { @"http://perm.hh.ru/search/vacancy?enable_snippets=true&industry=7.540&clusters=true&area=113&page=0",
                @"http://perm.hh.ru/search/vacancy?enable_snippets=true&industry=7.539&clusters=true&area=113&page=0",
                @"http://perm.hh.ru/search/vacancy?enable_snippets=true&industry=7.541&clusters=true&area=113&page=0" };
        const string Domain = "http://perm.hh.ru";
        public override List<string> GetLinks(string searchPage = null)
        {
            string searchPageLink = string.IsNullOrEmpty(searchPage) ? DefSearchPageLink : searchPage;
            HtmlWeb webStream = new HtmlWeb();
            List<string> links = new List<string>();            
            do
            {                
                string absPath = Utils.GetAbsUrl(Domain, searchPageLink);
                Debug.WriteLine("Parsing search page: " + absPath);
                HtmlDocument doc = null;
                int maxCount = 3;
                while (doc == null && maxCount > 0)
                {
                    try
                    {
                        doc = webStream.Load(absPath);
                    }
                    catch
                    {
                        Debug.WriteLine("Can't access: " + absPath);
                        doc = null;
                        maxCount--;
                    }
                }
                if (maxCount > 0)
                {
                    var jobLinks = doc.DocumentNode.SelectNodes(@"//a[contains(@class,'search-result-item__name')]");
                    var tempList = jobLinks
                       .Where(a => a.Attributes["href"] != null)
                       .Select(a => Utils.GetAbsUrl(Domain, a.Attributes["href"].Value))
                       .ToList();
                    links.AddRange(tempList);
                    var nextPageLink = doc.DocumentNode
                        .SelectSingleNode(@"//div[contains(@class, 'b-pager__next')]/a");
                    searchPageLink = nextPageLink != null ? nextPageLink.Attributes["href"].Value : "";
                }
                else
                {
                    Debug.WriteLine("Can't continue parsing vacancy links: " + absPath);
                    searchPageLink = null;
                }
            } while (!String.IsNullOrEmpty(searchPageLink));
            return links;
        }

        public override VacancyView Parse(string link)
        {
            HtmlWeb webStream = new HtmlWeb();
            HtmlDocument doc = null;
            int maxCount = 3;
            while (doc == null && maxCount > 0)
            {
                try
                {
                    doc = webStream.Load(link);
                }
                catch
                {
                    Debug.WriteLine("Can't access: " + link);
                    doc = null;
                    maxCount--;
                }
            }
            VacancyView vacancy = new VacancyView();
            Debug.WriteLineIf(maxCount == 0, "Can't continue parsing vacancy: " + link);
            if (maxCount > 0)
            {
                Debug.WriteLine("Parsing vacancy: " + link);
                vacancy = new VacancyView()
                {
                    InnerId = GetId(link),
                    Link = link,
                    Title = GetTitle(doc),
                    Employer = GetEmployer(doc),
                    Salary = NormalizeSalary(GetSalary(doc)),
                    PublishingDate = NormalizeDate(GetDate(doc)),
                    ContentText = GetDescriptionText(doc),
                    ContentHtml = GetDescriptionHtml(doc),
                    Skills = GetSkillSet(doc)
                };
            }
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
            var salaryNode = doc.DocumentNode.SelectSingleNode(@"//td[contains(@class,'b-v-info-content')][1]/div[contains(@class,'l-paddings')]");
            return salaryNode != null ? salaryNode.InnerText : "";
        }

        private string GetDate(HtmlDocument doc)
        {
            var dateNode = doc.DocumentNode.SelectSingleNode(@"//time");
            return dateNode != null ? dateNode.InnerText : "";
        }

        private string GetEmployer(HtmlDocument doc)
        {
            var employerNode = doc.DocumentNode.SelectSingleNode(@"//div[contains(@class,'companyname')]/a");
            return employerNode != null ? employerNode.InnerText : "";
        }

        private string GetDescriptionText(HtmlDocument doc)
        {
            var descriptionNode = doc.DocumentNode
                .SelectSingleNode(@"//div[contains(@class,'b-vacancy-desc-wrapper')]");
            return descriptionNode != null ? descriptionNode.InnerText : "";
        }

        private string GetDescriptionHtml(HtmlDocument doc)
        {
            var descriptionNode = doc.DocumentNode
                .SelectSingleNode(@"//div[contains(@class,'b-vacancy-desc-wrapper')]");
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
            Regex rgx = new Regex(@"\s+");
            string rawTempSalary = rgx.Replace(rawSalary, "");
            List<string> salaryRange = Regex
                .Matches(rawTempSalary, @"\d+")
                .Cast<Match>()
                .Select(m => m.Value)
                .ToList();
            if (salaryRange.Count >= 2)
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
            var skillNodes = doc.DocumentNode.SelectNodes(@"//span[contains(@data-qa,'skills-element')]");
            if (skillNodes != null)
            {
                skills = skillNodes.Select(n => n.InnerText).ToList();
            }
            return skills;
        }

        private List<VacancyView> ParseForParallel(IEnumerable<string> links)
        {
            return links.Select(l => Parse(l)).ToList();
        }

        public override Dictionary<VacancyView, bool> ParseAll(IEnumerable<string> links)
        {
            int take = 25;
            List<List<string>> splitedList = new List<List<string>>();
            var tempList = links;
            while (tempList.Any())
            {
                splitedList.Add(tempList.Take(take).ToList());
                tempList = tempList.Skip(take);
            }

            List<VacancyView> vacancies = new List<VacancyView>();

            Parallel.ForEach(splitedList, linkList =>
            {
                vacancies.AddRange(ParseForParallel(linkList));
            });

            return vacancies.ToDictionary(x => x, x => true);
        }

        public Dictionary<VacancyView, bool> ParseAllStrait(IEnumerable<string> links)
        {
            return links.ToDictionary(x => Parse(x), x => true);
        }

        public override List<string> GetAllLinks(IEnumerable<string> searchPages = null)
        {
            var startWith = searchPages ?? DefSearchPageLinkList;
            List<string> links = new List<string>();
            Parallel.ForEach(startWith, searchPage =>
            {
                links.AddRange(GetLinks(searchPage));
            });
            return links.Distinct().ToList();
        }
    }
}
