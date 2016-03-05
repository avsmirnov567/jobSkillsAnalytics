using System;
using System.Collections.Generic;
using System.Data.Entity;
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
                Vacancy vacancy = db.Vacancies.SingleOrDefault(v => v.Link == vacancyView.Link) ?? new Vacancy();
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
                            throw new Exception("Нет з/п");
                        }
                    }
                    catch (Exception e)
                    {
                        vacancy.SalaryFrom = null;
                        vacancy.SalaryTo = null;
                    }
                    if (vacancyView.IsValid())
                    {
                        foreach (string skillName in vacancyView.Skills)
                        {
                            var skill = db.Skills.SingleOrDefault(s => s.Name == skillName);
                            if (skill == null)
                            {
                                skill = new Skill();
                                skill.Name = skillName;
                            }
                            vacancy.Skills.Add(skill);
                        }
                        var row = db.Vacancies.SingleOrDefault(v => v.Link == vacancyView.Link);
                        if (row != null)
                        {
                            db.Entry(row).CurrentValues.SetValues(vacancy);
                        }
                        else
                        {
                            db.Vacancies.Add(vacancy);
                        }
                        db.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("Битая ссылка:"+vacancyView.Link);
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            using (JobSkillsContext db = new JobSkillsContext())
            {
                db.Vacancies.RemoveRange(db.Vacancies);
                db.Skills.RemoveRange(db.Skills);
                db.SaveChanges();
            }
            List<VacancyParserBase> adapters = new List<VacancyParserBase>();
            adapters.Add(new MoiKrugParser());
            //adapters.Add(new HeadHunterParser());
            List<VacancyView> views = new List<VacancyView>();

            foreach (VacancyParserBase adapter in adapters)
            {
                var links = adapter.GetLinks();
                var dictParallel = adapter.ParseAll(links);
                views.AddRange(dictParallel.Keys);
            }
            List<string> errors = new List<string>();
            views.ForEach(v =>
            {
                try
                {
                    AddVacancy(v);
                }
                catch (Exception e)
                {
                    errors.Add(e.Message);
                }                
            });
            using (StreamWriter sw = new StreamWriter("log-"+Guid.NewGuid()+".txt"))
            {
                errors.ForEach(e => sw.WriteLine(e));
            }
        }
    }
}
