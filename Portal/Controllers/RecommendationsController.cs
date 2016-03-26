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

        //public ActionResult RecomendationList(string name)
        //{
        //    using(JobSkillsContext db = new JobSkillsContext())
        //    {
        //        List<string> recommendations = db.EclatSets
        //            .Where(s => s.ItemSet.Contains("," + name + ","))
        //            .OrderByDescending(s => s.Support)
        //            .Select(s => s.ItemSet)
        //            .Take(20)
        //            .ToList();
        //        List<string> list = new List<string>();
        //        foreach(string set in recommendations)
        //        {
        //            string[] skills = set.ToLower()
        //                .Trim(',')
        //                .Split(',');
        //            list.AddRange(skills.Except(list));
        //        }
        //        list.Remove(name.ToLower());
        //        list = list.Take(5).ToList();
        //        return PartialView(list);
        //    }
        //}

        public ActionResult RecomendationList(string name)
        {

            //final recomendation to the Console formatting in this method

            var parsedInput = ParseInputForRecommend(name);

            var context = new JobSkillsContext();
            var dbEclat = context.EclatSets;

            //contains intersections
            var tupleSet = new List<Tuple<double, string>>();
            var recomendedTupleSet = new List<Tuple<double, string>>();

            while (parsedInput.Count != 0)
            {
                foreach (var set in dbEclat)
                {
                    var templist = new List<string>();
                    var parsedSet = ParseInputForRecommend(set.ItemSet);

                    if (parsedSet[parsedSet.Count - 1] == "")
                    {

                        for (var i = 1; i < parsedSet.Count - 1; i++)
                        {
                            templist.Add(parsedSet[i]);
                        }
                        parsedSet = templist;
                    }

                    var checkIntersection = parsedInput.All(s => parsedSet.Contains(s));

                    if (!checkIntersection || parsedSet.Count <= parsedInput.Count) continue;
                    var support = set.Support;

                    tupleSet.Add(
                        new Tuple<double, string>(support, set.ItemSet)
                        );

                    var recommendedItemsTail = parsedSet.Except(parsedInput).ToList();
                    var recommendedString = string.Join(",", recommendedItemsTail);

                    recomendedTupleSet.Add(
                        new Tuple<double, string>(support, recommendedString));
                }
                if (parsedInput.Count != 0)
                    parsedInput.RemoveAt(parsedInput.Count - 1);

                if (recomendedTupleSet.Count > 100) break;
            }
            var sortedTupleRecomendationList = recomendedTupleSet.OrderBy(i => i.Item1).Take(10).ToList();

            var united = new List<string>();

            foreach (var a in sortedTupleRecomendationList)
            {
                var spl = a.Item2.Split(',');
                united.AddRange(spl);
            }

            var noDups = united.Distinct().ToList();

            return PartialView(noDups);
        }

        public static List<string> ParseInputForRecommend(string input)
        {
            var parsedInput = input.Split(new char[] { ',' }, StringSplitOptions.None).ToList();
            for (int i = 0; i < parsedInput.Count; i++)
            {
                parsedInput[i] = parsedInput[i].Trim(' ');
            }
            return parsedInput;
        }
    }
}