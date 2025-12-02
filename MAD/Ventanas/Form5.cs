using MAD.DAO;
using MAD.Entidad;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MAD.Entidad.InicioSesion;

namespace MAD
{
    public partial class Form5: Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            Perc_DedcDAO.MostrarAjuste(dataGridView1);
            CargarEmpleados();
            CargarDepartamentos();

            // Al inicio, ocultamos ambos hasta que se seleccione el tipo
            comboEmpleado.Visible = false;
            comboDepartamento.Visible = false;
        }

        string rol = SesionActual.Rol;


        private void CargarEmpleados()
        {
            using (SqlConnection conexion = BDConexion.ObtenerConexion())
            {
                string query = "SELECT idEmpleado, nombre FROM Empleado";
                using (SqlCommand comando = new SqlCommand(query, conexion))
                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    comboEmpleado.DataSource = dt;
                    comboEmpleado.DisplayMember = "nombre";  // lo que se muestra
                    comboEmpleado.ValueMember = "idEmpleado";        // el valor real
                }
            }
        }
        private void CargarDepartamentos()
        {
            using (SqlConnection conexion = BDConexion.ObtenerConexion())
            {
                string query = "SELECT idDepartamento, nombreDepartamento FROM Departamento";
                using (SqlCommand comando = new SqlCommand(query, conexion))
                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    comboDepartamento.DataSource = dt;
                    comboDepartamento.DisplayMember = "nombreDepartamento";
                    comboDepartamento.ValueMember = "idDepartamento";
                }
            }
        }
        private void radioButtonEmpleado_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                comboEmpleado.Visible = true;
                comboDepartamento.Visible = false;
            }
        }
        private void radioButtonDepartamento_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                comboEmpleado.Visible = false;
                comboDepartamento.Visible = true;
            }
        }

        private void comboTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox4.Items.Clear();

            if (comboBox3.SelectedItem == null)
                return;

            string tipo = comboBox3.SelectedItem.ToString();

            if (tipo == "Percepciones")
            {
                comboBox4.Items.Add("Bono productividad");
                comboBox4.Items.Add("Despensa");
                //comboBox4.Items.Add("Aguinaldo");
                comboBox4.Items.Add("Bono puntualidad");
                comboBox4.Items.Add("Bono asistencia");
            }
            else if (tipo == "Deducciones")
            {
                comboBox4.Items.Add("ISR");
                comboBox4.Items.Add("IMSS");
                //comboBox4.Items.Add("Seguro social");
                //comboBox4.Items.Add("Impuesto de renta");
                comboBox4.Items.Add("Faltas");
            }

            // Para que selecciona el primer elemento automáticamente (opcional)
            if (comboBox4.Items.Count > 0)
                comboBox4.SelectedIndex = 0;
        }


        private void InsertarAjuste_Click(object sender, EventArgs e)
        {
            try
            {
                int numeroMes = DateTime.ParseExact(comboBox1.Text, "MMMM", new System.Globalization.CultureInfo("es-ES")).Month;
                int anio = int.Parse(comboBox2.Text);

                // 🔒 Validar si ya existe el mes en MesesProcesados
                //if (Perc_DedcDAO.MesAnioYaProcesado(numeroMes, anio))
                //{
                //    MessageBox.Show($"⚠ Este mes ({numeroMes}) y año ({anio}) ya fueron procesados.\nNo puedes registrar más ajustes.",
                //                    "Mes ya registrado",
                //                    MessageBoxButtons.OK,
                //                    MessageBoxIcon.Warning);
                //    return;
                //}

                Per_Dedc ajuste = new Per_Dedc
                {

                    Mes = numeroMes,
                    Anio = int.Parse(comboBox2.Text),
                    TipoAjuste = comboBox3.Text,
                    Concepto = comboBox4.Text,
                    Monto = decimal.Parse(textBox3.Text),
                    //FechaRegistro = DateTime.Now
                };

                // 🧍 Tipo de aplicación (Empleado / Departamento)
                if (radioButton1.Checked)
                {
                    ajuste.TipoAplicacion = "Empleado";
                    ajuste.idEmpleado = Convert.ToInt32(comboEmpleado.SelectedValue);
                    ajuste.idDepartamento = null;
                }
                else if (radioButton2.Checked)
                {
                    ajuste.TipoAplicacion = "Departamento";
                    ajuste.idDepartamento = Convert.ToInt32(comboDepartamento.SelectedValue);
                    ajuste.idEmpleado = null;
                }
                else
                {
                    MessageBox.Show("Selecciona si aplica a un empleado o a un departamento.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 💰 Tipo de monto (Fija / Porcentaje)
                if (radioButton3.Checked)
                {
                    ajuste.TipoMonto = "Fija";
                }
                else if (radioButton4.Checked)
                {
                    ajuste.TipoMonto = "Porcentaje";
                }
                else
                {
                    MessageBox.Show("Selecciona si el monto es Fijo o Porcentaje.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                //Validación para no permitir duplicar mes + año
                if (Perc_DedcDAO.ExisteMesAnio(ajuste.Mes, ajuste.Anio))
                {
                    MessageBox.Show("Ya existe un ajuste registrado para ese mes y año.",
                                    "Registro duplicado",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    return;
                }

                // Validación para no permitir saltarse meses del mismo año
                int ultimoMes = Perc_DedcDAO.ObtenerUltimoMes(ajuste.Anio);

                // Caso de aún no se registra ningún mes del año
                if (ultimoMes == 0)
                {
                    if (ajuste.Mes != 1)
                    {
                        MessageBox.Show("Debe iniciar registrando el mes de Enero.",
                                        "Orden de meses incorrecto",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                        return;
                    }
                }
                else
                {
                    // Ya hay meses registrados → exige que sea el siguiente mes
                    if (ajuste.Mes != ultimoMes + 1)
                    {
                        MessageBox.Show($"El siguiente mes obligatorio para {ajuste.Anio} es: {ultimoMes + 1}.",
                                        "Orden de meses incorrecto",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                        return;
                    }
                }

                //Validación para no permitir iniciar un año nuevo sin terminar el anterior
                int anioAnterior = ajuste.Anio - 1;
                if (anioAnterior < 2025)
                {
                    anioAnterior = anioAnterior + 1;
                }

                if (ajuste.Mes == 1) // Solo aplica cuando se va a registrar enero
                {
                    if (!Perc_DedcDAO.AnioCompleto(anioAnterior))
                    {
                        MessageBox.Show($"No puedes comenzar {ajuste.Anio}. " +
                                        $"El año {anioAnterior} no está completo (faltan meses).",
                                        "Año incompleto",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                        return;
                    }
                }

                // Guardar en BD
                int resultado = Perc_DedcDAO.InsertarAjuste(ajuste);

                if (resultado > 0)
                {
                    MessageBox.Show("Ajuste registrado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Perc_DedcDAO.MostrarAjuste(dataGridView1);

                    // (Opcional) limpiar campos
                    LimpiarFormulario();
                }
                else
                {
                    MessageBox.Show("No se pudo guardar el ajuste.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Verifica que los campos numéricos sean válidos.", "Error de formato", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Error SQL: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarFormulario()
        {
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            //textBox1.Clear();
            textBox3.Clear();
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;
        }

        //Menu

        private void empleadosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Reg_Empl f1 = new Reg_Empl();
            f1.Show();
            this.Hide();
        }

        private void departamentosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rol == "Administrador")
            {
                Form2 f2 = new Form2();
                f2.Show();
                this.Hide();
            }
        }

        private void puestosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rol == "Administrador")
            {
                Form3 f3 = new Form3();
                f3.Show();
                this.Hide();
            }
        }
        private void empresasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rol == "Administrador")
            {
                Form4 f4 = new Form4();
                f4.Show();
                this.Hide();
            }
        }

        private void PercepAndDeducToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Form5 f5 = new Form5();
            //f5.Show();
            //this.Hide();
        }

        private void CalcularToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form6 f6 = new Form6();
            f6.Show();
            this.Hide();
        }

        private void generalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form7 f7 = new Form7();
            f7.Show();
            this.Hide();
        }

        private void headcounterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form8 f8 = new Form8();
            f8.Show();
            this.Hide();
        }

        private void detalladaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form9 f9 = new Form9();
            f9.Show();
            this.Hide();
        }
        //
    }
}
