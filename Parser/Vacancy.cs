using System;
using System.Collections.Generic;

namespace Parser
{
    public class Vacancy
    {
        public string IDfromSite { get; set; }
        public string Title{ get; set; }
        public string Salary { get; set; }
        public string ContentText { get; set; }
        public string ContentHtml { get; set; }
        public string Link { get; set; }
        public string Employer { get; set; }
        public DateTime? PublishingDate { get; set; }
        public List<string> Skills { get; set; }

        public Vacancy()
        {
            IDfromSite = "";
            Title = "";
            Salary = "";
            ContentText = "";
            Link = "";
            Employer = "";
            PublishingDate = DateTime.Now;
            Skills = new List<string>();
        }
        //public Vacancy(List<string> skills, string title, string content, string link, string employer, DateTime publDate, int idFromSite, string salary)
        //{
        //    IDfromSite = idFromSite;
        //    Title = name;
        //    Salary = salary;
        //    Content = content;
        //    Link = link;
        //    Employer = publisher;
        //    PublishingDate = publDate;
        //    Skills = skills;
        //}
    }
}
