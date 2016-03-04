using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using JobSkillsDb;
using JobSkillsDb.Entities;


namespace Apriori
{
    [TestFixture]
    class TestAprioriImplementation
    {
        [Test]
        public void SubsettingTest()
        {
            var sk = new Skill {Name = "Test name"};
            var vac = new Vacancy();
            vac.Skills.Add(sk);

            Assert.Contains(sk, (ICollection)vac.Skills);
        }

        [Test]
        public void GettingSupportTest()
        {
            
        }

        [Test]
        public void GettingFrequentL1Itemsets()
        {
            
        }
    }
}