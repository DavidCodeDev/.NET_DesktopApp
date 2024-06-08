using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlasReal
{
    public class User
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string apellido1 { get; set; }
        public string apellido2 { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string? permisos { get; set; }
        public string password { get; set; }
    }
}
