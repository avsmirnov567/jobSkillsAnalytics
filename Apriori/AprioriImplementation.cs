using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
using JobSkillsDb;
using JobSkillsDb.Entities;
using NUnit.Framework.Constraints;


namespace Apriori
{
    //internal class ItemDictionary : KeyedCollection<Vacancy, Skill>
    //{

    //}


    class AprioriImplementation
    {
        private ISorter _sorter;

        public List<Skill> GetL1FrequentSkills(decimal minSupport, IList<Skill> skills,
            IList<Vacancy> vacancies)
        {
            int vacanciesCount = vacancies.Count();
            var frequentItemsL1 = new List<Skill>();
           
            foreach (var skill in skills)
            {
                skill.Support = (decimal)GetSupport(skill, vacancies);
                //skill.Support = GetSupport(skill, vacancies);

                var temp = skill.Support/vacanciesCount >= minSupport;
                if (temp)
                {
                    var newskill = skill;
                    newskill.Support = skill.Support;
                    frequentItemsL1.Add(newskill);
                }
            }
            
            return frequentItemsL1.OrderBy(i => i.Id).ToList();
        }

         
         public double GetSupport(Skill generatedCandidate, IList<Vacancy> vacancies)
         {
             var support = 0;

             foreach (var vac in vacancies)
             {
                 foreach (var skill in vac.Skills)
                 {
                     if (skill.Id == generatedCandidate.Id && skill.Name == generatedCandidate.Name)
                         support++;
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
                    if (skill.Id == generatedCandidate.Id)
                     {
                         skillset.Support++;
                     }
                 }
             }
             return support;
         }

         public double GetSupport(AprioriSkillSet skillset, IList<AprioriSkillSet> skillsets)
         {
             double support = 0;

             List<Skill> skills = skillset.Skills as List<Skill>;

             var howManySkills = skills.Count;
             int counter = 0;

             foreach (var skillsets_item in skillsets)
             {
                 //var set = skillsets_item.Skills as List<Skill>;
                 foreach (var set_element in skillsets_item.Skills)
                 {
                     for (var i = 0; i < skills.Count; i++)
                     {
                         if (set_element.Id == skills.ElementAt(i).Id)
                         {
                             counter++;
                         }
                     }
                 }
                 if (counter == skills.Count)
                 {
                     support++;
                     counter = 0;
                 }
             }

             return support;
         }

         public double GetSupport(AprioriSkillSet candidate, IList<Vacancy> vacancies)
         {
             var support = 0;
             var counter = 0;

             List<Skill> skills = candidate.Skills as List<Skill>;

             foreach (var vac in vacancies)
             {
                 var temp = vac.Skills as List<Skill>;
                 foreach (var skillOfVacancy in temp)
                 {
                     for (var i = 0; i < skills.Count; i++)
                     {
                         if (skillOfVacancy == skills.ElementAt(i))
                             counter++;
                     } 
                 }
                 if (counter == skills.Count)
                 {
                     counter = 0;
                     support++;
                 }
             }

             return support;
         }

         public static int CheckIsSubset(Skill generatedCandidate, Vacancy vac)
         {
             int yesno = 0;
             foreach (var skill in vac.Skills)
             {
                 if (skill.Id == generatedCandidate.Id && skill.Name == generatedCandidate.Name)
                 {
                     return yesno = 1;
                 }
             }

             return yesno = 0;
         }

         public static bool CheckIsSubset(Skill generatedCandidate, AprioriSkillSet skillset)
         {
                 return skillset.Skills.Any(skill => skill.Id == generatedCandidate.Id);
         }

        public IList<AprioriSkillSet> GenerateCandidates(IList<AprioriSkillSet> frequentSkills, IList<Vacancy> vacancies)
        {
            
            IList<AprioriSkillSet> candidates = new List<AprioriSkillSet>();

            for (var i = 0; i < frequentSkills.Count - 1; i ++)
            {
                var firstSkill = new AprioriSkillSet {Skills = _sorter.Sort(frequentSkills[i].Skills)};

                for (var j = i + 1; j < frequentSkills.Count; j++)
                {
                    var secondSkill = new AprioriSkillSet {Skills = _sorter.Sort(frequentSkills[j].Skills)};

                    var generatedCandidate = GenerateCandidate(firstSkill, secondSkill);

                    if (generatedCandidate.Skills.Count != 0)
                    {
                        var support = GetSupport(generatedCandidate, vacancies);
                        generatedCandidate.Support = (decimal)support;
                        candidates.Add(generatedCandidate);
                    }
                }
            }
            return candidates;
        }

