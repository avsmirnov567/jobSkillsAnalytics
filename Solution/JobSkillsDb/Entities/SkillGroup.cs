using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSkillsDb.Entities
{
    public class SkillsGroup
    {
        public SkillsGroup()
        {
            SkillToGroupConnections = new HashSet<SkillToGroupConnection>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<SkillToGroupConnection> SkillToGroupConnections { get; set; }
    }
}
