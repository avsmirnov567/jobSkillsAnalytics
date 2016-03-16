using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JobSkillsDb.Entities;

namespace Apriori
{
    public class AprioriProcessTransactions
    {
        public AprioriProcessTransactions(decimal minsupport, decimal minconfidence, IList<Skill> givenSkills, IList<Vacancy> vacancies)
        {
            //transform IList<Vacancy> to IList<AprioriSkillset>
            //var transactions = new List<AprioriSkillSet>();
            //Debug.Assert(vacancies.Count == 0, "shit vacancies parameter at apriori transaction");
            //foreach (var vac in vacancies)
            //{
            //    AprioriSkillSet vacancyTransformed = new AprioriSkillSet();
            //    vacancyTransformed.Skills = vac.Skills;
            //}
            

            //need to connect earlier (in implementation and pass these 
            //parameters to the method
            var databaseContext = new JobSkillsContext();
            var vacanciesAmount = databaseContext.Vacancies.Count();

            var processingClass = new AprioriImplementation();

            Debug.Assert(vacancies != null, "vacancies != null");
            Debug.Assert(givenSkills != null, "skills != null");

            //convert from ienumerable 
            //does anyone could be converted? 
            

            //get list of skills and their supports
            var frequentSkills = processingClass.GetL1FrequentSkills(minsupport, givenSkills, vacancies);

            //transform list of skills into a list of AprioriSkillset that contains 
            //ICollection<Skill>
            //TRANSFORM!!!!
            List<AprioriSkillSet> frequentItems = DividedIntoSkillsetsFrequentItems(frequentSkills, out frequentItems);
            
            var allFrequentItems = new List<AprioriSkillSet>();

            //add AprioriSkillsets with a single skill in ICollection<Skill>
            allFrequentItems.AddRange(frequentItems);


            IList<AprioriSkillSet> candidates;

            //fun begins
            do
            {
                IList<AprioriSkillSet> skillset = new List<AprioriSkillSet>();

                candidates = processingClass.GenerateCandidates(skillset, vacancies);
                frequentItems = processingClass.GetFrequentSkills(candidates, minsupport, vacanciesAmount);
                
                allFrequentItems.AddRange(frequentItems);

            } while (candidates.Count != 0);


            var rules = processingClass.GenerateRules(allFrequentItems);

            var strongRules = processingClass.GetStrongRules(minconfidence, rules, allFrequentItems);
            //var closedItemsets = processingClass.GetClosedItemsSets(allFrequentItems);
            //var maximalItemSets = processingClass.GetMaximalItemSets(closedItemsets);

        }

        //convert List<Skill> into List<AprioriSkillset> that contains ICollection<Skill>
        private static List<AprioriSkillSet> DividedIntoSkillsetsFrequentItems(IList<Skill> frequentSkills, out List<AprioriSkillSet> frequentItems)
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