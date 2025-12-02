using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAD.Entidad
{
    class Empleados
    {
        public int idEmpleados { get; set; }
        public int contra { get; set; }
        public string nombre { get; set; }
        public string apellidos { get; set; }
        public DateTime FechaN { get; set; }
        public string CURP { get; set; }
        public string NSS { get; set; }
        public string RFC { get; set; }
        //public string domicilio { get; set; }
        public int telefono { get; set; }
        public string email { get; set; }
        public string banco { get; set; }
        public int NumCuenta { get; set; }
        public decimal salarioDr { get; set; }
        public decimal salarioInt { get; set; }
        public string calle { get; set; }
        public int num { get; set; }
        public string colonia { get; set; }
        public string municipio { get; set; }
        public string estado { get; set; }
        public int codPost { get; set; }
        public int idDep { get; set; }
        public int idEmp { get; set; }
        public int idPuesto { get; set; }
        public DateTime FechaRegistro { get; set; }

    }

    /*class Domicilio
    {
        public int idDom { get; set; }
        public string calle { get; set; }
        public int num { get; set; }
        public string colonia { get; set; }
        public string municipio { get; set; }
        public string estado { get; set; }
        public int codPost { get; set; }
        public int idEmpleado { get; set; }

    }*/
}
