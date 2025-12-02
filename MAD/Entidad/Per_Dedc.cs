using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAD.Entidad
{
    class Per_Dedc
    {
        public int idPerc_Dedc { get; set; }
        public int Mes { get; set; }
        public int Anio { get; set; }
        public string TipoAplicacion { get; set; }   // 'Empleado' o 'Departamento'
        public int? idEmpleado { get; set; }
        public int? idDepartamento { get; set; }
        public string TipoAjuste { get; set; }       // 'Bono', 'Deducción', etc.
        public string Concepto { get; set; }
        public string TipoMonto { get; set; }        // 'Fija' o 'Porcentaje'
        public decimal Monto { get; set; }
        //public DateTime FechaRegistro { get; set; }
    }
}
