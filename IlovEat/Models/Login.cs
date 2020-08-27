using System;
using System.Collections.Generic;
using System.Text;

namespace IlovEat.Models
{
    public class Login
    {
       public string email { get; set; }
       public string senha { get; set; }
    }

    public class LoginResponse
    {
        public string type { get; set; }
        public string token { get; set; }
        public string refreshToken { get; set; }
    }
}
