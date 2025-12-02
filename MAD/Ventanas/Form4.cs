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
    public partial class Form4: Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        string rol = SesionActual.Rol;

        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void InsertEmpBtn_Click(object sender, EventArgs e)
        {

            Empresa empresa = new Empresa();
            empresa.NameEmp = textBox1.Text;
            empresa.RazonSoc = textBox2.Text;
            empresa.DomFiscal = textBox3.Text;
            empresa.DContact = textBox4.Text;
            empresa.RegPatronal = textBox5.Text;
            empresa.RFC = textBox6.Text;

            empresa.FInOper = dateTimePicker1.Value.Date;

            //if (generoRdH.Checked)
            //    usuario.genero = 0;
            //else
            //    usuario.genero = 1;

            EmpresaDAO.InsertarEmp(empresa);
            MessageBox.Show("Se ha registrado la empresa correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //refrescar();
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
            //Form4 f4 = new Form4();
            //f4.Show();
            //this.Hide();
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
        //
    }
}
