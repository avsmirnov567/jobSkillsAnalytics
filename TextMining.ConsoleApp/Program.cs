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
        static void DownloadFiles(string dirPath)
        {
            using (JobSkillsContext db = new JobSkillsContext())
            {
                var contents = db.VacancyContents.Select(c => new { c.VacancyId, c.Text }).ToList();
                foreach (var content in contents)
                {
                    var info = Directory.CreateDirectory(dirPath);
                    using (StreamWriter sw = new StreamWriter(info.FullName + "\\content-" + content.VacancyId + ".txt"))
                    {
                        sw.WriteLine(content.VacancyId);
                        sw.WriteLine(content.Text);
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
            //DownloadFiles("Contents");
            var cont = GetContentFromFiles("Contents");
            //var skills = SkillsAnalytics.GetFrequentSkills(0.01);
            //skills = skills.Where(s => !Regex.IsMatch(s.Name, "[А-я]+")).ToList();
        }
    }
}
