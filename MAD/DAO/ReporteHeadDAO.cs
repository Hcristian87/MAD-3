using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAD.DAO
{
    class ReporteHeadDAO
    {

        public static DataTable ObtenerEmpleadosPorPuesto(string departamento, int anio, int mes)
        {
            using (SqlConnection con = BDConexion.ObtenerConexion())
            {
                string query = @"
        SELECT 
            d.nombreDepartamento AS Departamento,
            p.nombrePuesto AS Puesto,
            COUNT(e.idEmpleado) AS CantidadEmpleados
        FROM Empleado e
        INNER JOIN Departamento d ON e.idDepartamento = d.idDepartamento
        INNER JOIN Puesto p ON e.idPuesto = p.idPuesto
        WHERE 
            d.nombreDepartamento = @Departamento
            AND YEAR(e.fechaRegistro) <= @Anio
            AND MONTH(e.fechaRegistro) <= @Mes
        GROUP BY d.nombreDepartamento, p.nombrePuesto
        ORDER BY d.nombreDepartamento, p.nombrePuesto;
        ";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Departamento", departamento);
                cmd.Parameters.AddWithValue("@Anio", anio);
                cmd.Parameters.AddWithValue("@Mes", mes);

                DataTable dt = new DataTable();
                new SqlDataAdapter(cmd).Fill(dt);
                return dt;
            }
        }


        public static DataTable ObtenerTotalEmpleadosPorDepartamento(int anio, int mes)
        {
            using (SqlConnection con = BDConexion.ObtenerConexion())
            {
                string query = @"
        SELECT 
            d.nombreDepartamento AS Departamento,
            COUNT(e.idEmpleado) AS TotalEmpleados
        FROM Empleado e
        INNER JOIN Departamento d ON e.idDepartamento = d.idDepartamento
        WHERE 
            YEAR(e.fechaRegistro) <= @Anio
            AND MONTH(e.fechaRegistro) <= @Mes
        GROUP BY d.nombreDepartamento
        ORDER BY d.nombreDepartamento;
        ";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Anio", anio);
                cmd.Parameters.AddWithValue("@Mes", mes);

                DataTable dt = new DataTable();
                new SqlDataAdapter(cmd).Fill(dt);
                return dt;
            }
        }



    }
}
