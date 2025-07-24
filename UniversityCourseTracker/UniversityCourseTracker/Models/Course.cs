using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C971App.Models
{
    public class Course
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int TermId { get; set; } //Foreign key from Term class/table
        public string Name { get; set; }
        public string Status { get; set; }
  
        public bool StartNotification { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Notes { get; set; }

        public string InstructorName { get; set; }
        public string InstructorPhone { get; set; }
        public string InstructorEmail { get; set; }

        public string PerformanceAssessmentName { get; set; }
        public DateTime PerformanceAssessmentStartDate { get; set; }
        public DateTime PerformanceAssessmentDueDate { get; set; }
        public bool PerformanceAssessmentNotification { get; set; }

        public string ObjectiveAssessmentName { get; set; }
        public DateTime ObjectiveAssessmentStartDate { get; set; }
        public DateTime ObjectiveAssessmentDueDate { get; set; }
        public bool ObjectiveAssessmentNotification { get; set; }

        //public int InStock { get; set; }
        //public decimal Price { get; set; }
    }
}
