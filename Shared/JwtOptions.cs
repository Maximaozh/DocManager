using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class JwtOptions
    {
        public string Key { get; set; } = string.Empty;
        public int HoursLeft { get; set; } = 0;
    }
}
