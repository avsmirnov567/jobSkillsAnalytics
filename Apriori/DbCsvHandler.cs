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
using RDotNet;

namespace Apriori
{

    public class DbCsvHandler
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

        public static string RunFromCmd(string rCodeFilePath, string rScriptExecutablePath, string args)
        {
            string file = rCodeFilePath;
            string result = string.Empty;

            try
            {

                var info = new ProcessStartInfo();
                info.FileName = rScriptExecutablePath;
                info.WorkingDirectory = Path.GetDirectoryName(rScriptExecutablePath);
                info.Arguments = rCodeFilePath + " " + args;

                info.RedirectStandardInput = false;
                info.RedirectStandardOutput = true;
                info.UseShellExecute = false;
                info.CreateNoWindow = true;

                using (var proc = new Process())
                {
                    proc.StartInfo = info;
                    proc.Start();
                    result = proc.StandardOutput.ReadToEnd();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("R Script failed: " + result, ex);
            }
        }

        public void ProcessDataWithAlgorithms()
        {
            //REngine.SetEnvironmentVariables();
            //REngine engine = REngine.GetInstance();
            //// REngine requires explicit initialization.
            //// You can set some parameters.
            //engine.Initialize();
            //var rScriptDirectory = GetFileDirectory("rscript.r");
            // var execPath = @"C:\Program Files\R\R-3.2.4revised\bin\Rscript.exe";
            //var strCmdLine = "R CMD BATCH " + rScriptDirectory + " " + sup + " " + conf;

            //RunFromCmd(rScriptDirectory, execPath, "0,02 0,1");
            //Process.Start("CMD.exe", strCmdLine);
            //engine.Evaluate("source('" + rScriptDirectory + "')");
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
                if (row[1].Contains("C++} => {C++}")) continue;

                var rules = row.ElementAt(0); //getting rule part of the row

                var lhsrhs = LHSRHSStringsGeneratorFromApriori(rules);

                var lhs = lhsrhs[0];
                if (lhs.Contains("Oracle Pl")) continue;
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
            return ruleEntityList;
        }

        /// <summary>
        /// Read frequent itemsets from ELCAT.csv
        /// </summary>
        /// <param name="eclat_file"></param>
        public List<EclatSet> GetDataFromElcatRulesCsv(string eclat_file)
        {
            context = new JobSkillsContext();
            var csv = File.ReadAllLines(eclat_file, Encoding.UTF8).Select(a => a.Split('/').ToList()).ToList(); 
            csv.RemoveAt(0); // remove headers

            var setList = new List<EclatSet>();
            var counter = 0;
            foreach (var row in csv)
            {
                var set = FrequentSetGeneratorFromEclat(row.ElementAt(0)); // get set of items
                var supp = row.ElementAt(1);
                if (supp.Contains("SQL}") || supp.Contains("IP}"))
                { continue;}
                if (counter == 6497 || counter == 18983)
                {
                    counter++;
                    continue;
                }
                var setSupport = double.Parse(row.ElementAt(1), CultureInfo.InvariantCulture);

                var writeSet = string.Join(",", set.Select(p => p.ToString()).ToList());

                setList.Add(
                    new EclatSet
                    {
                        ItemSet = writeSet,
                        Support = setSupport
                    });
                counter++;
            }
            
          return setList;
        }

        public void FillDatabase(List<AprioriRule> ruleEntityList)
        {
            //context.Database.ExecuteSqlCommand("TRUNCATE TABLE [dbo.AprioriRules]");
            foreach (var rule in ruleEntityList)
                context.AprioriRules.Add(rule);
            context.SaveChanges();
        }
        public void FillDatabase(List<EclatSet> setList)
        {
            foreach (var set in setList)
                context.EclatSets.Add(set);
            context.SaveChanges();
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
            char[] charsToTrim = { ' ', '}', '{', '"', '/' };
            for (int i = 0; i < splitted.Count; i++)
            {
                splitted[i] = splitted[i].Replace(@"\", "");
                splitted[i] = splitted[i].Trim(charsToTrim);
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

        public static List<string> ParseInputForRecommend(string input)
        {
            var parsedInput = input.Split(new char[] { ',' }, StringSplitOptions.None).ToList();
            for (int i = 0; i < parsedInput.Count; i++)
            {
                parsedInput[i] = parsedInput[i].Trim(' ');
            }
            return parsedInput;
        }

        public static List<AprioriRule> Top(string type)
        {
            
            var context = new JobSkillsContext();
            var dbRules = context.AprioriRules;
            var rules = new List<AprioriRule>();

            switch (type)
            {
                case "conf":
                    rules= (from t in dbRules
                        orderby t.Confidence
                        select t).Take(30).ToList();
                    break;
                case "supp":
                    rules = (from t in dbRules
                        orderby t.Support
                        select t).Take(30).ToList();
                    
                    break;
                case "lift":
                    rules = (from t in dbRules
                        orderby t.Lift
                        select t).Take(30).ToList();
                   
                    break;
            }
            rules.Reverse();
            return rules;
        }

        /// <summary>
        /// Generate recomendation
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string Recomend(string input)
        {
            //final recomendation to the Console formatting in this method
            var rec = "";

            var parsedInput = ParseInputForRecommend(input);

            var context = new JobSkillsContext();
            var dbApriori = context.AprioriRules;
            var dbEclat = context.EclatSets;

            //contains intersections
            var tupleSet = new List<Tuple<double, string>>();
            var recomendedTupleSet = new List<Tuple<double, string>>();

            //initialized in 2nd part
            int counterMin;
            int counterEntries;

            foreach (var set in dbEclat)
            {
                var parsedSet = ParseInputForRecommend(set.ItemSet);
                var checkIntersection = parsedSet.Intersect(parsedInput).Any();
                
                if (checkIntersection)
                {
                    var support = set.Support;

                    tupleSet.Add(
                        new Tuple<double, string>(support, set.ItemSet)
                        );

                    var recommendedItems = parsedSet.Except(parsedInput).ToList();
                    recomendedTupleSet.Add(
                        new Tuple<double, string>(support, set.ItemSet));
                }

                //sort recomendation
                var sortedTupleRecomendationList = recomendedTupleSet.OrderBy(i => i.Item1).ToList();
            }
            
            return rec;
        }
    }
}