using MAD.DAO;
using MAD.Entidad;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MAD.Entidad.InicioSesion;

namespace MAD
{
    public partial class Reg_Empl: Form
    {
        public Reg_Empl()
        {
            InitializeComponent();
        }

        string rol = SesionActual.Rol;

        //private void FormMenu_Load(object sender, EventArgs e)
        //{
        //    AplicarPermisos();
        //}


        private void Reg_Empl_Load(object sender, EventArgs e)
        {
            EmpleadosDAO.MostrarEmp(dataGridView1);
            EmpleadosDAO.LlenarComboEmpresas(comboBox3);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verifica que no se haya hecho clic en el encabezado
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dataGridView1.Rows[e.RowIndex];

                // Rellena los textBox con los datos del empleado seleccionado
                textBox1.Text = fila.Cells["idEmpleado"].Value.ToString();
                textBox2.Text = fila.Cells["contraseña"].Value.ToString();
                textBox3.Text = fila.Cells["nombre"].Value.ToString();
                textBox4.Text = fila.Cells["apellidos"].Value.ToString();
                textBox5.Text = fila.Cells["salarioDiario"].Value.ToString();
                textBox6.Text = fila.Cells["salarioIntegrado"].Value.ToString();

                // Fecha de nacimiento
                if (fila.Cells["fechaNacimiento"].Value != DBNull.Value)
                    dateTimePicker1.Value = Convert.ToDateTime(fila.Cells["fechaNacimiento"].Value);

                textBox7.Text = fila.Cells["CURP"].Value.ToString();
                textBox8.Text = fila.Cells["RFC"].Value.ToString();
                textBox9.Text = fila.Cells["email"].Value.ToString();
                textBox10.Text = fila.Cells["NSS"].Value.ToString();
                textBox17.Text = fila.Cells["banco"].Value.ToString();
                textBox18.Text = fila.Cells["numeroCuenta"].Value.ToString();
                textBox19.Text = fila.Cells["telefono"].Value.ToString();

                // Domicilio
                textBox11.Text = fila.Cells["calle"].Value.ToString();
                textBox12.Text = fila.Cells["num"].Value.ToString();
                textBox13.Text = fila.Cells["colonia"].Value.ToString();
                textBox14.Text = fila.Cells["municipio"].Value.ToString();
                textBox15.Text = fila.Cells["estado"].Value.ToString();
                textBox16.Text = fila.Cells["codPost"].Value.ToString();

                // Combos (puedes cambiarlos si en tu grid muestras los nombres)
                comboBox2.Text = fila.Cells["idPuesto"].Value.ToString();
                // Si tu grid muestra el nombre del puesto en vez del id, puedes usar:
                // comboBox2.Text = fila.Cells["nombrePuesto"].Value.ToString();
            }
        }

