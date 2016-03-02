using System;

namespace Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            //MoikrugParserOld.CrawlAllVacanciesURLs();
            //MoikrugParserOld.ParseAllVacancies();
            //Console.ReadKey();
            MoiKrugParser test = new MoiKrugParser();
            var links = test.GetLinks();
            var dict = test.ParseAll(links);
        }
    }
}
