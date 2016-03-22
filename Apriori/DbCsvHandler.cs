using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using JobSkillsDb.Entities;
using CsvHelper.Configuration;
using System.Globalization;

namespace Apriori
{

    class DbCsvHandler
    {
        

        private double sup;
        private double conf;
        private JobSkillsContext context;

        public DbCsvHandler(double sup, double conf, JobSkillsContext context)
        {
            this.sup = sup;
            this.conf = conf;
            this.context = context;
        }

        
        public void ProcessDataWithAlgorithms()
        {
            var rScriptDirectory = GetFileDirectory("rscript.R");
            var strCmdLine = "R CMD BATCH " + rScriptDirectory + sup + " " + conf;
            Process.Start("CMD.exe", strCmdLine);
        }


        
        /// <summary>
        /// Return file (dataset.csv) in relative project directory
        /// File consist of data from db (transactions = vacancies)
        /// each transaction = ICollection of Skill type
        /// </summary>
        public void GetVacanciesCsv()
        {
            var vacancies = context.Vacancies.Include(v => v.Skills)
                .Select(v => new { v.Id, v.Skills }).ToList()
                .Select(v => new Vacancy()
                {
                    Id = v.Id,
                    Skills = v.Skills
                }).ToList();

            var fileDirectory = GetFileDirectory("dataset.csv");

            ExportBasketType(vacancies, fileDirectory);
        }

        /// <summary>
        /// Get relative directory 
        /// </summary>
        /// <param name="file">What is name of file should I get </param>
        /// <returns></returns>
        public static string GetFileDirectory(string file)
        {
            var directory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

            string RSCRIPT_DIRECTORY =
                directory + @"\RSCRIPT\" + file;
            return RSCRIPT_DIRECTORY;
        }

        /// <summary>
        /// Read rules from APRIORI.csv
        /// </summary>
        /// <param name="file"></param>
        public List<AprioriRule> GetDataFromAprioriRulesCsv(string rules_file)
        {
            context = new JobSkillsContext();

            var csv = File.ReadAllLines(rules_file, Encoding.UTF8).Select(a => a.Split('/').ToList()).ToList();
            csv.RemoveAt(0); //remove headers
            
            var ruleEntityList = new List<AprioriRule>();

            foreach (var row in csv)
            {
                var rules = row.ElementAt(0);
                var lhsrhs = LHSRHSStringsGeneratorFromApriori(rules);
                var lhs = lhsrhs[0];
                var rhs = lhsrhs[1];

                ruleEntityList.Add(
                    new AprioriRule
                    {
                        LeftHandSide = lhs,
                        RightHandSide = rhs,
                        Rules = row.ElementAt(0),
                        Support = double.Parse(row.ElementAt(1), CultureInfo.InvariantCulture), //{...} => {...}      
                        Confidence = double.Parse(row.ElementAt(2), CultureInfo.InvariantCulture), //0.0000000000
                        Lift = double.Parse(row.ElementAt(3), CultureInfo.InvariantCulture) //0.0000000000
                    });
            }
            FillDatabaseByAprioriRules(ruleEntityList);
            return ruleEntityList;
        }

        private void FillDatabaseByAprioriRules(List<AprioriRule> ruleEntityList)
        {
            //context.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo.AprioriRules]");
            foreach (var rule in ruleEntityList)
                context.AprioriRules.Add(rule);
            context.SaveChanges();
        }


        /// <summary>
        /// Read frequent itemsets from ELCAT.csv
        /// </summary>
        /// <param name="eclat_file"></param>
        public void GetDataFromElcatRulesCsv(string eclat_file)
        {
            context = new JobSkillsContext();

            using (var readingStream = new StreamReader(eclat_file, Encoding.UTF8))
            {
                var reader = new CsvReader(readingStream);
                //var records = reader.GetRecords<>();
            }
        }
        

        /// <summary>
        /// Parse rules from row of file
        /// </summary>
        /// <param name="rule_record_part_of_record"></param>
        /// <returns></returns>
        public List<string> LHSRHSStringsGeneratorFromApriori(string rule_record_part_of_record)
        {
            var splitted = rule_record_part_of_record.Split(new string[] { "=>" }, StringSplitOptions.None).ToList();
            char[] charsToTrim = {' ','}', '{', '"', '/'}; 
            for (var i = 0; i < splitted.Count; i++)
            {
                splitted[i] = splitted[i].Replace(@"\", "");
                splitted[i] = splitted[i].Trim(charsToTrim);
            }
            return splitted;
        }

        /// <summary>
        /// Generate right (rules) part of record
        /// </summary>
        /// <param name="frequent_set_part_of_record"></param>
        /// <returns></returns>
        public List<string> FrequentSetGeneratorFromEclat(string frequent_set_part_of_record)
        {
            var splitted = frequent_set_part_of_record.Split(new char[] { ',' }, StringSplitOptions.None).ToList();
            for (int i = 0; i < splitted.Count; i++)
            {
                splitted[i] = splitted[i].Trim(' ');
            }
            return splitted;
        }

        /// <summary>
        /// Export handled data from db to file (dataset) 
        /// in a basket type for arules in R
        /// </summary>
        /// <param name="vacancies"></param>
        /// <param name="fileName"></param>
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

        
    }
}