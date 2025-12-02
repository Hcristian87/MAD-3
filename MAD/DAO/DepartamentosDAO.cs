using MAD.Entidad;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MAD.DAO
{
    class DepartamentosDAO
    {
        public static void LlenarComboEmpresas(ComboBox combo)
        {
            combo.Items.Clear();

            using (SqlConnection conexion = BDConexion.ObtenerConexion())
            {
                string query = "SELECT nombreEmp FROM Empresa";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            combo.Items.Add(reader["nombreEmp"].ToString());
                        }
                    }
                }
            }
        }

        public static int IngresarDep(Departamentos Depa)
        {
            int retorno = 0;

            using (SqlConnection conexion = BDConexion.ObtenerConexion())
            {
                string query = @"
            INSERT INTO Departamento 
            (nombreDepartamento, idEmpresa)
            VALUES 
            (@nombreDepartamento, @idEmpresa);";

                using (SqlCommand comando = new SqlCommand(query, conexion))
               {
                   comando.Parameters.AddWithValue("@nombreDepartamento", Depa.NameDep);
                   comando.Parameters.AddWithValue("@idEmpresa", Depa.idEmp);

                   retorno = comando.ExecuteNonQuery();
               }

            }

            return retorno;

        }

        public static int ModificarDep(Departamentos Mod_Depa)
        {
            int retorno = 0;

            using (SqlConnection conexion = BDConexion.ObtenerConexion())
            {
                string query = @"
            UPDATE Departamento
            SET nombreDepartamento = @nombreDepartamento,
                idEmpresa = @idEmpresa
            WHERE idDepartamento = @idDepartamento;";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@nombreDepartamento", Mod_Depa.NameDep);  
                    comando.Parameters.AddWithValue("@idEmpresa", Mod_Depa.idEmp);             
                    comando.Parameters.AddWithValue("@idDepartamento", Mod_Depa.idDep);         // seleccionado del grid o variable

                    retorno = comando.ExecuteNonQuery();
                }
            }

            return retorno;

        }

        public static int DeleteDep(Departamentos Del_Depa) {
            int retorno = 0;

            using (SqlConnection conexion = BDConexion.ObtenerConexion())
            {
                string query = @"
            DELETE FROM Departamento
            WHERE idDepartamento = @idDepartamento;";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@idDepartamento", Del_Depa.idDep);         // seleccionado del grid o variable

                    try
                    {
                        retorno = comando.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        // Verificamos si el error es por una FK (clave foránea)
                        if (ex.Number == 547)
                        {
                            MessageBox.Show(
                                "No se puede eliminar el departamento porque tiene puestos relacionados.",
                                "Advertencia",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning
                            );
                        }
                        /*else
                        {
                            MessageBox.Show(
                                "Ocurrió un error al eliminar el departamento:\n" + ex.Message,
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            );
                        }*/
                    }
                }
            }
            return retorno;
        }

        public static int ObtenerIdEmpresaPorNombre(string nombreEmpresa)
        {
            int idEmpresa = -1;

            using (SqlConnection conexion = BDConexion.ObtenerConexion())
            {
                string query = "SELECT idEmpresa FROM Empresa WHERE nombreEmp = @nombreEmpresa";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@nombreEmpresa", nombreEmpresa);

                    object result = comando.ExecuteScalar(); // obtiene un solo valor

                    if (result != null)
                    {
                        idEmpresa = Convert.ToInt32(result);
                    }
                }
            }

            return idEmpresa;
        }

        public static void MostrarDepartamentos(DataGridView grid)
        {
            using (SqlConnection conexion = BDConexion.ObtenerConexion())
            {
                string query = @"
                SELECT 
                    D.idDepartamento,
                    D.nombreDepartamento,
                    E.nombreEmp AS Empresa
                FROM 
                    Departamento D
                INNER JOIN 
                    Empresa E ON D.idEmpresa = E.idEmpresa;
            ";

                SqlDataAdapter da = new SqlDataAdapter(query, conexion); // 1️⃣ le pasas consulta y conexión
                DataTable dt = new DataTable();                          // 2️⃣ tabla en memoria
                da.Fill(dt);                                             // 3️⃣ el adaptador llena la tabla

                grid.DataSource = dt;                           // 4️⃣ se muestra en pantalla
            }
        }

    }
}
