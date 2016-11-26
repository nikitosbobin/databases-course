using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Models
{
    [Table("patients")]
    public class Patient
    {
        public Patient()
        {
            PatientAccount = new List<PatientAccount>();
        }

        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("surname")]
        public string Surname { get; set; }

        [Column("second_name")]
        public string SecondName { get; set; }

        [Column("adress")]
        public string Adress { get; set; }

        [Column("birth")]
        public DateTime? Birth { get; set; }

        [Column("district")]
        public int DistrictId { get; set; }

        public District District { get; set; }
        public ICollection<PatientAccount> PatientAccount { get; set; }

        public bool HasFiltered(string filter)
        {
            var name = Name.ToLower();
            var surname = Surname.ToLower();
            var secondName = SecondName.ToLower();
            var adress = Adress.ToLower();
            var birth = Birth?.ToString("yyyy-MM-dd");
            var district = District?.Title.ToLower();

            return filter.Contains(name) ||
                   filter.Contains(surname) ||
                   filter.Contains(secondName) ||
                   filter.Contains(adress) ||
                   (birth != null && filter.Contains(birth)) ||
                   (district != null && filter.Contains(district)) ||
                   name.Contains(filter) ||
                   surname.Contains(filter) ||
                   secondName.Contains(filter) ||
                   adress.Contains(filter) ||
                   (district?.Contains(filter) ?? false) ||
                   (birth?.Contains(filter) ?? false);
        }
    }
}