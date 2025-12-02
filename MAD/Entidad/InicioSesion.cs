using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAD.Entidad
{
    class InicioSesion
    {

        public static class SesionActual
        {            public static int IdEmpleado { get; set; }
            public static string NombreEmpleado { get; set; }
            public static string Rol { get; set; } // "Administrador" o "Auxiliar"
        }


    }
}
