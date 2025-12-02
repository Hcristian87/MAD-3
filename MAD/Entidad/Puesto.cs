using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAD.Entidad
{
    class Puesto
    {
        public int idPuesto { get; set; }
        public string namePuesto { get; set; }
        public string Desc { get; set; }
        public int idEmp { get; set; }
        public int idDep { get; set; }
    }
}
