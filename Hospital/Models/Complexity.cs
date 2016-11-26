using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Models
{
    [Table("complexity")]
    public class Complexity
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("title")]
        public string Title { get; set; }
    }
}