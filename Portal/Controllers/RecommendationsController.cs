using JobSkillsDb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class RecommendationsController : Controller
    {
        // GET: Recomendation
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RecomendationList(string name)
        {
            using(JobSkillsContext db = new JobSkillsContext())
            {
                List<string> recommendations = db.EclatSets
                    .Where(s => s.ItemSet.Contains("," + name + ","))
                    .OrderByDescending(s => s.Support)
                    .Select(s => s.ItemSet)
                    .Take(20)
                    .ToList();
                List<string> list = new List<string>();
                foreach(string set in recommendations)
                {
                    string[] skills = set.ToLower()
                        .Trim(',')
                        .Split(',');
                    list.AddRange(skills.Except(list));
                }
                list.Remove(name.ToLower());
                list = list.Take(5).ToList();
                return PartialView(list);
            }
        }
    }
}