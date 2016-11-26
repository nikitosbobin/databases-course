using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Models
{
    [Table("patientsaccounting")]
    public class PatientAccount
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("department")]
        public int DepartmentId { get; set; }

        [Column("patient")]
        public int PatientId { get; set; }

        [Column("complexity")]
        public int ComplexityId { get; set; }

        [Column("disease")]
        public int DiseaseId { get; set; }

        [Column("dateStart")]
        public DateTime? DateStart { get; set; }

        [Column("dateEnd")]
        public DateTime? DateEnd { get; set; }

        public Disease Disease { get; set; }
        public Complexity Complexity { get; set; }
        public Patient Patient { get; set; }
        public Department Department { get; set; }

        public bool HasFiltered(string filter)
        {
            var disease = Disease?.Title.ToLower();
            var complexity = Complexity?.Title.ToLower();
            var department = Department?.Name.ToLower();
            var dateStart = DateStart?.ToString("yyyy MMMM dd").ToLower();
            var dateEnd = DateEnd?.ToString("yyyy MMMM dd").ToLower();

            return ((disease != null) && filter.Contains(disease)) ||
                   ((complexity != null) && filter.Contains(complexity)) ||
                   ((department != null) && filter.Contains(department)) ||
                   ((dateStart != null) && filter.Contains(dateStart)) ||
                   ((dateEnd != null) && filter.Contains(dateEnd)) ||
                   (disease?.Contains(filter) ?? false) ||
                   (complexity?.Contains(filter) ?? false) ||
                   (department?.Contains(filter) ?? false) ||
                   (dateStart?.Contains(filter) ?? false) ||
                   (dateEnd?.Contains(filter) ?? false);
        }
    }
}