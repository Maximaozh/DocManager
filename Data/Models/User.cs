﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public List<Group> Groups { get; set; } = [];
        public List<UserGroup> UserGroups { get; set; } = [];
    }
}


