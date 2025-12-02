using iTextSharp.text.pdf;
using iTextSharp.text;
using MAD.Entidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MAD.DAO
{
    public class NominaDAO
    {
        /*
        public decimal ObtenerSalarioDiario(int idEmpleado, out int idDepto)
        {
            decimal salario = 0;
            idDepto = 0;


            using (SqlConnection conn = BDConexion.ObtenerConexion())
            {
                //conn.Open();
                SqlCommand cmd = new SqlCommand(
                "SELECT salarioDiario, idDepartamento FROM Empleado WHERE idEmpleado = @id", conn);
                cmd.Parameters.AddWithValue("@id", idEmpleado);


                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    if (rd.Read())
                    {
                        salario = rd.GetDecimal(0);
                        idDepto = rd.GetInt32(1);
                    }
                }
            }


            return salario;
        }

        public List<PercDedc> ObtenerPercDedc(int idEmpleado, int idDepto, int mes, int anio)
        {
            List<PercDedc> lista = new List<PercDedc>();


            using (SqlConnection conn = BDConexion.ObtenerConexion())
            {
                //conn.Open();


                SqlCommand cmd = new SqlCommand(@"
            SELECT mes, anio, tipo, idEYD, tipoPyD, concepto, forma, monto
            FROM Perc_Dedc
            WHERE mes = @mes AND anio = @anio
            AND ((tipo = 'Empleado' AND idEYD = @idEmpleado)
            OR (tipo = 'Departamento' AND idEYD = @idDepto))
            ", conn);


                cmd.Parameters.AddWithValue("@mes", mes);
                cmd.Parameters.AddWithValue("@anio", anio);
                cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                cmd.Parameters.AddWithValue("@idDepto", idDepto);


                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        lista.Add(new PercDedc
                        {
                            Mes = rd.GetInt32(0),
                            Anio = rd.GetInt32(1),
                            Tipo = rd.GetString(2),
                            IdEYD = rd.GetInt32(3),
                            TipoPyD = rd.GetString(4),
                            Concepto = rd.GetString(5),
                            Forma = rd.GetString(6),
                            Monto = rd.GetDecimal(7)
                        });
                    }
                }
            }


            return lista;
        }

        public ResultadoNomina CalcularNomina(int idEmpleado, int mes, int anio)
        {
            ResultadoNomina result = new ResultadoNomina();
            int idDepto;


            decimal salarioDiario = ObtenerSalarioDiario(idEmpleado, out idDepto);
            int diasTrabajados = 30;


            var lista = ObtenerPercDedc(idEmpleado, idDepto, mes, anio);


            foreach (var pd in lista)
            {
                decimal cantidadFinal = (pd.Forma == "Porcentaje")
                ? salarioDiario * (pd.Monto / 100)
                : pd.Monto;


                if (pd.Concepto.ToLower() == "Faltas")
                {
                    diasTrabajados -= 1;
                    cantidadFinal = salarioDiario;
                }


                if (pd.TipoPyD == "Percepción")
                {
                    result.TotalPercepciones += cantidadFinal;
                    result.ListaPercepciones.Add($"{pd.Concepto}: {cantidadFinal:C}");
                }
                else
                {
                    result.TotalDeducciones += cantidadFinal;
                    result.ListaDeducciones.Add($"{pd.Concepto}: -{cantidadFinal:C}");
                }
            }


            decimal pagoBase = salarioDiario * diasTrabajados;
            result.PagoFinal = pagoBase + result.TotalPercepciones - result.TotalDeducciones;


            return result;
        }*/

        //public static void LlenarComboDepartamentos(ComboBox combo)
        //{
        //    combo.DataSource = null;
        //    combo.DisplayMember = "nombreDepartamento";
        //    combo.ValueMember = "idDepartamento";

        //    using (SqlConnection conexion = BDConexion.ObtenerConexion())
        //    {
        //        string query = "SELECT nombreDepartamento FROM Departamento";
        //        SqlCommand cmd = new SqlCommand(query, conexion);
        //        //cmd.Parameters.AddWithValue("@idEmp", idEmpresa);

        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        DataTable dt = new DataTable();
        //        da.Fill(dt);

        //        combo.DataSource = dt;
        //    }
        //}

        public static void LlenarComboDepartamentos(ComboBox combo)
        {
            using (SqlConnection conexion = BDConexion.ObtenerConexion())
            {
                string query = "SELECT idDepartamento, nombreDepartamento FROM Departamento";

                SqlDataAdapter da = new SqlDataAdapter(query, conexion);
                DataTable dt = new DataTable();
                da.Fill(dt);

                combo.DataSource = dt;
                combo.DisplayMember = "nombreDepartamento";  // Lo que ve el usuario
                combo.ValueMember = "idDepartamento";        // El ID real
            }
        }


        /*public static (int Empl_Dep,int Empl_Emp, int idEmpl, decimal SalDr) ObtenerIdsPorNombreDepartamento(string nombreEmpresa)
        {
            int idDep = -1;
            int idEmp = -1;
            int idEmpl = -1;
            decimal SalDr = -1;

            using (SqlConnection conexion = BDConexion.ObtenerConexion())
            {
                string query = "SELECT E.idDepartamento, E.idEmpresa, E.idEmpleado, E.salarioDiario, D.nombreDepartamento FROM Empleado E INNER JOIN Departamento D ON E.idDepartamento = D.idDepartamento WHERE nombreDepartamento = @nombreP";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@nombreP", nombreEmpresa);

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            idDep = Convert.ToInt32(reader["idDepartamento"]);
                            idEmp = Convert.ToInt32(reader["idEmpresa"]);
                            idEmpl = Convert.ToInt32(reader["idEmpleado"]);
                            SalDr = Convert.ToDecimal(reader["salarioDiario"]);
                        }
                    }
                }
            }

            return (idDep, idEmp, idEmpl, SalDr);
        }*/

        public static List<Recursos> ObtenerEmpleadosFiltrados(string nombreDepartamento)
        {
            List<Recursos> lista = new List<Recursos>();

            using (SqlConnection conexion = BDConexion.ObtenerConexion())
            {
                string query = @"
            SELECT 
                E.idDepartamento,
                E.idEmpresa,
                E.idEmpleado,
                E.salarioDiario
            FROM Empleado E
            INNER JOIN Departamento D 
                ON E.idDepartamento = D.idDepartamento
            WHERE D.nombreDepartamento = @nombreP";

                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@nombreP", nombreDepartamento);

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Recursos
                            {
                                Empl_Dep = Convert.ToInt32(reader["idDepartamento"]),
                                Empl_Emp = Convert.ToInt32(reader["idEmpresa"]),
                                idEmpl = Convert.ToInt32(reader["idEmpleado"]),
                                SalDr = Convert.ToDecimal(reader["salarioDiario"]),
                            });
                        }
                    }
                }
            }

            return lista;
        }

        public class MovimientoNomina
        {
            public string tipo { get; set; }
            public string tipoPyD { get; set; }
            public string concepto { get; set; }
            public decimal monto { get; set; }
        }


        public static List<MovimientoNomina> ObtenerMovimientosEmpleado(int idEmpleado, int idDepartamento, int mes, int anio)
        {
            List<MovimientoNomina> lista = new List<MovimientoNomina>();

            using (SqlConnection conexion = BDConexion.ObtenerConexion())
            {
                string query = @"
        SELECT tipo, tipoPyD, concepto, monto
        FROM Perc_Dedc
        WHERE mes = @mes
          AND anio = @anio
          AND (
                idEYD = @idEmpleado
                OR (tipo = 'Departamento' AND idEYD = @idDepartamento)
              )";

                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                    cmd.Parameters.AddWithValue("@idDepartamento", idDepartamento);
                    cmd.Parameters.AddWithValue("@mes", mes);
                    cmd.Parameters.AddWithValue("@anio", anio);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new MovimientoNomina
                            {
                                tipo = reader["tipo"].ToString(),
                                tipoPyD = reader["tipoPyD"].ToString(),
                                concepto = reader["concepto"].ToString(),
                                monto = Convert.ToDecimal(reader["monto"])
                            });
                        }
                    }
                }
            }

            return lista;
        }

        public static decimal CalcularISRReal(decimal sueldoMensual)
        {
            // Tabla simplificada de ISR
            if (sueldoMensual <= 8670.93m) return 0;
            else if (sueldoMensual <= 13748.42m) return (sueldoMensual - 8670.93m) * 0.10m;
            else if (sueldoMensual <= 23898.36m) return 507.75m + ((sueldoMensual - 13748.42m) * 0.15m);
            else if (sueldoMensual <= 41762.71m) return 2030.94m + ((sueldoMensual - 23898.36m) * 0.20m);
            else if (sueldoMensual <= 81293.17m) return 5581.63m + ((sueldoMensual - 41762.71m) * 0.25m);
            else if (sueldoMensual <= 108390.83m) return 15465.84m + ((sueldoMensual - 81293.17m) * 0.30m);
            else return 23597.59m + ((sueldoMensual - 108390.83m) * 0.35m);
        }

        public static void InsertarDetalleNomina(
            int idEmpleado,
            int mes,
            int anio,
            int idEmpresa,
            int departamento,
            decimal salarioMensual,
            decimal salarioIntegrado,
            int diasTrabajados,
            int incidencias,
            decimal salarioBruto,
            decimal bonoPunt,
            decimal bonoAssist,
            decimal productividad,
            decimal despensa,
            decimal percepciones,
            decimal imss,
            decimal isr,
            decimal deducciones,
            decimal salarioTotal)
        {
            using (SqlConnection conexion = BDConexion.ObtenerConexion())
            {
                string query = @"
            INSERT INTO DetalleNomina
            (idEmpleado, mes, anio, empresa, departamento, SalMen, SalInteg, 
             DTrabj, incid, SBruto, BonoPunt, BonoAssist, BonoProd, Despensa, Percp, IMSS, ISR, Dedc, SalTotal)
            VALUES
            (@idEmpleado, @mes, @anio, @empresa, @departamento, @SalMen, @SalInteg,
             @DTrabj, @incid, @SBruto, @BonoPunt, @BonoAssist, @BonoProd, @Despensa, @Percp, @IMSS, @ISR, @Dedc, @SalTotal)";

                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@idEmpleado", idEmpleado);
                    cmd.Parameters.AddWithValue("@mes", mes);
                    cmd.Parameters.AddWithValue("@anio", anio);
                    cmd.Parameters.AddWithValue("@empresa", idEmpresa);
                    cmd.Parameters.AddWithValue("@departamento", departamento);
                    cmd.Parameters.AddWithValue("@SalMen", salarioMensual);
                    cmd.Parameters.AddWithValue("@SalInteg", salarioIntegrado);
                    cmd.Parameters.AddWithValue("@DTrabj", diasTrabajados);
                    cmd.Parameters.AddWithValue("@incid", incidencias);
                    cmd.Parameters.AddWithValue("@SBruto", salarioBruto);
                    cmd.Parameters.AddWithValue("@BonoPunt", bonoPunt);
                    cmd.Parameters.AddWithValue("@BonoAssist", bonoAssist);
                    cmd.Parameters.AddWithValue("@BonoProd", productividad);
                    cmd.Parameters.AddWithValue("@Despensa", despensa);
                    cmd.Parameters.AddWithValue("@Percp", percepciones);
                    cmd.Parameters.AddWithValue("@IMSS", imss);
                    cmd.Parameters.AddWithValue("@ISR", isr);
                    cmd.Parameters.AddWithValue("@Dedc", deducciones);
                    cmd.Parameters.AddWithValue("@SalTotal", salarioTotal);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al insertar detalle nómina: " + ex.Message);
                    }
                }
            }
        }

        public static bool RegistrarMesProcesado(int mes, int anio)
        {
            using (SqlConnection conexion = BDConexion.ObtenerConexion())
            {
                string query = @"
        INSERT INTO MesesProcesados (mes, anio)
        VALUES (@mes, @anio)";

                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@mes", mes);
                    cmd.Parameters.AddWithValue("@anio", anio);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true; // se guardó
                    }
                    catch (SqlException ex)
                    {
                        // Error de duplicado (viola el UNIQUE)
                        if (ex.Number == 2627)
                            return false;

                        MessageBox.Show("Error al registrar mes: " + ex.Message);
                        return false;
                    }
                }
            }
        }

        /*public static DataTable ObtenerDetalleNomina(int mes, int anio, int depto)
        {
            using (SqlConnection con = BDConexion.ObtenerConexion())
            {
                string query = @"
            SELECT *
            FROM DetalleNomina
            WHERE mes = @mes AND anio = @anio AND departamento = @depto";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.SelectCommand.Parameters.AddWithValue("@mes", mes);
                da.SelectCommand.Parameters.AddWithValue("@anio", anio);
                da.SelectCommand.Parameters.AddWithValue("@depto", depto);

                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }*/

        public static DataTable ObtenerDetalleNominaFiltrado(int mes, int anio, int idDepartamento)
        {
            DataTable tabla = new DataTable();

            using (SqlConnection conexion = BDConexion.ObtenerConexion())
            {
                string query = @"
                SELECT
                    d.idDetalle,
                    e.nombre + ' ' + e.apellidos AS NombreEmpleado,
                    dep.nombreDepartamento AS Departamento,
                    emp.nombreEmp AS Empresa,
                    d.mes AS Mes,
                    d.anio AS Año,
                    d.SalMen AS Salario_Mensual,
                    d.SalInteg AS Salario_Integral,
                    d.DTrabj AS Dias_Trbajados,
                    d.incid AS Incidencias,
                    d.SBruto AS Sueldo_bruto,
                    d.BonoPunt AS Bono_puntualidad,
                    d.BonoAssist AS Bono_asistencia,
                    d.BonoProd AS Bono_producción,
                    d.Despensa AS Despensa,
                    d.Percp AS Percepciones,
                    d.IMSS AS IMSS,
                    d.ISR AS ISR,
                    d.Dedc AS Deducciones,
                    d.SalTotal AS Salario_Total,
                    d.fechaRegistro AS Fecha_Registro
                FROM DetalleNomina d
                INNER JOIN Empleado e ON d.idEmpleado = e.idEmpleado
                LEFT JOIN Departamento dep ON d.departamento = dep.idDepartamento
                LEFT JOIN Empresa emp ON d.empresa = emp.idEmpresa
                WHERE d.mes = @mes
                 AND d.anio = @anio
                 AND d.departamento = @idDepartamento; ";

        SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@mes", mes);
                cmd.Parameters.AddWithValue("@anio", anio);
                cmd.Parameters.AddWithValue("@idDepartamento", idDepartamento);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tabla);
            }

            return tabla;
        }

        public static DataTable ObtenerDetalleNominaParaCSV(int mes, int anio, int idDep)
        {
            using (SqlConnection conexion = BDConexion.ObtenerConexion())
            {
                string query = @"
SELECT
    d.idDetalle,
    e.nombre + ' ' + e.apellidos AS NombreEmpleado,
    dep.nombreDepartamento AS Departamento,
    emp.nombreEmp AS Empresa,
    d.mes,
    d.anio,
    d.SalMen,
    d.SalInteg,
    d.DTrabj,
    d.incid,
    d.SBruto,
    d.BonoPunt,
    d.BonoAssist,
    d.BonoProd,
    d.Despensa,
    d.Percp,
    d.IMSS,
    d.ISR,
    d.Dedc,
    d.SalTotal,
    d.fechaRegistro
FROM DetalleNomina d
INNER JOIN Empleado e ON d.idEmpleado = e.idEmpleado
LEFT JOIN Departamento dep ON d.departamento = dep.idDepartamento
LEFT JOIN Empresa emp ON d.empresa = emp.idEmpresa
WHERE d.mes = @mes AND d.anio = @anio AND d.departamento = @dep;";

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@mes", mes);
                cmd.Parameters.AddWithValue("@anio", anio);
                cmd.Parameters.AddWithValue("@dep", idDep);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
        }

        public static DataTable ObtenerDetalleNominaParaPDF(int mes, int anio, int idDep)
        {
            using (SqlConnection conexion = BDConexion.ObtenerConexion())
            {
                string query = @"
SELECT
    d.idDetalle,
    e.nombre + ' ' + e.apellidos AS NombreEmpleado,
    dep.nombreDepartamento AS Departamento,
    emp.nombreEmp AS Empresa,
    d.mes,
    d.anio,
    d.SalMen,
    d.SalInteg,
    d.DTrabj,
    d.incid,
    d.SBruto,
    d.BonoPunt,
    d.BonoAssist,
    d.BonoProd,
    d.Despensa,
    d.Percp,
    d.IMSS,
    d.ISR,
    d.Dedc,
    d.SalTotal,
    d.fechaRegistro
FROM DetalleNomina d
INNER JOIN Empleado e ON d.idEmpleado = e.idEmpleado
LEFT JOIN Departamento dep ON d.departamento = dep.idDepartamento
LEFT JOIN Empresa emp ON d.empresa = emp.idEmpresa
WHERE d.mes = @mes AND d.anio = @anio AND d.departamento = @dep";

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@mes", mes);
                cmd.Parameters.AddWithValue("@anio", anio);
                cmd.Parameters.AddWithValue("@dep", idDep);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
        }

        
        

    }
}
