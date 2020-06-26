using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiNetCoreAutenticacionJwt.Models
{
    public class LoginUser
    {
        public string Usuario { get; set; }

        public string Clave { get; set; }
    }
}
