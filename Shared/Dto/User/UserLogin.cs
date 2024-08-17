using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dto
{
    // Отправляемая инфомрация на сервер для проверки
    public class UserLogin
    {
        public required string Login { get; set; }
        public required string Password { get; set; }
    }
}
