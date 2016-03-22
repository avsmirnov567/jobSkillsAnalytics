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

    public class CsvRow : List<string>
    {
        public string LineText { get; set; }
    }

    /// <summary>
    /// Class to write data to a CSV file
    /// </summary>
    public class CsvFileWriter : StreamWriter
    {
        public CsvFileWriter(Stream stream)
            : base(stream)
        {
        }

        public CsvFileWriter(string filename)
            : base(filename)
        {
        }

        /// <summary>
        /// Writes a single row to a CSV file.
        /// </summary>
        /// <param name="row">The row to be written</param>
        public void WriteRow(CsvRow row)
        {
            StringBuilder builder = new StringBuilder();
            bool firstColumn = true;
            foreach (string value in row)
            {
                // Add separator if this isn't the first value
                if (!firstColumn)
                    builder.Append(',');
                // Implement special handling for values that contain comma or quote
                // Enclose in quotes and double up any double quotes
                if (value.IndexOfAny(new char[] { '"', ',' }) != -1)
                    builder.AppendFormat("\"{0}\"", value.Replace("\"", "\"\""));
                else
                    builder.Append(value);
                firstColumn = false;
            }
            row.LineText = builder.ToString();
            WriteLine(row.LineText);
        }
    }

    /// <summary>
    /// Class to read data from a CSV file
    /// </summary>
    public class CsvFileReader : StreamReader
    {
        public CsvFileReader(Stream stream)
            : base(stream)
        {
        }

        public CsvFileReader(string filename)
            : base(filename)
        {
        }

        /// <summary>
        /// Reads a row of data from a CSV file
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public bool ReadRow(CsvRow row)
        {
            row.LineText = ReadLine();
            if (String.IsNullOrEmpty(row.LineText))
                return false;

            int pos = 0;
            int rows = 0;

            while (pos < row.LineText.Length)
            {
                string value;

                // Special handling for quoted field
                if (row.LineText[pos] == '"')
                {
                    // Skip initial quote
                    pos++;

                    // Parse quoted value
                    int start = pos;
                    while (pos < row.LineText.Length)
                    {
                        // Test for quote character
                        if (row.LineText[pos] == '"')
                        {
                            // Found one
                            pos++;

                            // If two quotes together, keep one
                            // Otherwise, indicates end of value
                            if (pos >= row.LineText.Length || row.LineText[pos] != '"')
                            {
                                pos--;
                                break;
                            }
                        }
                        pos++;
                    }
                    value = row.LineText.Substring(start, pos - start);
                    value = value.Replace("\"\"", "\"");
                }
                else
                {
                    // Parse unquoted value
                    int start = pos;
                    while (pos < row.LineText.Length && row.LineText[pos] != ',')
                        pos++;
                    value = row.LineText.Substring(start, pos - start);
                }

                // Add field to list
                if (rows < row.Count)
                    row[rows] = value;
                else
                    row.Add(value);
                rows++;

                // Eat up to and including next comma
                while (pos < row.LineText.Length && row.LineText[pos] != ',')
                    pos++;
                if (pos < row.LineText.Length)
                    pos++;
            }
            // Delete any unused items
            while (row.Count > rows)
                row.RemoveAt(rows);

            // Return true if any columns read
            return (row.Count > 0);
        }
    }



    class DbCsvHandler
    {
        struct AprioriRecord
        {
            string rules;
            double support;
            double confidence;
            double lift;
        }

        struct EclatRecord
        {
            string frequentItems;
            string support;
        }

        private double sup;
        private double conf;
        private JobSkillsContext context;

        public DbCsvHandler(double sup, double conf, JobSkillsContext context)
        {
            this.sup = sup;
            this.conf = conf;
            this.context = context;
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
        /// Read rules from APRIORI.csv or ECLAT.csv
        /// </summary>
        /// <param name="file"></param>
        public List<AprioriRule> GetDataFromAprioriRulesCsv(string rules_file)
        {
            this.context = new JobSkillsContext();

            var csv = File.ReadAllLines(rules_file).Select(a => a.Split('/').ToList()).ToList();
            csv.RemoveAt(0); //remove headers
            
            var ruleEntityList = new List<AprioriRule>();

            foreach (var row in csv)
            {
                var rules = row.ElementAt(0);
                var lhsrhs = LHSRHSStringsGeneratorFromApriori(rules);
                var rule = new AprioriRule
                {
                    Rules = row.ElementAt(0),
                    Support = double.Parse(row.ElementAt(1), NumberStyles.AllowDecimalPoint),    //{...} => {...}      
                    Confidence = double.Parse(row.ElementAt(2), NumberStyles.AllowDecimalPoint), //0.0000000000
                    Lift = double.Parse(row.ElementAt(3), NumberStyles.AllowDecimalPoint)        //0.0000000000
                };                                                                              
                
                

            }
            
            //rules, support, confidence, lift
            return null;
        }

        

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
            char[] charsToTrim = {'}', '{', '"', '/'}; 
            for (int i = 0; i < splitted.Count; i++)
            {
                splitted[i] = splitted[i].Replace(@"\", "");
                splitted[i] = splitted[i].Trim(charsToTrim);
            }
            return splitted;
        }

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