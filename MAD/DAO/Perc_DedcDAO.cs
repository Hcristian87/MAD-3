using MAD.Entidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MAD.DAO
{
    class Perc_DedcDAO
    {
        public static int InsertarAjuste(Per_Dedc ajuste)
        {
            int resultado = 0;

            using (SqlConnection conexion = BDConexion.ObtenerConexion())
            {
                string query = @"
              INSERT INTO Perc_Dedc
              (mes, anio, tipo, idEYD, tipoPyD, concepto, forma, monto)
              VALUES (@mes, @anio, @tipo, @idEYD, @tipoPyD, @concepto, @forma, @monto);
              ";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@mes", ajuste.Mes);
                    comando.Parameters.AddWithValue("@anio", ajuste.Anio);
                    comando.Parameters.AddWithValue("@tipo", ajuste.TipoAplicacion);
                    comando.Parameters.AddWithValue("@tipoPyD", ajuste.TipoAjuste);
                    comando.Parameters.AddWithValue("@concepto", ajuste.Concepto);
                    comando.Parameters.AddWithValue("@forma", ajuste.TipoMonto); // 'Fija' o 'Porcentaje'
                    comando.Parameters.AddWithValue("@monto", ajuste.Monto);

                    if (ajuste.idEmpleado.HasValue)
                        comando.Parameters.AddWithValue("@idEYD", ajuste.idEmpleado.Value);
                    else
                        if (ajuste.idDepartamento.HasValue)
                        comando.Parameters.AddWithValue("@idEYD", ajuste.idDepartamento.Value);

                    resultado = comando.ExecuteNonQuery();
                }
            }

            return resultado;
        }

        public static void MostrarAjuste(DataGridView grid)
        {
            using (SqlConnection conexion = BDConexion.ObtenerConexion())
            {
                string query = @"
                SELECT * FROM Perc_Dedc";

                SqlDataAdapter da = new SqlDataAdapter(query, conexion); // 1️⃣ le pasas consulta y conexión
                DataTable dt = new DataTable();                          // 2️⃣ tabla en memoria
                da.Fill(dt);                                             // 3️⃣ el adaptador llena la tabla

                grid.DataSource = dt;                           // 4️⃣ se muestra en pantalla
            }
        }

        //public static bool MesAnioYaProcesado(int mes, int anio)
        //{
        //    using (SqlConnection conexion = BDConexion.ObtenerConexion())
        //    {
        //        string query = "SELECT COUNT(*) FROM MesesProcesados WHERE mes = @mes AND anio = @anio";

        //        using (SqlCommand cmd = new SqlCommand(query, conexion))
        //        {
        //            cmd.Parameters.AddWithValue("@mes", mes);
        //            cmd.Parameters.AddWithValue("@anio", anio);

        //            int count = (int)cmd.ExecuteScalar();
        //            return count > 0; // true si ya existe
        //        }
        //    }
        //}

        public static bool ExisteMesAnio(int mes, int anio)
        {
            using (SqlConnection conexion = BDConexion.ObtenerConexion())
            {
                string query = "SELECT COUNT(*) FROM Perc_Dedc WHERE mes = @mes AND anio = @anio";

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@mes", mes);
                cmd.Parameters.AddWithValue("@anio", anio);

                return ((int)cmd.ExecuteScalar()) > 0;
            }
        }

        public static int ObtenerUltimoMes(int anio)
        {
            using (SqlConnection conexion = BDConexion.ObtenerConexion())
            {
                string query = "SELECT ISNULL(MAX(mes), 0) FROM Perc_Dedc WHERE anio = @anio";

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@anio", anio);

                return (int)cmd.ExecuteScalar();
            }
        }

        public static bool AnioCompleto(int anio)
        {
            using (SqlConnection conexion = BDConexion.ObtenerConexion())
            {
                string query = "SELECT COUNT(*) FROM Perc_Dedc WHERE anio = @anio";

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@anio", anio);

                int total = (int)cmd.ExecuteScalar();

                return total == 12; // Deben existir los 12 meses
            }
        }


    }
}
