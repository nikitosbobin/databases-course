using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Models
{
    [Table("departments")]
    public class Department
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("head")]
        public string Head { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("seats")]
        public int Seats { get; set; }
    }
}