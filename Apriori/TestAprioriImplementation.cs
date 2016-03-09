using System;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Messaging;
using JobSkillsDb;
using JobSkillsDb.Entities;


namespace Apriori
{
    [TestFixture]
    internal class TestAprioriImplementation
    {
        private decimal _minsupport;
        private decimal _minconfidence;
        private IList<Skill> _skills;
        private IList<Vacancy> _vacancies;
        private AprioriProcessTransactions contextProcesor;
        private AprioriImplementation implementation;
        
        //private readonly Apriori_Accessor _target;

        public TestAprioriImplementation()
        {
            implementation = new AprioriImplementation();
            
            _minsupport = (decimal).01;
            _minconfidence = (decimal).01;
            _vacancies = new List<Vacancy>();
            _skills = new List<Skill>();

            Skill skill1, skill2, skill3, skill4, skill5;
            initTestSkills(out skill1, out skill2, out skill3, out skill4, out skill5);

            var vacancy1 = new Vacancy
            {
                Id = 0,
                Title = "title0",
                Link = "link0",
                Skills = new List<Skill>(2) { skill1, skill2, skill3 }
            };
            var vacancy2 = new Vacancy
            {
                Id = 1,
                Title = "title1",
                Link = "link1",
                Skills = new List<Skill>(2) {skill2, skill1, skill4}
            };
            var vacancy3 = new Vacancy
            {
                Id = 2,
                Title = "title2",
                Link = "link2",
                Skills = new List<Skill>(2) {skill1, skill5, skill4}
            };
            
            skill1.Vacancies = new List<Vacancy> {vacancy1, vacancy2, vacancy3};
            skill2.Vacancies = new List<Vacancy> {vacancy1, vacancy2};
            skill3.Vacancies = new List<Vacancy> {vacancy1};
            skill4.Vacancies = new List<Vacancy> {vacancy2, vacancy3};
            skill5.Vacancies = new List<Vacancy> {vacancy3};

            _vacancies.Add(vacancy1);
            _vacancies.Add(vacancy2);
            _vacancies.Add(vacancy3);

            _skills.Add(skill1);
            _skills.Add(skill2);
            _skills.Add(skill3);
            _skills.Add(skill4);
            _skills.Add(skill5);

        }

        private void initTestSkills(out Skill skill1, out Skill skill2, out Skill skill3, out Skill skill4, out Skill skill5)
        {
            skill1 = new Skill
            {
                Id = 0,
                Name = "C#"
            };
            skill2 = new Skill
            {
                Id = 1,
                Name = "Backbonejs"
            };
            skill3 = new Skill
            {
                Id = 2,
                Name = "Javascript"
            };

            skill4 = new Skill
            {
                Id = 3,
                Name = "ES6"
            };

            skill5 = new Skill
            {
                Id = 4,
                Name = "AngularJS"
            };
        }

        #region Init methods for test data — OLD

        //private void FillTestVacanciesAndSkills(List<List<string>> listOfSkillsetInVacanciesList)
        //{
        //    int amount = 3;
        //    InitSkillsAndVacanciesLists(amount);

        //    for (var i = 0; i <= 2; i++)
        //    {
        //        var vacancy = InitVacancy(i);
        //        _vacancies.Add(vacancy);

        //        for (var vacancyIndex = 0; vacancyIndex <= 2; vacancyIndex++)
        //        {
        //            //_vacancies.ToList()[j].Skills = new List<Skill>();
        //            FillVacancyBySkills(listOfSkillsetInVacanciesList, vacancyIndex);
        //        }
        //    }
        //    FillVacancyCollectionInSkill();
        //}

        //private static Vacancy InitVacancy(int index)
        //{
        //    var vacancy = new Vacancy
        //    {
        //        Id = index,
        //        Title = "VacancyTitle#" + index,
        //        Link = "TestLink#" + index
        //    };
        //    return vacancy;
        //}

        //private void FillVacancyBySkills(List<List<string>> listOfSkillsetInVacanciesList, int vacancyIndex)
        //{
        //    for (var skillIndex = 0; skillIndex <= 2; skillIndex++)
        //    {
        //        var skill = InitSkill(listOfSkillsetInVacanciesList, skillIndex, vacancyIndex);
        //        _vacancies.ToList()[vacancyIndex].Skills.Add(skill);
        //    }
        //}

        //private static Skill InitSkill(List<List<string>> listOfSkillsetInVacanciesList, int skillIndex, int vacancyIndex)
        //{
        //    var skill = new Skill
        //    {
        //        Id = skillIndex,
        //        Name = listOfSkillsetInVacanciesList[vacancyIndex][skillIndex]
        //    };
        //    return skill;
        //}

        //private void InitSkillsAndVacanciesLists(int amount)
        //{
        //    _skills = new List<Skill>();
        //    _vacancies = new List<Vacancy>(amount);

        //    foreach (var vac in _vacancies)
        //        vac.Skills = new List<Skill>(amount);
        //}

        //private void FillVacancyCollectionInSkill()
        //{
        //    foreach (var vac in _vacancies)
        //        foreach (var skill in vac.Skills)
        //            skill.Vacancies = new List<Vacancy> {vac};
        //}
        #endregion 

        [Test]
        public void GetL1FrequentItemsTest()
        {
            implementation = new AprioriImplementation();
            var frequentItems = implementation.GetL1FrequentItems(_minsupport, _skills, _vacancies);

            Assert.AreEqual(5, frequentItems.Count());
        }

        [Test]
        public void AprioriProcessTransactionTest()
        {

        }

        [Test]
        public void SubsettingTest()
        {

        }

        [Test]
        public void GettingSupportTest()
        {
            //contextProcesor = new AprioriProcessTransactions(_minsupport, _minconfidence, _skills, _vacancies);
            var generatedCandidate = _vacancies[0].Skills.ElementAt(0); //C
            var t = implementation.GetSupport(generatedCandidate, _vacancies);

            Assert.AreEqual(2, t, "Couldn't find support value for " + generatedCandidate);
        }

        [Test]
        public void GettingFrequentL1Itemsets()
        {

        }

        [Test]
        public void GenerateCandidatesTest()
        {

        }

        [Test]
        public void GenerateCandidate_SameFirstElementTest()
        {

        }

        [Test]
        public void GenerateCandidate_SingleElementsTest()
        {
        }

        [Test]
        public void GenerateCandidate_DifferentFirstElementTest()
        {

        }

        [Test]
        public void GetFrequentItemsTest()
        {

        }

        [Test]
        public void GenerateSubsetsRecursiveTest()
        {

        }

        [Test]
        public void GenerateSubsetsTest()
        {

        }

        [Test]
        public void GetRemainingTest()
        {

        }

        [Test]
        public void GetClosedItemSetsTest()
        {

        }

        [Test]
        public void GetItemParentstest()
        {

        }

        [Test]
        public void CheckIsClosed_OpenItemTest()
        {

        }

        [Test]
        public void CheckIsClosed_ClosedItemTest()
        {

        }

        [Test]
        public void GenerateStrongRulesTest()
        {

        }

        [Test]
        public void GetStrongRulesTest()
        {

        }

    }
}