using System;
using System.Collections.Generic;

namespace Parser
{
    class Vacancy
    {
        public int IDfromSite { get; set; }
        public string Name{ get; set; }
        public string Salary { get; set; }
        public string Content { get; set; }
        public string Link { get; set; }
        public string Publisher { get; set; }
        public DateTime PublishingDate { get; set; }
        public List<string> Skills { get; set; }

        public Vacancy()
        {
            IDfromSite = 0;
            Name = "";
            Salary = "";
            Content = "";
            Link = "";
            Publisher = "";
            PublishingDate = DateTime.Now;
            Skills = new List<string>();
        }
        public Vacancy(List<string> skills, string name, string content, string link, string publisher, DateTime publDate, int idFromSite, string salary)
        {
            IDfromSite = idFromSite;
            Name = name;
            Salary = salary;
            Content = content;
            Link = link;
            Publisher = publisher;
            PublishingDate = publDate;
            Skills = skills;
        }
    }
}
