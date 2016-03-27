using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobSkillsDb.Entities;
using System.Data.Entity;
using TextMining.Handler;
using System.IO;
using System.Text.RegularExpressions;

namespace TextMining.ConsoleApp
{
    class Program
    {
        static Random rnd = new Random();        
        static string NormalizeText(string text)
        {
            return text.ToLower()
                .Replace("\t", " ")
                .Replace("\n", " ")
                .Replace("  ", "")
                .Replace("&quot;"," ")
                .Trim();
        }

        public Dictionary<Skill, int> GetFrequency(List<List<Skill>> skillsTransactions)
        {
            Dictionary<Skill, int> table = new Dictionary<Skill, int>();
            foreach(var set in skillsTransactions)
            {
                foreach(Skill s in set)
                {
                    if (table.ContainsKey(s))
                    {
                        table[s]++;
                    }
                    else
                    {
                        table.Add(s, 1);
                    }
                }
            }
            return table;
        }

        static void DownloadFiles(string dirPath)
        {
            using (JobSkillsContext db = new JobSkillsContext())
            {
                var contents = db.VacancyContents.Include(v=>v.Vacancy.Skills)
                    .Where(v=>v.Vacancy.Skills.Count>=2)
                    .Select(c => new { c.VacancyId, c.Text, Skills = c.Vacancy.Skills })
                    .ToList();
                var programContent = contents.Where(c => c.Skills
                    .Any(s => Regex.IsMatch(s.Name, "([A-z])+")))
                    .ToList();
                var nonProgramContent = contents.Where(c => c.Skills
                    .All(s => !Regex.IsMatch(s.Name, "([A-z])+")))
                    .ToList();
                int trainCountP = (int)Math.Round(programContent.Count * 0.7);
                int trainCountN = (int)Math.Round(nonProgramContent.Count * 0.7);
                var train = programContent.Take(trainCountP).ToList();
                var test = programContent.Skip(trainCountP).Take(programContent.Count - trainCountP).ToList();
                var info = Directory.CreateDirectory(dirPath);
                using(StreamWriter sw = new StreamWriter(dirPath + "\\all-content.txt"))
                {
                    foreach(var item in contents)
                    {
                        sw.WriteLine(NormalizeText(item.Text));
                    }
                }
                info = Directory.CreateDirectory(dirPath + "\\content-p\\train");
                foreach (var content in train)
                {
                    using (StreamWriter sw = new StreamWriter(info.FullName + "\\train-" + content.VacancyId + ".txt"))
                    {
                        sw.WriteLine(NormalizeText(content.Text));
                    }
                }
                info = Directory.CreateDirectory(dirPath + "\\content-p\\test");
                foreach (var content in test)
                {
                    using (StreamWriter sw = new StreamWriter(info.FullName + "\\test-" + content.VacancyId + ".txt"))
                    {
                        sw.WriteLine(NormalizeText(content.Text));
                    }
                }
                train = nonProgramContent.Take(trainCountN).ToList();
                test = nonProgramContent.Skip(trainCountN).Take(nonProgramContent.Count - trainCountN).ToList();
                info = Directory.CreateDirectory(dirPath + "\\content-n\\train");
                foreach (var content in train)
                {
                    using (StreamWriter sw = new StreamWriter(info.FullName + "\\train-" + content.VacancyId + ".txt"))
                    {
                        sw.WriteLine(NormalizeText(content.Text));
                    }
                }
                info = Directory.CreateDirectory(dirPath + "\\content-n\\test");
                foreach (var content in test)
                {
                    using (StreamWriter sw = new StreamWriter(info.FullName + "\\test-" + content.VacancyId + ".txt"))
                    {
                        sw.WriteLine(NormalizeText(content.Text));
                    }
                }
            }
        }
        static List<VacancyContent> GetContentFromFiles(string dirPath)
        {
            var filePaths = Directory.GetFiles(dirPath);
            List<VacancyContent> contents = new List<VacancyContent>();
            foreach(var path in filePaths)
            {
                using(StreamReader sr = new StreamReader(path))
                {
                    int id = Int32.Parse(sr.ReadLine());
                    string contentText = sr.ReadToEnd();
                    contents.Add(new VacancyContent()
                    {
                        VacancyId = id,
                        Text = contentText
                    });
                }                
            }
            return contents;
        }

        static void Main(string[] args)
        {
            DownloadFiles("Contents");
        }
    }
}
