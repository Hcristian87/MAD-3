using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MAD.Entidad;
using System.Windows.Forms;
using System.Data;


namespace MAD.DAO
{
    class PuestoDAO
    {
        public static void LlenarComboEmpresas(ComboBox combo)
        {
            combo.DataSource = null;
            combo.DisplayMember = "nombreEmp";
            combo.ValueMember = "idEmpresa";

            //Llena mi combobox con los nombres empresa, pero me guarda el id
            using (SqlConnection conexion = BDConexion.ObtenerConexion())
            {
                string query = "SELECT idEmpresa, nombreEmp FROM Empresa";
                SqlDataAdapter da = new SqlDataAdapter(query, conexion);
                DataTable dt = new DataTable();
                da.Fill(dt);
                combo.DataSource = dt;
            }
        }

        public static void LlenarComboDepartamentos(ComboBox combo, int idEmpresa)
        {
            combo.DataSource = null;
            combo.DisplayMember = "nombreDepartamento";
            combo.ValueMember = "idDepartamento";

            using (SqlConnection conexion = BDConexion.ObtenerConexion())
            {
                string query = "SELECT idDepartamento, nombreDepartamento FROM Departamento WHERE idEmpresa = @idEmp";
                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@idEmp", idEmpresa);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                combo.DataSource = dt;
            }
        }

        public static (int idDepa, int idEmp) ObtenerIdsPorNombreDepartamento(string nombreDepa)
        {
            int idDepa = -1;
            int idEmp = -1;

            using (SqlConnection conexion = BDConexion.ObtenerConexion())
            {
                string query = "SELECT idDepartamento, idEmpresa FROM Departamento WHERE nombreDepartamento = @nombreDepa";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@nombreDepa", nombreDepa);

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            idDepa = Convert.ToInt32(reader["idDepartamento"]);
                            idEmp = Convert.ToInt32(reader["idEmpresa"]);
                        }
                    }
                }
            }

            return (idDepa, idEmp);
        }

        public static Puesto ObtenerPuestoPorId(int idPuesto)
        {
            Puesto puesto = null;
            using (SqlConnection conexion = BDConexion.ObtenerConexion())
            {
                string query = "SELECT * FROM Puesto WHERE idPuesto = @idPuesto";
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@idPuesto", idPuesto);
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            puesto = new Puesto
                            {
                                idPuesto = (int)reader["idPuesto"],
                                namePuesto = reader["nombrePuesto"].ToString(),
                                Desc = reader["descripcion"].ToString(),
                                idEmp = (int)reader["idEmpresa"],
                                idDep = (int)reader["idDepartamento"]
                            };
                        }
                    }
                }
            }
            return puesto;
        }

        public static int IngresarPuesto(Puesto Puesto)
        {
            int retorno = 0;

            using (SqlConnection conexion = BDConexion.ObtenerConexion())
            {
                string query = @"
            INSERT INTO Puesto 
            (nombrePuesto, descripcion, idEmpresa, idDepartamento)
            VALUES 
            (@nombrePuesto, @descripcion, @idEmpresa, @idDepartamento);";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@nombrePuesto", Puesto.namePuesto);
                    comando.Parameters.AddWithValue("@descripcion", Puesto.Desc);
                    comando.Parameters.AddWithValue("@idEmpresa", Puesto.idEmp);
                    comando.Parameters.AddWithValue("@idDepartamento", Puesto.idDep);

                    retorno = comando.ExecuteNonQuery();
                }

            }

            return retorno;

        }

        public static int ModificarPuesto(Puesto Puesto)
        {
            int retorno = 0;

            using (SqlConnection conexion = BDConexion.ObtenerConexion())
            {
                string query = @"
            UPDATE Puesto
            SET nombrePuesto = @nombrePuesto,
                descripcion = @descripcion,
                idEmpresa = @idEmpresa,
                idDepartamento = @idDepartamento
            WHERE idPuesto = @idPuesto;";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@nombrePuesto", Puesto.namePuesto);
                    comando.Parameters.AddWithValue("@descripcion", Puesto.Desc);
                    comando.Parameters.AddWithValue("@idEmpresa", Puesto.idEmp);
                    comando.Parameters.AddWithValue("@idDepartamento", Puesto.idDep);
                    comando.Parameters.AddWithValue("@idPuesto", Puesto.idPuesto);

                    retorno = comando.ExecuteNonQuery();
                }

            }

            return retorno;

        }

        public static int DeletePuesto(Puesto Del_Puesto)
        {
            int retorno = 0;

            using (SqlConnection conexion = BDConexion.ObtenerConexion())
            {
                string query = @"
            DELETE FROM Puesto
            WHERE idPuesto = @idPuesto;";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@idPuesto", Del_Puesto.idPuesto);         // seleccionado del grid o variable

                    retorno = comando.ExecuteNonQuery();
                }
            }
            return retorno;
        }

        public static void MostrarPuestos(DataGridView grid)
        {
            using (SqlConnection conexion = BDConexion.ObtenerConexion())
            {
                string query = @"
                SELECT 
                    P.idPuesto,
                    P.nombrePuesto AS Nombre,
                	P.descripcion AS Descripción,
                	D.nombreDepartamento AS Departamento,
                    E.nombreEmp AS Empresa
                FROM 
                    Puesto P
                INNER JOIN 
                    Empresa E ON P.idEmpresa = E.idEmpresa
                INNER JOIN
                	Departamento D ON P.idDepartamento = D.idDepartamento;
            ";

                SqlDataAdapter da = new SqlDataAdapter(query, conexion); // 1️⃣ le pasas consulta y conexión
                DataTable dt = new DataTable();                          // 2️⃣ tabla en memoria
                da.Fill(dt);                                             // 3️⃣ el adaptador llena la tabla

                grid.DataSource = dt;                           // 4️⃣ se muestra en pantalla
            }
        }



    }
}
