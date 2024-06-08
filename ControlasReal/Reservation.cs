using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlasReal
{
    class Reservation
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string Zone { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime EndDate { get; set; }
        public string AccesoMode { get; set; }
        public string AccesoData { get; set; }
    }
}
