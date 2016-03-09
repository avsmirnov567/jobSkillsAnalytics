using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobSkillsDb.Entities;

namespace Portal.Controllers
{
    public class TextMiningController : Controller
    {
        // GET: TextMining
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(int id = 0)
        {
            using(JobSkillsContext db = new JobSkillsContext())
            {
                Vacancy vacancy = id > 0 ? db.Vacancies.Find(id) : db.Vacancies.First();
                return View(vacancy);
            }
        }

    }
}