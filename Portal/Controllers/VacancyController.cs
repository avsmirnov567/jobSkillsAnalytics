using JobSkillsDb.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;

namespace Portal.Controllers
{
    public class VacancyController : Controller
    {
        public ActionResult Index(int page = 1, int pageSize = 50)
        {
            using (JobSkillsContext db = new JobSkillsContext())
            {
                var vacancies = db.Vacancies.Include(v=>v.Skills)
                    .OrderBy(v=>v.Id)
                    .ToPagedList(page, pageSize);
                return View(vacancies);
            }
        }
    }
}
