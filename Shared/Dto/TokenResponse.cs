using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dto
{
    // Строка, представляющая собой JWT токен
    public class TokenResponse
    {
        public string? Token { get; set; }
    }
}
