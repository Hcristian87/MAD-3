using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAD
{
    class BDConexion
    {

        public static SqlConnection ObtenerConexion()
        {
            SqlConnection conexion = new SqlConnection("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=MAD_PIA;Data Source=GAYTAN_LARA\\OPUSDB");

            conexion.Open();
            return conexion;
        }

    }
}
