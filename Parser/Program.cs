using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CsvHelper;
using JobSkillsDb.Entities;

namespace Parser
{
    class Program
    {
        public static void AddVacancy(VacancyView vacancyView)
        {
            using (JobSkillsContext db = new JobSkillsContext())
            {
                Vacancy vacancy = new Vacancy();
                vacancy.InnerId = vacancyView.InnerId;
                vacancy.Link = vacancyView.Link;
                vacancy.Title = vacancyView.Title;
                vacancy.Employer = vacancyView.Employer;
                vacancy.Date = vacancyView.PublishingDate?? DateTime.Now; //add current date if date is null
                vacancy.ContentText = vacancyView.ContentText;
                vacancy.ContentHtml = vacancyView.ContentHtml;
                vacancy.Currency = vacancyView.Currency;
                try
                {
                    var salaryStrings = Regex.Matches(vacancyView.Salary, @"\d+")
                        .Cast<Match>()
                        .Select(m => m.Value)
                        .ToList();
                    if (salaryStrings.Count > 0)
                    {
                        if (salaryStrings.Count == 2)
                        {
                            vacancy.SalaryFrom = int.Parse(salaryStrings[0]);
                            vacancy.SalaryTo = int.Parse(salaryStrings[1]);
                        }
                        else
                        {
                            int salary = int.Parse(salaryStrings.First());
                            vacancy.SalaryFrom = salary;
                            vacancy.SalaryTo = salary;
                        }
                    }
                    else
                    {
                        throw new Exception("Нет з/п");
                    }
                }
                catch (Exception e)
                {
                    vacancy.SalaryFrom = null;
                    vacancy.SalaryTo = null;
                }
                foreach (string skillName in vacancyView.Skills)
                {
                    Skill skill = db.Skills.Single(s => s.Name == skillName) ?? new Skill() {Name = skillName};
                    vacancy.Skills.Add(skill);                    
                }
                db.SaveChanges();
            }
        }

        static void Main(string[] args)
        {
            VacancyParserBase test = new HeadHunterParser();
            List<VacancyParserBase> adapters = new List<VacancyParserBase>();
            adapters.Add(new MoiKrugParser());
            adapters.Add(new HeadHunterParser());
            List<VacancyView> views = new List<VacancyView>();

            foreach (VacancyParserBase adapter in adapters)
            {
                var links = test.GetLinks();
                var dictParallel = test.ParseAll(links);
                views.AddRange(dictParallel.Keys);
            }
            Parallel.ForEach(views, (x) =>
            {
                AddVacancy(x);
            });
            Console.ReadKey();
        }
    }
}
