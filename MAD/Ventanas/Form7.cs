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
    public partial class Form7: Form
    {
        public Form7()
        {
            InitializeComponent();
        }
        string rol = SesionActual.Rol;


        private void Form7_Load(object sender, EventArgs e)
        {

        }

        private int MesANumero(string mes)
        {
            switch (mes)
            {
                case "Enero": return 1;
                case "Febrero": return 2;
                case "Marzo": return 3;
                case "Abril": return 4;
                case "Mayo": return 5;
                case "Junio": return 6;
                case "Julio": return 7;
                case "Agosto": return 8;
                case "Septiembre": return 9;
                case "Octubre": return 10;
                case "Noviembre": return 11;
                case "Diciembre": return 12;
                default: return 0;
            }
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            int mes = MesANumero(comboBox1.Text);
            int anio = int.Parse(comboBox2.Text);

            dataGridView1.DataSource = ReporteGenlNominaDAO.ObtenerListadoEmpleados(mes, anio);
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
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
            //Form7 f7 = new Form7();
            //f7.Show();
            //this.Hide();
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
