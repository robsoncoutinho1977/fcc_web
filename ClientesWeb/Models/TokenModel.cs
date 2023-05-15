using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientesWeb.Models
{
    public class TokenModel
    {
        public int status { get; set; }
        public string mensagem { get; set; }
        public ClienteModel cliente { get; set; }
        public string token { get; set; }
        public string debug { get; set; }
    }
}
