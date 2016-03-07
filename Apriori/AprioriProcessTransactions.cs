using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JobSkillsDb.Entities;

namespace Apriori
{
    class AprioriProcessTransactions
    {
        public AprioriProcessTransactions(double minsupport, double minconfidence, IEnumerable<Skill> skills, IEnumerable<Vacancy> vacancies)
        {
            //JobSkillsContext databaseContext = new JobSkillsContext();
            var databaseContext = new JobSkillsContext();
            var vacanciesAmount = databaseContext.Vacancies.Count();

            var processingClass = new AprioriImplementation();

            Debug.Assert(vacancies != null, "vacancies != null");
            Debug.Assert(skills != null, "skills != null");

            var frequentItems = processingClass.GetL1FrequentItems(minsupport, skills, vacancies);
            var allFrequentItems = frequentItems.ToList();

            IDictionary<Skill, double> candidates;

            do
            {
                Debug.Assert(vacancies != null, "vacancies != null");

                candidates = processingClass.GenerateCandidates(frequentItems, vacancies);
                allFrequentItems.AddRange(frequentItems);

            } while (candidates.Count != 0);

            var rules = processingClass.GenerateRules(allFrequentItems);
            var strongRules = processingClass.GetStrongRules(minconfidence, rules, allFrequentItems);
            var closedItemsets = processingClass.GetClosedItemsSets(allFrequentItems);
            var maximalItemSets = processingClass.GetMaximalItemSets(closedItemsets);

            return new Output
            {
                null;
            }
        }


    }
}