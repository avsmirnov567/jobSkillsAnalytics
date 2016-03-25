using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using JobSkillsDb.Entities;
using CsvHelper;
using System.Data.Entity;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Globalization;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

//using Parser;

namespace Apriori
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("CONNECTIN' TO DB...");
            string fileName = "";

            var context = new JobSkillsContext();
            //var apriori = context.AprioriRules.Count();
            //var eclat = context.EclatSets.Count();
            var minsupport = 0.03;
            var minconfidence = 0.1;

            Console.WriteLine("INITIALIZE DATA FROM DB...");
            var dch = new DbCsvHandler(minsupport, minconfidence, context);
            //dch.GetVacanciesCsv();
            
            Console.WriteLine("GETTIN' APRORI RULES...");
            Thread.Sleep(1000);

            //dch.ProcessDataWithAlgorithms();

            fileName = "APRIORI.csv";
            var arulesFile = DbCsvHandler.GetFileDirectory(fileName);
            var rules = dch.GetDataFromAprioriRulesCsv(arulesFile);

            //context.AprioriRules.RemoveRange(context.AprioriRules.ToList());
            //context.SaveChanges();
            //context.AprioriRules.RemoveRange(context.AprioriRules.ToList());
            //context.SaveChanges();
            dch.FillDatabase(rules);

            fileName = "ECLAT.csv";
            var eclatFile = DbCsvHandler.GetFileDirectory(fileName);
            var sets = dch.GetDataFromElcatRulesCsv(eclatFile);
            //dch.FillDatabase(sets);

            string input = "";
            do
            {
                Console.WriteLine("choose one: ");
                Console.WriteLine("1. recommend");
                Console.WriteLine("2. top");
                Console.WriteLine("3. close sample");

                input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Console.Write("enter the current your skills set:  ");
                        var textSet = Console.ReadLine();

                        var rec = dch.Recomend(textSet);
                        Console.WriteLine("processing recommendation...");
                        Console.WriteLine(rec);

                        break;
                    case "2":
                        Console.Write("1. lift / 2. conf / 3. supp: ");
                        
                        var userInput = Console.ReadLine();
                        var top = new List<AprioriRule>();

                        switch (userInput)
                        {
                            case "1":
                                top = DbCsvHandler.Top("lift");
                                foreach (var t in top)
                                {
                                    Console.WriteLine(t.LeftHandSide + " " + " =>>> " + t.RightHandSide + " " + t.Lift.ToString(CultureInfo.InvariantCulture));
                                    //Console.WriteLine(t.Lift.ToString(CultureInfo.InvariantCulture));
                                }
                                break;
                            case "2":
                                top = DbCsvHandler.Top("conf");
                                foreach (var t in top)
                                {
                                    Console.WriteLine(t.LeftHandSide + " " + " =>>> " + " " + t.RightHandSide + " " + t.Confidence.ToString(CultureInfo.InvariantCulture));
                                    //Console.WriteLine(t.Lift.ToString(CultureInfo.InvariantCulture));
                                }
                                break;
                            case "3":
                                top = DbCsvHandler.Top("supp");
                                foreach (var t in top)
                                {
                                    Console.WriteLine(t.LeftHandSide  + " =>>> " + t.RightHandSide + " " + " " + t.Support.ToString(CultureInfo.InvariantCulture));
                                    //Console.WriteLine(t.Lift.ToString(CultureInfo.InvariantCulture));

                                }
                                break;
                        }
                        break;
                    case "3":
                        break;
                }
            } while (input != "3");

            #region old - Contains custom apriori implementation

            //var givenSkills = context.Skills.ToList();
            //var givenVacancies = context.Vacancies.ToList();

            //var givenVacancies = context.Vacancies.Take(5).ToList();
            //foreach (var vac in givenVacancies)
            //{
            //    Console.WriteLine("name -> {0} / id -> {1}", vac.Title, vac.Id);
            //    foreach (var skill in vac.Skills)
            //    {
            //        Console.WriteLine("           -> {0}", skill.Name);
            //    }
            //}

            //var givenSkills = new List<Skill>();

            //givenSkills.AddRange(givenVacancies.ElementAt(0).Skills);
            //givenSkills.AddRange(givenVacancies.ElementAt(1).Skills);
            //givenSkills.AddRange(givenVacancies.ElementAt(2).Skills);
            //givenSkills.AddRange(givenVacancies.ElementAt(3).Skills);
            //givenSkills.AddRange(givenVacancies.ElementAt(4).Skills);

            //AprioriProcessTransactions process = new AprioriProcessTransactions(minsupport, minconfidence, givenSkills,
            //    givenVacancies);

            #endregion
        }
        
    }
}
