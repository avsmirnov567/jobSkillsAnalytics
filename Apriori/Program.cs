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
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

//using Parser;

namespace Apriori
{
    class Program
    {
        public static void ExportBasketType(List<Vacancy> vacancies, string fileName = "")
        {
           
            string name = string.IsNullOrEmpty(fileName)
                ? @"session#" + Guid.NewGuid().ToString() + ".csv"
                : fileName;

            using (var writingStream = new StreamWriter(name, true, Encoding.UTF8))
            {
                var fileWriter = new CsvWriter(writingStream);
                //var recordContainer = "";

                //const string separator = ",";
                int maxSkillsetLength = 0;
                int optimalSkillsetLength = 0;

                IEnumerable<Skill> maxSet = null;
                IEnumerable<Skill> optimalMaxSet = null;

                int idOfMaxVacancy = 0;
                int idOfOptimalMaxVacancy = 0;

                Debug.WriteLine("SEARCH MAX...");

                foreach (var vac in vacancies)
                {
                    if (vac.Skills.Count > maxSkillsetLength)
                    {

                        //return previous fact of condition
                        optimalSkillsetLength = maxSkillsetLength;
                        idOfOptimalMaxVacancy = idOfMaxVacancy;
                        optimalMaxSet = maxSet;

                        //return fact of condition
                        maxSkillsetLength = vac.Skills.Count;
                        maxSet = vac.Skills;
                        idOfMaxVacancy = vac.Id;
                    }
                }


                Debug.WriteLine("WRITE TO FILE...");
                foreach (var item in optimalMaxSet)
                {
                    fileWriter.WriteField(item.Name);
                }
                fileWriter.NextRecord();

                foreach (var vac in vacancies.Where(x => x.Id != idOfMaxVacancy && x.Id != idOfOptimalMaxVacancy))
                {
                    if (vac.Skills.Count == 0)
                        continue;
                    if (vac.Skills.Any(item => item.Name.Contains("Продажи") || item.Name.Contains("продажи")))
                        continue;

                    foreach (var skill in vac.Skills)
                    {
                        fileWriter.WriteField(skill.Name);
                    }
                    fileWriter.NextRecord();
                }
            }

            Debug.WriteLine("EXPORTED");
        }

        static void Main(string[] args)
        {
            
            Debug.WriteLine("CONNECTING TO DB...");
            string fileName = "";

            var context = new JobSkillsContext();

            var minsupport = 0.4;
            var minconfidence = 0.4;
            
            var dch = new DbCsvHandler(minsupport, minconfidence, new JobSkillsContext());
            dch.GetVacanciesCsv();

            dch.ProcessDataWithAlgorithms();

            Console.WriteLine("GOING TO FILES");

            fileName = "APRIORI.csv";
            var arulesFile = DbCsvHandler.GetFileDirectory(fileName);
            dch.GetDataFromAprioriRulesCsv(arulesFile);

           // fileName = "ECLAT.csv";
            //var eclatFile = DbCsvHandler.GetFileDirectory(fileName);
            //dch.GetDataFromElcatRulesCsv(eclatFile);
            int counter = 0;
            foreach (var el in context.AprioriRules)
            {
                Console.WriteLine(el.LeftHandSide + "=>" + el.RightHandSide);
                counter++;
            }
            Console.WriteLine("OK " + counter);
            Console.ReadLine();

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
