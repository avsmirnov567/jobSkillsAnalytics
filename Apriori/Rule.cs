using System;
using System.Collections.Generic;
using JobSkillsDb.Entities;

namespace Apriori
{
    class Rule 
    {
        public AprioriSkillSet X { get; set; }
        public AprioriSkillSet Y { get; set; }
        public decimal Confidence { get; set; }


        public Rule(AprioriSkillSet combination, AprioriSkillSet remaining, decimal confidence)
        {
            this.X = combination;
            this.Y = remaining;
            this.Confidence = confidence;
        }
    }
}