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
    public partial class Form10: Form
    {
        public Form10()
        {
            InitializeComponent();
        }

        private void btnIniciarSesion_Click(object sender, EventArgs e)
        {
            string correo = textBox1.Text;
            string pass = textBox2.Text;
            string rol = radioButton1.Checked ? "Administrador" :
                         radioButton2.Checked ? "Auxiliar" : "";

            if (string.IsNullOrEmpty(rol))
            {
                MessageBox.Show("Selecciona un rol (Administrador o Auxiliar).");
                return;
            }

            var empleado = InicioSesiónDAO.ValidarLogin(correo, pass);

            if (empleado == null)
            {
                MessageBox.Show("Correo o contraseña incorrectos.");
                return;
            }

            // Guardar la sesión global
            SesionActual.IdEmpleado = empleado.idEmpleados;
            SesionActual.NombreEmpleado = empleado.nombre + " " + empleado.apellidos;
            SesionActual.Rol = rol;

            MessageBox.Show($"Bienvenido {SesionActual.NombreEmpleado}\nRol: {SesionActual.Rol}");

            // Abrir otro form
            Reg_Empl frm = new Reg_Empl();
            frm.Show();
            this.Hide();
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form10_Load(object sender, EventArgs e)
        {

        }
    }
}
