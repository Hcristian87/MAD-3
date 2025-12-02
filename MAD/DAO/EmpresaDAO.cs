using MAD.Entidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAD.DAO
{
    class EmpresaDAO
    {
        /*public static int InsertarEmp(Empresa empresa){
            int retorno = 0;

            using (SqlConnection conexion = BDConexion.ObtenerConexion())
            {
                string query = "EXEC InsertarEmp @nombreEmp = '" + empresa.NameEmp + "', @razonSocial = '" + empresa.RazonSoc + "', @domicilioFiscal = '" + empresa.DomFiscal + "', @registroPatronal = '" + empresa.RegPatronal + "', @RFC = '" + empresa.RFC + "', @fechaInicioOperaciones = '" + empresa.FInOper.Date.ToString("yyyy-MM-dd") + "', @datosContacto = " + empresa.DContact + "; ";
                SqlCommand comando = new SqlCommand(query, conexion);
                retorno = comando.ExecuteNonQuery();
            }

            return retorno;
        }*/

        public static int InsertarEmp(Empresa empresa)
        {
            int retorno = 0;

            using (SqlConnection conexion = BDConexion.ObtenerConexion())
            {
                string query = @"
            INSERT INTO Empresa 
            (nombreEmp, razonSocial, domicilioFiscal, registroPatronal, RFC, fechaInicioOperaciones, datosContacto)
            VALUES 
            (@nombreEmp, @razonSocial, @domicilioFiscal, @registroPatronal, @RFC, @fechaInicioOperaciones, @datosContacto);
        ";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@nombreEmp", empresa.NameEmp);
                    comando.Parameters.AddWithValue("@razonSocial", empresa.RazonSoc);
                    comando.Parameters.AddWithValue("@domicilioFiscal", empresa.DomFiscal);
                    comando.Parameters.AddWithValue("@registroPatronal", empresa.RegPatronal);
                    comando.Parameters.AddWithValue("@RFC", empresa.RFC);
                    comando.Parameters.AddWithValue("@fechaInicioOperaciones", empresa.FInOper);
                    comando.Parameters.AddWithValue("@datosContacto", empresa.DContact);

                    retorno = comando.ExecuteNonQuery();
                }
            }

            return retorno;
        }

    }
}
