using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Models
{
    [Table("disease")]
    public class Disease
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("diseaseCategory")]
        public int DiseaseCategoryId { get; set; }

        [Column("title")]
        public string Title { get; set; }

        public DiseaseCategory DiseaseCategory { get; set; }
    }
}