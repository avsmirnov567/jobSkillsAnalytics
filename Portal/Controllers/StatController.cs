using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobSkillsDb.Entities;

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
    }
}