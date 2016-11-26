using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Models
{
    [Table("districts")]
    public class District
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("title")]
        public string Title { get; set; }
    }
}