using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSkillsDb.Entities
{
    public class SkillToGroupConnection
    {
        public int Id { get; set; }

        public int SkillId { get; set; }

        public int GroupId { get; set; }

        public bool IsStandard { get; set; }

        public virtual Skill Skill { get; set; }

        public virtual SkillsGroup SkillGroup { get; set; }
    }
}