        private void comboEmpresas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedValue != null && int.TryParse(comboBox3.SelectedValue.ToString(), out int idEmp))
            {
                EmpleadosDAO.LlenarComboDepartamentos(comboBox1, idEmp);
            }
        }

        private void comboDepartamentos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue != null && int.TryParse(comboBox1.SelectedValue.ToString(), out int idDep))
            {
                EmpleadosDAO.LlenarComboPuestos(comboBox2, idDep);
            }
        }

        private void InsertEmp_Click(object sender, EventArgs e)
        {
            // Crear el objeto del puesto
            Empleados Emp = new Empleados();
            //Domicilio Dom = new Domicilio();
            Emp.idEmpleados = Convert.ToInt32(textBox1.Text);
            Emp.contra = Convert.ToInt32(textBox2.Text);
            Emp.nombre = textBox3.Text;
            Emp.apellidos = textBox4.Text;
            Emp.salarioDr= Convert.ToDecimal(textBox5.Text);
            Emp.salarioInt = Convert.ToDecimal(textBox6.Text);
            Emp.FechaN = dateTimePicker1.Value.Date;
            Emp.CURP = textBox7.Text;
            Emp.RFC = textBox8.Text;
            Emp.email = textBox9.Text;
            Emp.NSS = textBox10.Text;
            Emp.banco = textBox17.Text;
            Emp.NumCuenta = Convert.ToInt32(textBox18.Text);
            Emp.telefono = Convert.ToInt32(textBox19.Text);


            // Buscar el idDepartamento y el idEmpresa según el nombre del departamento seleccionado
            string nombrePuesto = comboBox2.Text;
            var ids = EmpleadosDAO.ObtenerIdsPorNombrePuesto(nombrePuesto);
            Emp.idPuesto = ids.idPuesto;
            Emp.idDep = ids.idDepa;
            Emp.idEmp = ids.idEmp;
            Emp.FechaRegistro = DateTime.Now;

            if (Emp.idDep == -1 || Emp.idEmp == -1||Emp.idPuesto==-1)
            {
                MessageBox.Show("No se encontró el puesto seleccionado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Se inserta la info del domicilio
            Emp.calle = textBox11.Text;
            Emp.num = Convert.ToInt32(textBox12.Text);
            Emp.colonia = textBox13.Text;
            Emp.municipio = textBox14.Text;
            Emp.estado = textBox15.Text;
            Emp.codPost = Convert.ToInt32(textBox16.Text);

            // Insertar el nuevo puesto
            int resultado = EmpleadosDAO.IngresarEmp(Emp);

            if (resultado > 0)
            {
                MessageBox.Show("Puesto agregado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 🔄 Refrescar el DataGridView automáticamente
                EmpleadosDAO.MostrarEmp(dataGridView1);

                // (Opcional) limpiar los campos
                textBox1.Clear();
                textBox2.Clear();
                comboBox1.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Error al agregar el puesto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ModEmp_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Selecciona un empleado para modificar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idEmp = Convert.ToInt32(dataGridView1.CurrentRow.Cells["idEmpleado"].Value);
            Empleados puestoActual = EmpleadosDAO.ObtenerEmpleadoPorId(idEmp);

            if (puestoActual == null)
            {
                MessageBox.Show("No se encontró el empleado seleccionado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Crear el objeto del empleado
            Empleados Emp = new Empleados();

            // Captura de datos
            Emp.idEmpleados = Convert.ToInt32(textBox1.Text);
            Emp.contra = Convert.ToInt32(textBox2.Text);
            Emp.nombre = textBox3.Text;
            Emp.apellidos = textBox4.Text;
            Emp.salarioDr = Convert.ToDecimal(textBox5.Text);
            Emp.salarioInt = Convert.ToDecimal(textBox6.Text);
            Emp.FechaN = dateTimePicker1.Value.Date;
            Emp.CURP = textBox7.Text;
            Emp.RFC = textBox8.Text;
            Emp.email = textBox9.Text;
            Emp.NSS = textBox10.Text;
            Emp.banco = textBox17.Text;
            Emp.NumCuenta = Convert.ToInt32(textBox18.Text);
            Emp.telefono = Convert.ToInt32(textBox19.Text);

            // Buscar el idDepartamento, idEmpresa e idPuesto según el nombre del puesto
            string nombrePuesto = comboBox2.Text;
            var ids = EmpleadosDAO.ObtenerIdsPorNombrePuesto(nombrePuesto);
            Emp.idPuesto = ids.idPuesto;
            Emp.idDep = ids.idDepa;
            Emp.idEmp = ids.idEmp;

            if (Emp.idDep == -1 || Emp.idEmp == -1 || Emp.idPuesto == -1)
            {
                MessageBox.Show("No se encontró el puesto seleccionado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Domicilio
            Emp.calle = textBox11.Text;
            Emp.num = Convert.ToInt32(textBox12.Text);
            Emp.colonia = textBox13.Text;
            Emp.municipio = textBox14.Text;
            Emp.estado = textBox15.Text;
            Emp.codPost = Convert.ToInt32(textBox16.Text);

            int resultado = EmpleadosDAO.ModificarEmp(Emp);

            if (resultado > 0)
            {
                MessageBox.Show("Puesto modificado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //PuestoDAO.MostrarPuestos(dataGridView1);
                EmpleadosDAO.MostrarEmp(dataGridView1);
                textBox1.Clear();
                textBox2.Clear();
                comboBox1.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Error al modificar el puesto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ElimEmp_Click(object sender, EventArgs e)
        {
            // Verificar que haya una fila seleccionada
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Selecciona un empleado para eliminar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Crear el objeto del puesto
            Empleados Emp = new Empleados();
            Emp.idEmpleados = Convert.ToInt32(dataGridView1.CurrentRow.Cells["idEmpleado"].Value);
            /*Mod_puesto.namePuesto = textBox1.Text;
            Mod_puesto.Desc = textBox2.Text;*/

            // Buscar el idDepartamento y el idEmpresa según el nombre del departamento seleccionado
            /*string nombreDep = comboBox1.Text;
            var ids = PuestoDAO.ObtenerIdsPorNombreDepartamento(nombreDep);
            Mod_puesto.idDep = ids.idDepa;
            Mod_puesto.idEmp = ids.idEmp;

            if (Mod_puesto.idDep == -1 || Mod_puesto.idEmp == -1)
            {
                MessageBox.Show("No se encontró el departamento seleccionado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }*/

            // Insertar el nuevo puesto
            int resultado = EmpleadosDAO.DeletePuesto(Emp);
            //MessageBox.Show("Resultado del update: " + resultado);

            if (resultado > 0)
            {
                MessageBox.Show("Empleado eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 🔄 Refrescar el DataGridView automáticamente
                EmpleadosDAO.MostrarEmp(dataGridView1);

                // (Opcional) limpiar los campos
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
                textBox6.Clear();
                textBox7.Clear();
                textBox8.Clear();
                textBox9.Clear();
                textBox10.Clear();
                textBox11.Clear();
                textBox12.Clear();
                textBox13.Clear();
                textBox14.Clear();
                textBox15.Clear();
                textBox16.Clear();
                textBox17.Clear();
                textBox18.Clear();
                textBox19.Clear();

                comboBox1.SelectedIndex = -1;
                comboBox2.SelectedIndex = -1;
                comboBox3.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Error al eliminar el empleado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Menu

        private void departamentoToolStripMenuItem_Click(object sender, EventArgs e)
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
        private void empresaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rol == "Administrador")
            {
                Form4 f4 = new Form4();
                f4.Show();
                this.Hide();
            }
        }

        private void generalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form5 f5 = new Form5();
            f5.Show();
            this.Hide();
        }

        private void heartcounterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form6 f6 = new Form6();
            f6.Show();
            this.Hide();
        }

        private void generalToolStripMenuItem1_Click(object sender, EventArgs e)
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

        private void detalladaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form9 f9 = new Form9();
            f9.Show();
            this.Hide();
        }
        //
    }
}
