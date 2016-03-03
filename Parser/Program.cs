using System;
using System.Linq;
using System.Threading.Tasks;

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
            var start = DateTime.Now;
            var dict1 = test.ParseAll(links);
            var end = DateTime.Now.Subtract(start).TotalSeconds;
            Console.WriteLine("Parallel finished in " + end + " second(s)");

            start = DateTime.Now;
            var dict2 = test.ParseAllStrait(links);
            end = DateTime.Now.Subtract(start).TotalSeconds;
            Console.WriteLine("Strait finished in " + end + " second(s)");
            Console.ReadLine();
        }
    }
}
