using System.Collections.Generic;
using JobSkillsDb.Entities;

namespace Apriori
{
    interface IApriori
    {
        Output ProcessTransation(decimal minsupport, decimal minconfidence, List<Skill> givenSkills,
            List<Vacancy> vacancies);
    }
}