using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class UserGroup
    {
        [Key]
        public int Usergroup_ID { get; set; }
        public int User_ID { get; set; }
        public User? User { get; set; }
        public int Group_ID { get; set; }
        public Group? Group { get; set; }
    }
}
