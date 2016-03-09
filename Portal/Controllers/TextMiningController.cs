using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobSkillsDb.Entities;
using System.Threading.Tasks;

namespace Portal.Controllers
{
    public class TextMiningController : Controller
    {
        // GET: TextMining
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Details(int id = 0)
        {
            using(JobSkillsContext db = new JobSkillsContext())
            {
                Vacancy vacancy = id > 0 ? await db.Vacancies.FindAsync(id) : db.Vacancies.First();
                return View(vacancy);
            }
        }

        public async Task<ActionResult> Edit(int id = 0)
        {
            using (JobSkillsContext db = new JobSkillsContext())
            {
                Vacancy vacancy = id > 0 ? await db.Vacancies.FindAsync(id) : db.Vacancies.First();
                return View(vacancy);
            }
        }
    }
}