using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dto
{
    // Информация, передаваемая в JWT токен
    public class TokenData
    {
        public required string Login {get;set; }
        public required string Password { get; set; }
        public required string Role { get; set; }
    }
}
