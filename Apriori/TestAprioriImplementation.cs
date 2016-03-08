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
        private double _minsupport;
        private double _minconfidence;
        private IList<Skill> _skills;
        private IList<Vacancy> _vacancies;
        private AprioriProcessTransactions contextProcesor;
        private AprioriImplementation implementation;
        
        //private readonly Apriori_Accessor _target;

        public TestAprioriImplementation()
        {
            implementation = new AprioriImplementation();
            
            _minsupport = .01;
            _minconfidence = .01;
            _vacancies = new List<Vacancy>(3);

            var listOfSkillsetInVacanciesList = new List<List<string>>();

            listOfSkillsetInVacanciesList.Add(new List<string> {"C#", "Backbonejs", "Javascript"});
            listOfSkillsetInVacanciesList.Add(new List<string> {"Javascript", "ES6", "AngularJS"});
            listOfSkillsetInVacanciesList.Add(new List<string> {"C#", "Javascript", "ES6"});

            FillTestVacanciesAndSkills(listOfSkillsetInVacanciesList);
        }

        private void FillTestVacanciesAndSkills(List<List<string>> listOfSkillsetInVacanciesList)
        {
            _vacancies = new List<Vacancy>();
            for (var i = 0; i < 2; i++)
            {
                for (var j = 0; j < 2; j++)
                {
                    var vacancy = new Vacancy
                    {
                        Id = j,
                        Title = "VacancyTitle#" + j,
                        Link = "TestLink#" + j
                    };
                    
                    _vacancies.Add(vacancy);
                    _vacancies.ToList()[j].Skills = new List<Skill>();
                    for (var skillItem = 0; skillItem < 2; skillItem++)
                    {
                        var skill = new Skill
                        {
                            Id = skillItem,
                            Name = listOfSkillsetInVacanciesList[j][skillItem]
                        };
                        _vacancies.ToList()[j].Skills.Add(skill);
                    }
                }
            }
        }

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