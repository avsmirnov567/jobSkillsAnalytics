//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using HtmlAgilityPack;

//namespace Parser
//{
//    /// <summary>
//    /// This class contains methods for parsing "moikrug.ru" in order to browse all vacancies into database.
//    /// </summary>
//    static class MoikrugParserOld
//    {
//        private static List<string> RSS_PagesURLs = new List<string>();
//        private static List<string> vacanciesPagesURLs = new List<string>();
//        private static List<Vacancy> vacanciesForDatabase = new List<Vacancy>();
        
//        public static void CrawlRSS_PagesURLs()
//        {           
//            //HTML document loading
//            HtmlDocument searchResultsPage = new HtmlDocument();
//            string currentPageUrl = "https://moikrug.ru/vacancies";
//            HtmlWeb webStream = new HtmlWeb();
//            searchResultsPage = webStream.Load(currentPageUrl);

//            //Pagination crawling
//            RSS_PagesURLs.Add("https://moikrug.ru/vacancies");
//            string nextPageUrl = "https://moikrug.ru/vacancies";
//            Console.WriteLine(nextPageUrl);
//            HtmlNode nextPageNode = null;

//            do
//            {
//                HtmlNodeCollection temp = searchResultsPage //browse node with URL of next page
//                    .DocumentNode
//                    .SelectNodes("//a[contains(@class,'next_page')]");

//                if (temp != null) //if next page exists
//                {
//                    nextPageNode = temp[0];
//                    nextPageUrl = "https://moikrug.ru" + nextPageNode.Attributes["href"].Value;
//                    RSS_PagesURLs.Add(nextPageUrl); //save the URL

//                    Console.WriteLine(nextPageUrl); //browse next page TODO DELETE CONSOLE!
//                    currentPageUrl = nextPageUrl;
//                    searchResultsPage = webStream.Load(currentPageUrl);
//                }
//                else
//                    nextPageUrl = "";
//            }
//            while (nextPageUrl != "");
//        }

//        public static void CrawlVacanciesURLsFromSingleRSS_Page(string RSS_PageURL)
//        {
//            //browsing RSS Page
//            HtmlDocument RSS_Page = new HtmlDocument();
//            HtmlWeb webStream = new HtmlWeb();
//            RSS_Page = webStream.Load(RSS_PageURL);

//            //browsing nodes which contain vacancies info
//            HtmlNodeCollection vacanciesNodes = RSS_Page.DocumentNode.SelectNodes("//div[contains(@class,'job  ')]");
//            //making another XPath query in order to browse marked vacancies nodes (NE MOGU V XPATH)
//            HtmlNodeCollection markedVacanciesNodes = RSS_Page.DocumentNode.SelectNodes("//div[contains(@class,'job marked')]");

//            if (markedVacanciesNodes != null)
//            {
//                foreach (HtmlNode markedVacancyNode in markedVacanciesNodes)
//                    vacanciesNodes.Add(markedVacancyNode);
//            }

//            //browsing vacancies URLs from the collection of HTML nodes
//            for (int i=0; i < vacanciesNodes.Count; i++) 
//            {
//                string URL = "https://moikrug.ru/vacancies/" + vacanciesNodes[i].Id.Replace("job_", "");
//                vacanciesPagesURLs.Add(URL);
//                Console.WriteLine(URL);
//            }
//        }

//        public static void CrawlAllVacanciesURLs()
//        {
//            CrawlRSS_PagesURLs();
//            foreach (string URL in RSS_PagesURLs)
//            {
//                CrawlVacanciesURLsFromSingleRSS_Page(URL);
//            }       
//        }

//        public static Vacancy ParseSingleVacancyByURL(string URL)
//        {
//            HtmlDocument vacancyPage = new HtmlDocument();
//            HtmlWeb webStream = new HtmlWeb();
//            vacancyPage = webStream.Load(URL);

//            Vacancy parsingVacancy = new Vacancy();
//            parsingVacancy.Link = URL;
//            parsingVacancy.InnerId = Convert.ToInt32(URL.Replace("https://moikrug.ru/vacancies/", ""));

//            //TODO: organise exceptions catching correctly
//            try//one by one, searching for nodes which contains field info, and browsing InnerText
//            {
//                HtmlNode vacancyDescriptionNode, publisherNode, nameNode, dateNode, salaryNode;
//                GetHtmlNodeValues(vacancyPage, out vacancyDescriptionNode, out publisherNode, out nameNode, out dateNode, out salaryNode);

