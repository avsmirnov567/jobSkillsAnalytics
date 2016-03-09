using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using JobSkillsDb;
using JobSkillsDb.Entities;


namespace Apriori
{
    //internal class ItemDictionary : KeyedCollection<Vacancy, Skill>
    //{

    //}

     class AprioriImplementation
    {
        private ISorter _sorter;

        public List<Skill> GetL1FrequentItems(double minSupport, IList<Skill> skills,
            IList<Vacancy> vacancies)
        {
            var transactionsCount= vacancies.Count();
            var frequentItemsL1 = new List<Skill>();
           
            foreach (var skill in skills)
            {
                var support = GetSupport(skill, vacancies);

                if (support/transactionsCount >= minSupport)
                {
                    var newskill = skill;
                    newskill.Support = (decimal?)support;
                    frequentItemsL1.Add(newskill);
                }
            }
            
            return frequentItemsL1.OrderBy(i => i.Id).ToList();
        }

         public double GetSupport(Skill generatedCandidate, IList<Vacancy> vacancies)
        {
            double support = 0;

            foreach (var vac in vacancies)
            {
                foreach (var skill in vac.Skills)
                {
                    if (CheckIsSubset(generatedCandidate, vac))
                    {
                        support++;
                    }
                }
            }
            return support;
        }

         public double GetSupport(Skill generatedCandidate, IList<AprioriSkillSet> skillsets)
         {
             double support = 0;
             foreach (var skillset in skillsets)
             {
                 foreach (var skill in skillset.Skills)
                 {
                     if (CheckIsSubset(generatedCandidate, skillset))
                     {
                         support++;
                     }
                 }
             }
             return support;
         }

         public static bool CheckIsSubset(Skill generatedCandidate, Vacancy vac)
         {
             return vac.Skills.Any(skill => skill.Id == generatedCandidate.Id);
         }

         public static bool CheckIsSubset(Skill generatedCandidate, AprioriSkillSet skillset)
         {
                 return skillset.Skills.Any(skill => skill.Id == generatedCandidate.Id);
         }

        public IList<AprioriSkillSet> GenerateCandidates(IList<AprioriSkillSet> frequentSkills, IEnumerable<Vacancy> vacancies)
        {

            IList<AprioriSkillSet> candidates = new List<AprioriSkillSet>();

            for (var i = 0; i < frequentSkills.Count - 1; i ++)
            {
                var firstSkill = new AprioriSkillSet {Skills = _sorter.Sort(frequentSkills[i].Skills)};

                for (var j = i + 1; j < frequentSkills.Count; j++)
                {
                    var secondSkill = new AprioriSkillSet {Skills = _sorter.Sort(frequentSkills[j].Skills)};

                    var generatedCandidate = GenerateCandidate(firstSkill, secondSkill);
                }

            }
            return candidates;
        }

        public AprioriSkillSet GenerateCandidate(AprioriSkillSet firstSkill, AprioriSkillSet secondSkill)
        {
            var candidate = new AprioriSkillSet();

            foreach (var collectionElement in firstSkill.Skills)
            {
                candidate.Skills.Add(collectionElement);
            }
            foreach (var collectionElement in secondSkill.Skills)
            {
                candidate.Skills.Add(collectionElement);
            }

            return candidate;
        }

        public List<AprioriSkillSet> GetFrequentSkills(IList<AprioriSkillSet> candidates, double minSupport,
            double vacancyCount)
        {
            var frequentItems = new List<AprioriSkillSet>();

            foreach (var candidate in candidates)
            {
                var skillset = new AprioriSkillSet();
                Debug.Assert(candidate.Support == 0, "this shit is a fucking null");
                if (candidate.Support != null && (double) candidate.Support.Value/vacancyCount >= minSupport)
                {
                    frequentItems.Add(candidate);
                }

            }
            return frequentItems;
        }

        public HashSet<Rule> GenerateRules(List<Skill> allFrequentItems)
        {

            throw new System.NotImplementedException();
        }

        public IList<Rule> GetStrongRules(double minconfidence, HashSet<Rule> rules, List<Skill> allFrequentItems)
        {
            throw new System.NotImplementedException();
        }

        public AprioriSkillSet GetClosedItemsSets(List<Skill> allFrequentItems)
        {
            return null;
        }

        public AprioriSkillSet GetMaximalItemSets(Tuple<Skill, Skill> closedItemsets)
        {
            throw new NotImplementedException();
        }
    }
}