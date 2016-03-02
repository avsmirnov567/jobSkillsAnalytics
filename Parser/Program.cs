using System;

namespace Parser
{
    class Program
    {
        static void Main(string[] args)
        {       
            MoikrugParser.CrawlAllVacanciesURLs();
            MoikrugParser.ParseAllVacancies();
            Console.ReadKey();
        }
    }
}
