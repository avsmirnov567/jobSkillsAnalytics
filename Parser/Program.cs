using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CsvHelper;
using JobSkillsDb.Entities;
using System.Diagnostics;

namespace Parser
{
    class Program
    {

        public static void AddNewSkills(List<Skill> skills)
        {
            using(JobSkillsContext db = new JobSkillsContext())
            {
                var existingSkills = db.Skills.ToList();
                var newSkills = skills.Where(s => !existingSkills.Contains(s));
                Debug.WriteLine("New skills: " + newSkills.Count());
                db.Skills.AddRange(newSkills);
                db.SaveChanges();
            }
        }

        public static void AddVacancy(VacancyView vacancyView)
        {
            using (JobSkillsContext db = new JobSkillsContext())
            {
                Vacancy vacancy = db.Vacancies.SingleOrDefault(v => v.Link == vacancyView.Link) ?? new Vacancy();
                Debug.WriteLineIf(vacancy.Id > 0, "Vacancy exists: "+vacancy.Link);
                if (vacancy.Id == 0) //I desided not to update records
                {                    
                    vacancy.InnerNumber = vacancyView.InnerId;
                    vacancy.Link = vacancyView.Link;
                    vacancy.Title = vacancyView.Title;
                    vacancy.Employer = vacancyView.Employer;
                    vacancy.Date = vacancyView.PublishingDate ?? DateTime.Now; //add current date if date is null
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
                            throw new Exception("No salary");
                        }
                    }
                    catch (Exception)
                    {
                        vacancy.SalaryFrom = null;
                        vacancy.SalaryTo = null;
                    }
                    if (vacancyView.IsValid())
                    {
                        foreach (string skillName in vacancyView.Skills)
                        {
                            var skill = db.Skills
                                .SingleOrDefault(s => s.Name == skillName) ?? 
                                new Skill() { Name = skillName };
                            vacancy.Skills.Add(skill);
                        }
                        var row = db.Vacancies.SingleOrDefault(v => v.Link == vacancyView.Link);
                        if (row != null)
                        {
                            Debug.WriteLine("Update vacancy: "+vacancy.Link);
                            db.Entry(row).CurrentValues.SetValues(vacancy);
                        }
                        else
                        {
                            Debug.WriteLine("Add vacancy: " + vacancy.Link);
                            db.Vacancies.Add(vacancy);
                        }
                        Debug.WriteLine("Started saving vacancy to database");
                        db.SaveChanges();
                        Debug.WriteLine("Finished saving vacancy to database");
                    }
                    else
                    {                        
                        throw new Exception("Link is corrupt: "+vacancyView.Link);
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            List<string> existingLinks = new List<string>();
            using (JobSkillsContext db = new JobSkillsContext())
            {
                existingLinks.AddRange(db.Vacancies.Select(v => v.Link).ToList());
            }
            List<VacancyParserBase> adapters = new List<VacancyParserBase>();
            adapters.Add(new MoiKrugParser());
            adapters.Add(new HeadHunterParser());
            List<VacancyView> views = new List<VacancyView>();

            foreach (VacancyParserBase adapter in adapters)
            {
                Debug.WriteLine("Started parsing links: "+adapter.GetType().ToString());
                var newLinks = adapter.GetAllLinks().Where(l => !existingLinks.Contains(l)).ToList();
                Debug.WriteLine("New links: " + newLinks.Count + " item(s)");
                Debug.WriteLine("Started parsing vacancies: " + adapter.GetType().ToString());
                var dictParallel = adapter.ParseAll(newLinks);
                Debug.WriteLine("Parsed vacancies: " + dictParallel.Count + " item(s)");
                views.AddRange(dictParallel.Keys);
            }
            Debug.WriteLine("Total number of vacanies: " + views.Count + " item(s)");
            Debug.WriteLine("Started saving vacancies to database");
            List<string> errors = new List<string>();
            views.ForEach(v =>
            {
                try
                {
                    Debug.WriteLine("Started adding vacancy: " + v.Link);
                    AddVacancy(v);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    errors.Add(e.Message);
                }                
            });
            using(JobSkillsContext db = new JobSkillsContext())
            {
                int countVacancies = db.Vacancies.Count();
                int countSkills = db.Skills.Count();
                Debug.WriteLine(string.Format("Now we have {0} vacancies and {1} skills",
                    countVacancies, countSkills));
            }
            using (StreamWriter sw = new StreamWriter("log-"+Guid.NewGuid()+".txt"))
            {
                errors.ForEach(e => sw.WriteLine(e));
            }
        }
    }
}
