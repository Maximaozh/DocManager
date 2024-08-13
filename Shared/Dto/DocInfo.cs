using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dto
{
    //Информация о документе
    public class DocInfo
    {
        public int UserId { get; set; }
        public required string Name { get; set; }
        public required string Author { get; set; }
        public required string Path { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
