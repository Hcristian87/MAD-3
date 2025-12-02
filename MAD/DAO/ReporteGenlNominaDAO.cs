using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAD.DAO
{
    class ReporteGenlNominaDAO
    {

        public static DataTable ObtenerListadoEmpleados(int mes, int anio)
        {
            DataTable tabla = new DataTable();

            using (SqlConnection conexion = BDConexion.ObtenerConexion())
            {
                string query = @"
SELECT 
    d.nombreDepartamento AS Departamento,
    p.nombrePuesto AS Puesto,
    e.apellidos + ' ' + e.nombre AS NombreEmpleado,
    e.fechaRegistro AS FechaIngreso,

    DATEDIFF(YEAR, e.fechaNacimiento, GETDATE()) 
        - CASE 
            WHEN (MONTH(GETDATE()) < MONTH(e.fechaNacimiento)) 
              OR (MONTH(GETDATE()) = MONTH(e.fechaNacimiento) AND DAY(GETDATE()) < DAY(e.fechaNacimiento)) 
            THEN 1 
            ELSE 0 
          END AS Edad,

    e.salarioDiario AS SalarioDiario

FROM Empleado e
INNER JOIN Departamento d ON e.idDepartamento = d.idDepartamento
INNER JOIN Puesto p ON e.idPuesto = p.idPuesto

WHERE YEAR(e.fechaRegistro) = @anio
  AND MONTH(e.fechaRegistro) = @mes

ORDER BY 
    d.nombreDepartamento,
    p.nombrePuesto,
    e.apellidos;
";

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@anio", anio);
                cmd.Parameters.AddWithValue("@mes", mes);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tabla);
            }

            return tabla;
        }


    }
}
