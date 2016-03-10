using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobSkillsDb.Entities;
using System.Threading.Tasks;

namespace Portal.Controllers
{
    public class TextMiningController : Controller
    {
        public async Task<ActionResult> Details(int id = 0)
        {
            using(JobSkillsContext db = new JobSkillsContext())
            {
                //these requests can go parallel to each other
                Vacancy vacancy = id > 0 ? await db.Vacancies.FindAsync(id) : db.Vacancies.First();
                List<Skill> skills = await db.Skills.OrderBy(s=>s.Name).ToListAsync();
                List<MarkedZone> zones = await db.MarkedZones.Include(z => z.Skill).ToListAsync();
                ViewBag.SkillsList = skills;
                ViewBag.ZonesList = zones;
                return View(vacancy);
            }
        }

        public async Task<ActionResult> VacancyMarkedZones(int vacancyId)
        {
            using (JobSkillsContext db = new JobSkillsContext())
            {
                List<MarkedZone> zones =
                    await db.MarkedZones.Include(z => z.Skill).Where(z => z.Vacancy.Id == vacancyId).ToListAsync();
                return PartialView(zones);
            }
        }
    }
}