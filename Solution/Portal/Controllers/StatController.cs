using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobSkillsDb.Entities;
using System.Data.Entity;

namespace Portal.Controllers
{
    public class StatController : Controller
    {
        // GET: Stat
        public ActionResult Index()
        {
            using(JobSkillsContext db = new JobSkillsContext())
            {
                ViewBag.VacanciesCount = db.Vacancies.Count();
                ViewBag.SkillsCount = db.Skills.Count();
            }
            return View();
        }

        public ActionResult GetTop(int count = 10)
        {
            using (JobSkillsContext db = new JobSkillsContext())
            {
                Dictionary<string, int> topSkills = db.Skills
                    .OrderByDescending(s => s.Vacancies.Count)
                    .Take(count)
                    .Select(s => new { s.Name, Count = s.Vacancies.Count })
                    .ToDictionary(x => x.Name, x => x.Count);
                return PartialView(topSkills);
            }
        }
    }
}