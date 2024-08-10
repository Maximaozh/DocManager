using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Document
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Desc { get; set; }
        public DateOnly Created { get; set; }
        public DateOnly ExpireDate { get; set; }
        public User User { get; set; }
    }
}
