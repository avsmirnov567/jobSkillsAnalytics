using System;
using System.Collections.Generic;

namespace Parser
{
    public class VacancyView
    {
        public string InnerId { get; set; }
        public string Title{ get; set; }
        public string Salary { get; set; }
        public string Currency { get; set; }
        public string ContentText { get; set; }
        public string ContentHtml { get; set; }
        public string Link { get; set; }
        public string Employer { get; set; }
        public DateTime? PublishingDate { get; set; }
        public List<string> Skills { get; set; }

        public VacancyView()
        {
            InnerId = "";
            Title = "";
            Salary = "";
            ContentText = "";
            Link = "";
            Employer = "";
            PublishingDate = DateTime.Now;
            Skills = new List<string>();
        }                
    }
}
