using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSkillsDb.Entities
{
    public partial class AprioriRule
    {
        public int Id { get; set; }
        public string LeftHandSide { get; set; }
        public string RightHandSide { get; set; }
        public string Rules { get; set; }
        public double Support { get; set; }
        public double Confidence { get; set; }
        public double Lift { get; set; }
    }
}
