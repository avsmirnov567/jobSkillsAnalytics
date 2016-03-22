using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobSkillsDb.Entities;
using System.Data.Entity;
using TextMining.Handler;
using System.Text.RegularExpressions;

namespace TextMining.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var skills = SkillsAnalytics.GetFrequentSkills(0.01);
            skills = skills.Where(s => !Regex.IsMatch(s.Name, "[А-я]+")).ToList();
        }
    }
}
