using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedEstudiantilRoque
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Agregar inicializacion de base de datos
        }

        private void btnAccesoAdmin_Click(object sender, EventArgs e)
        {
            inicioSesionAdmins formAdmins = new inicioSesionAdmins();
            formAdmins.Show();
            this.Hide();
        }

        private void btnAcceder_Click(object sender, EventArgs e)
        {
            Inicio formMain = new Inicio();
            formMain.Show();
            this.Hide();
        }
    }
}
