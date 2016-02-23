using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;


namespace TryParser
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
