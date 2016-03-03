using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;

namespace Parser
{
    class CsvVacancyExporter
    {
        private List<Vacancy> vacancies;

        public CsvVacancyExporter(List<Vacancy> vacancies)
        {
            this.vacancies = vacancies;
        }

        public void ExportVacancies(string fileName = "")
        {
            string name = String.IsNullOrEmpty(fileName) ? "vacancies_" + Guid.NewGuid().ToString() + ".csv" : fileName;
            using (StreamWriter writer = new StreamWriter(name))
            {
                var csvWriter = new CsvWriter(writer);
                csvWriter.Configuration.RegisterClassMap<VacancyCsvMapping>();
                csvWriter.WriteRecords(vacancies);
            }
        }

        public void ExportSkills(string fileName = "")
        {
            string name = String.IsNullOrEmpty(fileName) ? "skills_" + Guid.NewGuid().ToString() + ".csv" : fileName;
            using (StreamWriter writer = new StreamWriter(name))
            {
                var csvWriter = new CsvWriter(writer);
                csvWriter.WriteField("vacancy_id");
                csvWriter.WriteField("vacancy_link");
                csvWriter.WriteField("skill");                
                foreach (var vacancy in vacancies)
                {                    
                    foreach (var skill in vacancy.Skills)
                    {
                        csvWriter.NextRecord();
                        csvWriter.WriteField(vacancy.IDfromSite);
                        csvWriter.WriteField(vacancy.Link);
                        csvWriter.WriteField(skill);
                    }
                }
            }
        }
    }
}
