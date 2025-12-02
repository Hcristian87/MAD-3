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
    public partial class Form3: Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        string rol = SesionActual.Rol;

        private void Form3_Load(object sender, EventArgs e)
        {
            PuestoDAO.LlenarComboEmpresas(comboBox2);
            PuestoDAO.MostrarPuestos(dataGridView1);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verifica que no se haya hecho clic en el encabezado
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dataGridView1.Rows[e.RowIndex];

                // Llena los campos del formulario con los datos de la fila seleccionada
                textBox1.Text = fila.Cells["Nombre"].Value.ToString();
                textBox2.Text = fila.Cells["Descripción"].Value.ToString();
                comboBox1.Text = fila.Cells["Departamento"].Value.ToString();
                comboBox2.Text = fila.Cells["Empresa"].Value.ToString();
            }
        }

        private void comboEmpresas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedValue != null && int.TryParse(comboBox2.SelectedValue.ToString(), out int idEmp))
            {
                PuestoDAO.LlenarComboDepartamentos(comboBox1, idEmp);
            }
        }

        private void InsertPuesto_Click(object sender, EventArgs e)
        {
            // Crear el objeto del puesto
            Puesto puesto = new Puesto();
            puesto.namePuesto = textBox1.Text;
            puesto.Desc = textBox2.Text;

            // Buscar el idDepartamento y el idEmpresa según el nombre del departamento seleccionado
            string nombreDep = comboBox1.Text;
            var ids = PuestoDAO.ObtenerIdsPorNombreDepartamento(nombreDep);
            puesto.idDep = ids.idDepa;
            puesto.idEmp = ids.idEmp;

            if (puesto.idDep == -1 || puesto.idEmp == -1)
            {
                MessageBox.Show("No se encontró el departamento seleccionado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Insertar el nuevo puesto
            int resultado = PuestoDAO.IngresarPuesto(puesto);

            if (resultado > 0)
            {
                MessageBox.Show("Puesto agregado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 🔄 Refrescar el DataGridView automáticamente
                PuestoDAO.MostrarPuestos(dataGridView1);

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

        /*private void ModifPuesto_Click(object sender, EventArgs e)
        {
            // Verificar que haya una fila seleccionada
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Selecciona un puesto para modificar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Crear el objeto del puesto
            Puesto Mod_puesto = new Puesto();
            Mod_puesto.idPuesto = Convert.ToInt32(dataGridView1.CurrentRow.Cells["idPuesto"].Value);
            Mod_puesto.namePuesto = textBox1.Text;
            Mod_puesto.Desc = textBox2.Text;

            // Buscar el idDepartamento y el idEmpresa según el nombre del departamento seleccionado
            string nombreDep = comboBox1.Text;
            var ids = PuestoDAO.ObtenerIdsPorNombreDepartamento(nombreDep);
            Mod_puesto.idDep = ids.idDepa;
            Mod_puesto.idEmp = ids.idEmp;

            if (Mod_puesto.idDep == -1 || Mod_puesto.idEmp == -1)
            {
                MessageBox.Show("No se encontró el departamento seleccionado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Insertar el nuevo puesto
            int resultado = PuestoDAO.ModificarPuesto(Mod_puesto);
            //MessageBox.Show("Resultado del update: " + resultado);

            if (resultado > 0)
            {
                MessageBox.Show("Puesto modificado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 🔄 Refrescar el DataGridView automáticamente
                PuestoDAO.MostrarPuestos(dataGridView1);

                // (Opcional) limpiar los campos
                textBox1.Clear();
                textBox2.Clear();
                comboBox1.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Error al modificar el puesto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }*/

        private void ModifPuesto_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Selecciona un puesto para modificar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idPuesto = Convert.ToInt32(dataGridView1.CurrentRow.Cells["idPuesto"].Value);
            Puesto puestoActual = PuestoDAO.ObtenerPuestoPorId(idPuesto);

            if (puestoActual == null)
            {
                MessageBox.Show("No se encontró el puesto seleccionado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Crear objeto con los posibles cambios
            Puesto Mod_puesto = new Puesto();
            Mod_puesto.idPuesto = idPuesto;
            Mod_puesto.namePuesto = textBox1.Text;
            Mod_puesto.Desc = textBox2.Text;

            // Si el usuario cambió el combo, actualiza los IDs
            string nombreDep = comboBox1.Text;
            if (!string.IsNullOrWhiteSpace(nombreDep))
            {
                var ids = PuestoDAO.ObtenerIdsPorNombreDepartamento(nombreDep);
                Mod_puesto.idDep = ids.idDepa;
                Mod_puesto.idEmp = ids.idEmp;
            }
            else
            {
                // Si no cambió, conserva los originales
                Mod_puesto.idDep = puestoActual.idDep;
                Mod_puesto.idEmp = puestoActual.idEmp;
            }

            int resultado = PuestoDAO.ModificarPuesto(Mod_puesto);

            if (resultado > 0)
            {
                MessageBox.Show("Puesto modificado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                PuestoDAO.MostrarPuestos(dataGridView1);
                textBox1.Clear();
                textBox2.Clear();
                comboBox1.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Error al modificar el puesto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ElimPuesto_Click(object sender, EventArgs e)
        {
            // Verificar que haya una fila seleccionada
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Selecciona un puesto para eliminar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Crear el objeto del puesto
            Puesto Elim_puesto = new Puesto();
            Elim_puesto.idPuesto = Convert.ToInt32(dataGridView1.CurrentRow.Cells["idPuesto"].Value);
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
            int resultado = PuestoDAO.DeletePuesto(Elim_puesto);
            //MessageBox.Show("Resultado del update: " + resultado);

            if (resultado > 0)
            {
                MessageBox.Show("Puesto eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 🔄 Refrescar el DataGridView automáticamente
                PuestoDAO.MostrarPuestos(dataGridView1);

                // (Opcional) limpiar los campos
                textBox1.Clear();
                textBox2.Clear();
                comboBox1.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Error al eliminar el puesto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            //Form3 f3 = new Form3();
            //f3.Show();
            //this.Hide();
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
            Form5 f5 = new Form5();
            f5.Show();
            this.Hide();
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

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        //


    }
}
