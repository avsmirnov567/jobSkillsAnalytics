using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;

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
            var dictParallel = test.ParseAll(links);
            var end = DateTime.Now.Subtract(start).TotalSeconds;
            Console.WriteLine("Parallel finished in " + end + " second(s)");

            CsvVacancyExporter exporter = new CsvVacancyExporter(dictParallel.Keys.ToList());
            exporter.ExportVacancies();
            exporter.ExportSkills();
        }
    }
}
