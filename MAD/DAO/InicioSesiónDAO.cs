using MAD.Entidad;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAD.DAO
{
    class InicioSesiónDAO
    {

        public static Empleados ValidarLogin(string correo, string contraseña)
        {
            using (SqlConnection con = BDConexion.ObtenerConexion())
            {
                string query = @"
                SELECT idEmpleado, nombre, apellidos, email, contraseña
                FROM Empleado
                WHERE email = @correo AND contraseña = @pass
            ";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@correo", correo);
                cmd.Parameters.AddWithValue("@pass", contraseña);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        return new Empleados
                        {
                            idEmpleados = dr.GetInt32(0),
                            nombre = dr.GetString(1),
                            apellidos = dr.GetString(2),
                            email = dr.GetString(3)
                        };
                    }
                }
            }

            return null;
        }

    }
}
