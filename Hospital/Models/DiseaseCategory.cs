using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Models
{
    [Table("diseasecategories")]
    public class DiseaseCategory
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("title")]
        public string Title { get; set; }
    }
}