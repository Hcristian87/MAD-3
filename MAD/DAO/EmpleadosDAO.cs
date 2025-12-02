using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MAD.Entidad;
using System.Windows.Forms;

namespace MAD.DAO
{
    class EmpleadosDAO
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

        public static void LlenarComboPuestos(ComboBox combo, int idDepartamento)
        {
            combo.DataSource = null;
            combo.DisplayMember = "nombrePuesto";
            combo.ValueMember = "idPuesto";

            using (SqlConnection conexion = BDConexion.ObtenerConexion())
            {
                string query = "SELECT idPuesto, nombrePuesto FROM Puesto WHERE idDepartamento = @idDep";
                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@idDep", idDepartamento);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                combo.DataSource = dt;
            }
        }

        public static (int idPuesto, int idDepa, int idEmp) ObtenerIdsPorNombrePuesto(string nombrePuesto)
        {
            int idPuesto = -1;
            int idDepa = -1;
            int idEmp = -1;

            using (SqlConnection conexion = BDConexion.ObtenerConexion())
            {
                string query = "SELECT idPuesto, idDepartamento, idEmpresa FROM Puesto WHERE nombrePuesto = @nombreP";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@nombreP", nombrePuesto);

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            idPuesto= Convert.ToInt32(reader["idPuesto"]);
                            idDepa = Convert.ToInt32(reader["idDepartamento"]);
                            idEmp = Convert.ToInt32(reader["idEmpresa"]);
                        }
                    }
                }
            }

            return (idPuesto, idDepa, idEmp);
        }

        public static int IngresarEmp(Empleados Emp)
        {
            int retorno = 0;

            using (SqlConnection conexion = BDConexion.ObtenerConexion())
            {
                string query = @"
            INSERT INTO Empleado 
            (idEmpleado, contraseña, nombre, apellidos, fechaNacimiento, CURP, NSS, RFC, telefono, email, banco,
                numeroCuenta, salarioDiario, salarioIntegrado, idDepartamento, idPuesto, idEmpresa, calle, num,
                colonia, municipio, estado, codPost, fechaRegistro)
            VALUES
            (@idEmpleado, @contraseña, @nombre, @apellidos, @fechaNacimiento, @CURP, @NSS, @RFC, @telefono,
                @email, @banco, @numeroCuenta, @salarioDiario, @salarioIntegrado, @idDepartamento, @idPuesto,
                @idEmpresa, @calle, @num, @colonia, @municipio, @estado, @codPost, GETDATE()
            );";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@idEmpleado", Emp.idEmpleados);
                    comando.Parameters.AddWithValue("@contraseña", Emp.contra);
                    comando.Parameters.AddWithValue("@nombre", Emp.nombre);
                    comando.Parameters.AddWithValue("@apellidos", Emp.apellidos);
                    comando.Parameters.AddWithValue("@fechaNacimiento", Emp.FechaN);
                    comando.Parameters.AddWithValue("@CURP", Emp.CURP);
                    comando.Parameters.AddWithValue("@NSS", Emp.NSS);
                    comando.Parameters.AddWithValue("@RFC", Emp.RFC);
                    //comando.Parameters.AddWithValue("@domicilio", Emp.domicilio);
                    comando.Parameters.AddWithValue("@telefono", Emp.telefono);
                    comando.Parameters.AddWithValue("@email", Emp.email);
                    comando.Parameters.AddWithValue("@banco", Emp.banco);
                    comando.Parameters.AddWithValue("@numeroCuenta", Emp.NumCuenta);
                    comando.Parameters.AddWithValue("@salarioDiario", Emp.salarioDr);
                    comando.Parameters.AddWithValue("@salarioIntegrado", Emp.salarioInt);
                    comando.Parameters.AddWithValue("@idDepartamento", Emp.idDep);
                    comando.Parameters.AddWithValue("@idPuesto", Emp.idPuesto);
                    comando.Parameters.AddWithValue("@idEmpresa", Emp.idEmp);
                    comando.Parameters.AddWithValue("@calle", Emp.calle);
                    comando.Parameters.AddWithValue("@num", Emp.num);
                    comando.Parameters.AddWithValue("@colonia", Emp.colonia);
                    comando.Parameters.AddWithValue("@municipio", Emp.municipio);
                    comando.Parameters.AddWithValue("@estado", Emp.estado);
                    comando.Parameters.AddWithValue("@codPost", Emp.codPost);

                    retorno = comando.ExecuteNonQuery();
                }

            }

            return retorno;

        }

        public static int ModificarEmp(Empleados Emp)
        {
            int retorno = 0;

            using (SqlConnection conexion = BDConexion.ObtenerConexion())
            {
                string query = @"
        UPDATE Empleado SET
            contraseña = @contraseña,
            nombre = @nombre,
            apellidos = @apellidos,
            fechaNacimiento = @fechaNacimiento,
            CURP = @CURP,
            NSS = @NSS,
            RFC = @RFC,
            telefono = @telefono,
            email = @email,
            banco = @banco,
            numeroCuenta = @numeroCuenta,
            salarioDiario = @salarioDiario,
            salarioIntegrado = @salarioIntegrado,
            idDepartamento = @idDepartamento,
            idPuesto = @idPuesto,
            idEmpresa = @idEmpresa,
            calle = @calle,
            num = @num,
            colonia = @colonia,
            municipio = @municipio,
            estado = @estado,
            codPost = @codPost
        WHERE idEmpleado = @idEmpleado;";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@idEmpleado", Emp.idEmpleados);
                    comando.Parameters.AddWithValue("@contraseña", Emp.contra);
                    comando.Parameters.AddWithValue("@nombre", Emp.nombre);
                    comando.Parameters.AddWithValue("@apellidos", Emp.apellidos);
                    comando.Parameters.AddWithValue("@fechaNacimiento", Emp.FechaN);
                    comando.Parameters.AddWithValue("@CURP", Emp.CURP);
                    comando.Parameters.AddWithValue("@NSS", Emp.NSS);
                    comando.Parameters.AddWithValue("@RFC", Emp.RFC);
                    // comando.Parameters.AddWithValue("@domicilio", Emp.domicilio);
                    comando.Parameters.AddWithValue("@telefono", Emp.telefono);
                    comando.Parameters.AddWithValue("@email", Emp.email);
                    comando.Parameters.AddWithValue("@banco", Emp.banco);
                    comando.Parameters.AddWithValue("@numeroCuenta", Emp.NumCuenta);
                    comando.Parameters.AddWithValue("@salarioDiario", Emp.salarioDr);
                    comando.Parameters.AddWithValue("@salarioIntegrado", Emp.salarioInt);
                    comando.Parameters.AddWithValue("@idDepartamento", Emp.idDep);
                    comando.Parameters.AddWithValue("@idPuesto", Emp.idPuesto);
                    comando.Parameters.AddWithValue("@idEmpresa", Emp.idEmp);
                    comando.Parameters.AddWithValue("@calle", Emp.calle);
                    comando.Parameters.AddWithValue("@num", Emp.num);
                    comando.Parameters.AddWithValue("@colonia", Emp.colonia);
                    comando.Parameters.AddWithValue("@municipio", Emp.municipio);
                    comando.Parameters.AddWithValue("@estado", Emp.estado);
                    comando.Parameters.AddWithValue("@codPost", Emp.codPost);

                    retorno = comando.ExecuteNonQuery();
                }
            }

            return retorno;
        }

        public static Empleados ObtenerEmpleadoPorId(int idEmpleado)
        {
            Empleados empleado = null;

            using (SqlConnection conexion = BDConexion.ObtenerConexion())
            {
                string query = "SELECT * FROM Empleado WHERE idEmpleado = @idEmpleado";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@idEmpleado", idEmpleado);

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            empleado = new Empleados
                            {
                                idEmpleados = (int)reader["idEmpleado"],
                                contra = Convert.ToInt32(reader["contraseña"]),
                                nombre = reader["nombre"].ToString(),
                                apellidos = reader["apellidos"].ToString(),
                                FechaN = Convert.ToDateTime(reader["fechaNacimiento"]),
                                CURP = reader["CURP"].ToString(),
                                NSS = reader["NSS"].ToString(),
                                RFC = reader["RFC"].ToString(),
                                //domicilio = reader["domicilio"].ToString(), // si lo usas
                                telefono = Convert.ToInt32(reader["telefono"]),
                                email = reader["email"].ToString(),
                                banco = reader["banco"].ToString(),
                                NumCuenta = Convert.ToInt32(reader["numeroCuenta"]),
                                salarioDr = Convert.ToDecimal(reader["salarioDiario"]),
                                salarioInt = Convert.ToDecimal(reader["salarioIntegrado"]),
                                idDep = Convert.ToInt32(reader["idDepartamento"]),
                                idPuesto = Convert.ToInt32(reader["idPuesto"]),
                                idEmp = Convert.ToInt32(reader["idEmpresa"]),
                                calle = reader["calle"].ToString(),
                                num = Convert.ToInt32(reader["num"]),
                                colonia = reader["colonia"].ToString(),
                                municipio = reader["municipio"].ToString(),
                                estado = reader["estado"].ToString(),
                                codPost = Convert.ToInt32(reader["codPost"])
                            };
                        }
                    }
                }
            }

            return empleado;
        }

        public static int DeletePuesto(Empleados Del_Emp)
        {
            int retorno = 0;

            using (SqlConnection conexion = BDConexion.ObtenerConexion())
            {
                string query = @"
            DELETE FROM Empleado
            WHERE idEmpleado = @idEmpleado;";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@idEmpleado", Del_Emp.idEmpleados);         // seleccionado del grid o variable

                    retorno = comando.ExecuteNonQuery();
                }
            }
            return retorno;
        }

        public static void MostrarEmp(DataGridView grid)
        {
            using (SqlConnection conexion = BDConexion.ObtenerConexion())
            {
                string query = @"
                SELECT * FROM Empleado";

                SqlDataAdapter da = new SqlDataAdapter(query, conexion); // 1️⃣ le pasas consulta y conexión
                DataTable dt = new DataTable();                          // 2️⃣ tabla en memoria
                da.Fill(dt);                                             // 3️⃣ el adaptador llena la tabla

                grid.DataSource = dt;                           // 4️⃣ se muestra en pantalla
            }
        }

    }
}
