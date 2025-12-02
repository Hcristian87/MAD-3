using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAD.Entidad
{
    public class Recursos
    {
        //public int mes { get; set; }
        //public int anio { get; set; }
        public int idEmpl { get; set; }
        public int Empl_Emp { get; set; }
        public int Empl_Dep { get; set; }
        public decimal SalDr { get; set; }

    }

    //public class MovimientoPD
    //{
    //    public string tipoPyD { get; set; }
    //    public string concepto { get; set; }
    //    public decimal monto { get; set; }
    //}

    //public class Reporte
    //{
    //    public int mes { get; set; }
    //    public int anio { get; set; }
    //}

    /*
    public class ResultadoNomina
    {
        public decimal TotalPercepciones { get; set; }
        public decimal TotalDeducciones { get; set; }
        public decimal PagoFinal { get; set; }

        public List<string> ListaPercepciones { get; set; } = new List<string>();
        public List<string> ListaDeducciones { get; set; } = new List<string>();
    }

    public class PercDedc
    {
        public int Mes { get; set; }
        public int Anio { get; set; }
        public string Tipo { get; set; } // "Empleado" o "Departamento"
        public int IdEYD { get; set; }
        public string TipoPyD { get; set; } // "Percepcion" o "Deduccion"
        public string Concepto { get; set; }
        public string Forma { get; set; } // "Decimal" o "Porcentaje"
        public decimal Monto { get; set; }
    }*/

}
