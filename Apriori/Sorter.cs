using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using JobSkillsDb.Entities;

namespace Apriori
{
    public class SkillSorter : ISorter
    {
        public IList<Skill> Sort(ICollection<Skill> skillList)
        {
            var sortedSkillList = skillList.OrderBy(q => q.Name).ToList();
            return sortedSkillList;
        }
    }

    internal interface ISorter
    {
        IList<Skill> Sort(ICollection<Skill> skillList);
    }
}