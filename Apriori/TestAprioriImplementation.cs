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
        private readonly double _minsupport;
        private readonly double _minconfidence;
        private readonly IEnumerable<Skill> _itemSkills;
        private readonly IEnumerable<Vacancy> _transactionsVacancies;
        //private readonly Apriori_Accessor _target;

        public TestAprioriImplementation()
        {
            _minsupport = .5;
            _minconfidence = .8;
            _transactionsVacancies = new List<Vacancy>(3);

            var listOfSkillsetInVacanciesList = new List<List<string>>();

            listOfSkillsetInVacanciesList.Add(new List<string> {"C#", "Backbonejs", "Javascript"});
            listOfSkillsetInVacanciesList.Add(new List<string> {"Javascript", "ES6", "AngularJS"});
            listOfSkillsetInVacanciesList.Add(new List<string> {"C#", "Javascript", "ES6"});

            FillTestVacanciesAndSkills(listOfSkillsetInVacanciesList);
        }

        private void FillTestVacanciesAndSkills(List<List<string>> listOfSkillsetInVacanciesList)
        {
            for (var i = 0; i < 2; i++)
            {
                var vacancy = new Vacancy();
                for (var j = 0; j < 2; j++)
                {
                    _transactionsVacancies.ToList().Add(vacancy);
                    for (var skillItem = 0; skillItem < 2; skillItem++)
                    {
                        var skill = new Skill
                        {
                            Id = skillItem,
                            Name = listOfSkillsetInVacanciesList[j][skillItem]
                        };
                        _transactionsVacancies.ToList()[j].Skills.Add(skill);
                    }
                }
            }
        }

        [Test]
        public void GetL1FrequentItemsTest()
        {

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