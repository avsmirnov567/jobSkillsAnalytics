using System.Collections.Generic;
using JobSkillsDb.Entities;

namespace Apriori
{
    public class Output
    {
        public List<Rule> StrongRules { get; set; }
        public List<AprioriSkillSet> FrequentItems { get; set; }
    }
}