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
    public partial class inicioSesionAdmins : Form
    {
        public inicioSesionAdmins()
        {
            InitializeComponent();
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            Form1 frmInicioAlumno = new Form1();
            frmInicioAlumno.Show();
            this.Hide();
        }
    }
}
