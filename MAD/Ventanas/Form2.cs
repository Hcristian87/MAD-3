using MAD.Entidad;
using MAD.DAO;
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
    public partial class Form2: Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            DepartamentosDAO.LlenarComboEmpresas(comboBox1);
            DepartamentosDAO.MostrarDepartamentos(dataGridView1);
        }

        string rol = SesionActual.Rol;


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verifica que no se haya hecho clic en el encabezado
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dataGridView1.Rows[e.RowIndex];

                // Llena los campos del formulario con los datos de la fila seleccionada
                textBox1.Text = fila.Cells["nombreDepartamento"].Value.ToString();
                comboBox1.Text = fila.Cells["Empresa"].Value.ToString();
            }
        }

        private void InsertDep_Click(object sender, EventArgs e)
        {
            // Crear el objeto del departamento
            Departamentos depa = new Departamentos();
            depa.NameDep = textBox1.Text;

            // Buscar el idEmpresa según el nombre seleccionado en el ComboBox
            string nombreEmpresa = comboBox1.Text;
            depa.idEmp = DepartamentosDAO.ObtenerIdEmpresaPorNombre(nombreEmpresa);

            if (depa.idEmp == -1)
            {
                MessageBox.Show("No se encontró la empresa seleccionada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Insertar el nuevo departamento
            int resultado = DepartamentosDAO.IngresarDep(depa);

            if (resultado > 0)
            {
                MessageBox.Show("Departamento agregado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 🔄 Refrescar el DataGridView automáticamente
                DepartamentosDAO.MostrarDepartamentos(dataGridView1);

                // (Opcional) limpiar los campos
                textBox1.Clear();
                comboBox1.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Error al agregar el departamento.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ModifDep_Click(object sender, EventArgs e)
        {
            // Verificar que haya una fila seleccionada
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Selecciona un departamento para modificar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Crear el objeto del departamento
            Departamentos Mod_depa = new Departamentos();
            Mod_depa.idDep = Convert.ToInt32(dataGridView1.CurrentRow.Cells["idDepartamento"].Value);
            Mod_depa.NameDep = textBox1.Text;

            // Buscar el idEmpresa según el nombre seleccionado en el ComboBox
            string nombreEmpresa = comboBox1.Text;
            Mod_depa.idEmp = DepartamentosDAO.ObtenerIdEmpresaPorNombre(nombreEmpresa);

            if (Mod_depa.idEmp == -1)
            {
                MessageBox.Show("No se encontró la empresa seleccionada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Ejecutar la actualización
            int resultado = DepartamentosDAO.ModificarDep(Mod_depa);

            if (resultado > 0)
            {
                MessageBox.Show("Departamento actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 🔄 Refrescar el DataGridView automáticamente
                DepartamentosDAO.MostrarDepartamentos(dataGridView1);

                //Limpia los campos
                textBox1.Clear();
                comboBox1.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Error al actualizar el departamento.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteDep_Click(object sender, EventArgs e)
        {
            // Verificar que haya una fila seleccionada
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Selecciona un departamento para modificar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Crear el objeto del departamento
            Departamentos Del_depa = new Departamentos();
            Del_depa.idDep = Convert.ToInt32(dataGridView1.CurrentRow.Cells["idDepartamento"].Value);
            //Mod_depa.NameDep = textBox1.Text;

            // Buscar el idEmpresa según el nombre seleccionado en el ComboBox
            /*string nombreEmpresa = comboBox1.Text;
            Mod_depa.idEmp = DepartamentosDAO.ObtenerIdEmpresaPorNombre(nombreEmpresa);

            if (Mod_depa.idEmp == -1)
            {
                MessageBox.Show("No se encontró la empresa seleccionada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }*/

            // Ejecutar la eliminación
            int resultado = DepartamentosDAO.DeleteDep(Del_depa);

            if (resultado > 0)
            {
                MessageBox.Show("Departamento eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 🔄 Refrescar el DataGridView automáticamente
                DepartamentosDAO.MostrarDepartamentos(dataGridView1);

                //Limpia los campos
                textBox1.Clear();
                comboBox1.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Error al eliminar el departamento.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Menu
        private void empleadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reg_Empl f1 = new Reg_Empl();
            f1.Show();
            this.Hide();
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

        private void departamentosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //Form2 f2 = new Form2();
            //f2.Show();
            //this.Hide();
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