         public IList<AprioriSkillSet> GenerateCandidates(IList<AprioriSkillSet> frequentSkills,
             IList<AprioriSkillSet> transactions)
         {
             IList<AprioriSkillSet> candidates = new List<AprioriSkillSet>();
            //for (var i =0; i < frequentSkills.Coun)
                return null;
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

        public List<AprioriSkillSet> GetFrequentSkills(IList<AprioriSkillSet> candidates, decimal? minSupport,
            int vacancyCount)
        {
            var frequentItems = new List<AprioriSkillSet>();

            foreach (var candidate in candidates)
            {
                var skillset = new AprioriSkillSet();

                Debug.Assert(candidate.Support == 0, "this shit is a fucking null");

                if (candidate.Support != null && candidate.Support/vacancyCount >= minSupport)
                {
                    frequentItems.Add(candidate);
                }

            }
            return frequentItems;
        }

        public HashSet<Rule> GenerateRules(List<AprioriSkillSet> allFrequentItems)
        {
            var rules = new HashSet<Rule>();

            foreach (var item in allFrequentItems)
            {
                if (item.Skills.Count > 1)
                {
                    var subsetList = GenerateSubsets(item);

                    foreach (var subset in subsetList)
                    {
                        //generate remaining tail
                        var remaining = GetRemaining(subset, item);

                        var rule = new Rule(subset, remaining, 0);

                        if (!rules.Contains(rule))
                        {
                            rules.Add(rule);
                        }
                    }
                }
            }
            return rules;
        }

        private AprioriSkillSet GetRemaining(AprioriSkillSet child, AprioriSkillSet parent)
        {
            var clearParent = new AprioriSkillSet {Skills = new List<Skill>(parent.Skills.Count - child.Skills.Count)};

            var parentSkills = parent.Skills;
            var childSkills = child.Skills;

            foreach (var pa in parentSkills)
            {
                foreach (var ch in childSkills.Where(ch => pa.Name != ch.Name))
                {
                    clearParent.Skills.Add(pa);
                }
            }

            return clearParent;
        }

        private List<AprioriSkillSet> GenerateSubsets(AprioriSkillSet itemOfAllFrequentItemsets)
         {
             var skills = itemOfAllFrequentItemsets.Skills.ToList();
             var allsubsets = new List<AprioriSkillSet>();

             int subsetLength = skills.Count/2;

             for (var i = 0; i < subsetLength; i++)
             {
                 //List<List<Skill>> subsets = new List<List<Skill>>();
                 List<List<Skill>> subsets = new List<List<Skill>>();
                 GenerateSubsetsRecursive(skills, i, new List<Skill>(skills.Count), subsets);

                 //convert from list of skill lists to allSubsets variable signature
                 foreach (var listElement in subsets)
                 {
                     var aprSkillset = new AprioriSkillSet {Skills = listElement};
                     allsubsets.Add(aprSkillset);
                 }
                
                //allsubsets.Add(subsets);
                
                //allsubsets = allsubsets.AddRange(subsets);
                //generate List of apriori skillsets with support of current item in previous (calling) method
             }
             return allsubsets;
         }

         private void GenerateSubsetsRecursive(List<Skill> skills, int subsetLength, List<Skill> temp, List<List<Skill>> subsets, int q = 0, int r = 0)
         {
             if (q == subsetLength)
             {
                var tempSkilllist = new List<Skill>();

                for (var j = 0; j < subsetLength; j++)
                 {
                     tempSkilllist.Add(temp.ElementAt(j));
                     //subsets.Add(temp.ElementAt(j))
                 }

                 subsets.Add(tempSkilllist);
             }
             else
             {
                 for (int i = 0; i < temp.Count; i++)
                 {
                     temp[q] = skills.ElementAt(i); //possible error
                     GenerateSubsetsRecursive(skills, subsetLength, temp, subsets, q + 1, i + 1);
                 }
             }
         }

         public List<Rule> GetStrongRules(decimal minconfidence, HashSet<Rule> rules, List<AprioriSkillSet> allFrequentItems)
         {
             List<Rule> strongRules = new List<Rule>();

             foreach (var rule in rules)
             {
                 var tempSkillset = new AprioriSkillSet();
                 var tempSkills = new List<Skill>();

                 tempSkills.AddRange(rule.X.Skills);
                 tempSkills.AddRange(rule.Y.Skills);
                 _sorter.Sort(tempSkills);
                 tempSkillset.Skills = tempSkills;

                 //var xy = new AprioriSkillset(new Skill_sorter.Sort(tempSkillset.Skills);
                 var xy = new AprioriSkillSet {Skills = tempSkills};
                 AddStrongRule(rule, xy, strongRules, minconfidence, allFrequentItems);
             }

             return strongRules;
         }

        private void AddStrongRule(Rule rule, AprioriSkillSet xy, List<Rule> strongRules, decimal minconfidence, List<AprioriSkillSet> allFrequentItems)
        {
            var confidence = GetConfidence(rule.X, xy, allFrequentItems);

            if (confidence >= minconfidence)
            {
                var newRule = new Rule(rule.X, rule.Y, confidence);
                strongRules.Add(newRule);
            }
            confidence = GetConfidence(rule.Y, xy, allFrequentItems);
            if (confidence >= minconfidence)
            {
                var newRule = new Rule(rule.Y, xy, confidence);
                strongRules.Add(newRule);
            }
        }

        private decimal GetConfidence(AprioriSkillSet x, AprioriSkillSet xy, List<AprioriSkillSet> allFrequentItems)
        {
            decimal xSupport = 0, xySupport = 0;
            List<Skill> getXskills = new List<Skill>();
            getXskills = x.Skills.ToList();

            List<Skill> getXYSkills = new List<Skill>();
            getXYSkills = xy.Skills.ToList();

            var returnedXSkillset = allFrequentItems.Where(t => t.Skills.Intersect(getXskills).Any()).ToList();
            if (returnedXSkillset.Count > 0)
            {
                //sorry
                if (returnedXSkillset.Count < 2)
                {
                    xSupport = (decimal)returnedXSkillset.ElementAt(0).Support;
                }
            }

            var returnedXYSkillset = allFrequentItems.Where(t => t.Skills.Intersect(getXYSkills).Any()).ToList();
            if (returnedXYSkillset.Count > 0)
            {
                //sorry
                if (returnedXYSkillset.Count < 2)
                {
                    xySupport = (decimal)returnedXYSkillset.ElementAt(0).Support;
                }
            }

            return xySupport/xSupport;
        }

        public AprioriSkillSet GetClosedItemsSets(List<AprioriSkillSet> allFrequentItems)
        {
            return null;
        }

        public AprioriSkillSet GetMaximalItemSets(AprioriSkillSet closedItemsets)
        {
            throw new NotImplementedException();
        }

        
    }
}