//                parsingVacancy.Content = vacancyDescriptionNode.InnerHtml; //TODO: remove html tags from text correctly
//                parsingVacancy.Employer = publisherNode.InnerText;
//                parsingVacancy.Title = nameNode.InnerText;
//                parsingVacancy.PublishingDate = ParseDateFromString(dateNode.InnerText);

//                if (salaryNode != null)
//                    parsingVacancy.Salary = salaryNode.InnerText;

//                HtmlNodeCollection skillsNodes = vacancyPage.DocumentNode.SelectNodes("//a[contains(@class,'skill')]");

//                if (skillsNodes != null)
//                {
//                    foreach (HtmlNode skillNode in skillsNodes)
//                        parsingVacancy.Skills.Add(skillNode.InnerText);
//                }
//            }

//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//                return null;
//            }
            
//            return parsingVacancy;
//        }

//        private static void GetHtmlNodeValues(HtmlDocument vacancyPage, out HtmlNode vacancyDescriptionNode, out HtmlNode publisherNode, out HtmlNode nameNode, out HtmlNode dateNode, out HtmlNode salaryNode)
//        {
//            vacancyDescriptionNode = vacancyPage
//                                        .DocumentNode
//                                        .SelectSingleNode("//div[contains(@class,'vacancy_description')]");
//            publisherNode = vacancyPage
//                                        .DocumentNode
//                                        .SelectSingleNode("//div[contains(@class,'company_name')]");
//            nameNode = vacancyPage
//                                        .DocumentNode
//                                        .SelectSingleNode("//h1[contains(@class,'title')]");
//            dateNode = vacancyPage
//                                        .DocumentNode
//                                        .SelectSingleNode("//span[contains(@class,'date')]");
//            salaryNode = vacancyPage
//                                        .DocumentNode
//                                        .SelectSingleNode("//span[contains(@class,'salary')]");
//        }

//        public static void ParseAllVacancies()
//        {
//            int count = 0;
            
//            foreach (string URL in vacanciesPagesURLs)
//            {
//                Vacancy currentVacancy = ParseSingleVacancyByURL(URL);
//                if (currentVacancy != null)
//                {
//                    vacanciesForDatabase.Add(ParseSingleVacancyByURL(URL));
//                    count++;
                    
//                    Console.WriteLine(count);
//                }
//            }
//        }

//        public static string GetClearInnerTextFromNode(HtmlNode Node)
//        {
//            var root = Node;
//            var sb = new StringBuilder();
            
//            foreach (var node in root.DescendantsAndSelf())
//            {
//                if (!node.HasChildNodes)
//                {
//                    string text = node.InnerText;
//                    if (!string.IsNullOrEmpty(text))
//                        sb.AppendLine(text.Trim());
//                }
//            }

//            return sb.ToString();
//        }//TODO: fix (html tags removing)

//        public static DateTime ParseDateFromString(string date)
//        {
//            date = date.Replace("Вакансия размещена ", "")
//                .Replace(" января ", ".01.")
//                .Replace(" февраля ", ".02.")
//                .Replace(" марта ", ".03.")
//                .Replace(" апреля ", ".04.")
//                .Replace(" мая ", ".05.")
//                .Replace(" июня ", ".06.")
//                .Replace(" июля ", ".07.")
//                .Replace(" августа ", ".08.")
//                .Replace(" сентября ", ".09.")
//                .Replace(" октября ", ".10.")
//                .Replace(" ноября ", ".11.")
//                .Replace(" декабря ", ".12.")
//                .Replace(" &bull; ", "");
                
//            return Convert.ToDateTime(date);            
//        }

//        public static string[] ParseSalaryFromString(string salary)
//        {
//            salary = salary.Remove(0, 3)
//                           .Replace(" ", "")
//                           .Replace("до", ":")
//                           .Replace("От", ":");
                           
//            string currency = salary.Substring(salary.Length - 4, 4);
//            salary = salary.Replace(currency, ":" + currency);       
                                
//            return salary.Split(':').ToArray();
//        }//TODO: fix (make an algorythm for correct parsing of salaries)
//    }
//}
