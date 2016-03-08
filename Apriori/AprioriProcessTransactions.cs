using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JobSkillsDb.Entities;

namespace Apriori
{
    class AprioriProcessTransactions
    {
        public AprioriProcessTransactions(double minsupport, double minconfidence, IEnumerable<Skill> givenskillsSkills, IEnumerable<Vacancy> givenVacancies)
        {
            //need to connect earlier (in implementation and pass these 
            //parameters to the method
            var databaseContext = new JobSkillsContext();
            var vacanciesAmount = databaseContext.Vacancies.Count();

            var processingClass = new AprioriImplementation();

            Debug.Assert(givenVacancies != null, "vacancies != null");
            Debug.Assert(givenskillsSkills != null, "skills != null");

            //convert from ienumerable 
            //does anyone could be converted? 
            var vacancies = givenVacancies as IList<Vacancy> ?? givenVacancies.ToList();

            //get list of skills and their supports
            var frequentSkills = processingClass.GetL1FrequentItems(minsupport, givenskillsSkills, vacancies);

            //transform list of skills into a list of AprioriSkillset that contains 
            //ICollection<Skill>
            List<AprioriSkillSet> frequentItems = DividedIntoSkillsetsFrequentItems(frequentSkills, out frequentItems);
            
            var allFrequentItems = new List<AprioriSkillSet>();

            //add AprioriSkillsets with a single skill in ICollection<Skill>
            allFrequentItems.AddRange(frequentItems);


            IList<AprioriSkillSet> candidates;

            //fun begins
            do
            {
                Debug.Assert(givenVacancies != null, "vacancies != null");
                
                IList<AprioriSkillSet> skillset = new List<AprioriSkillSet>();

                candidates = processingClass.GenerateCandidates(skillset, vacancies);
                frequentItems = new List<AprioriSkillSet>();
                frequentItems = processingClass.GetFrequentSkills(candidates, minsupport, vacanciesAmount);
                
                allFrequentItems.AddRange(frequentItems);

            } while (candidates.Count != 0);

            //var rules = processingClass.GenerateRules(allFrequentItems);
            //var strongRules = processingClass.GetStrongRules(minconfidence, rules, allFrequentItems);
            //var closedItemsets = processingClass.GetClosedItemsSets(allFrequentItems);
            //var maximalItemSets = processingClass.GetMaximalItemSets(closedItemsets);

        }

        //convert List<Skill> into List<AprioriSkillset> that contains ICollection<Skill>
        private static List<AprioriSkillSet> DividedIntoSkillsetsFrequentItems(IEnumerable<Skill> frequentSkills, out List<AprioriSkillSet> frequentItems)
        {

            var dividedFrequentItems = new List<AprioriSkillSet>();
            foreach (var skill in frequentSkills)
            {
                var tempSkillset = new AprioriSkillSet
                {
                    Id = skill.Id,
                    Support = skill.Support,
                    Skills = new List<Skill> {skill}
                };

                dividedFrequentItems.Add(tempSkillset);
            }

            frequentItems = dividedFrequentItems;
            return dividedFrequentItems;
        }
    }
}