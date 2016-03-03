using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobSkillsDb.Entities;

namespace JobSkillsDb
{
    class Program
    {
        static void Main(string[] args)
        {
            JobSkillsContext context = new JobSkillsContext();
            var v = context.Vacancies.ToList();
        }
    }
}
