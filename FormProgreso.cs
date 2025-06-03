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
    public partial class FormProgreso : Form
    {
        public FormProgreso()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            frmInicioAlumno frmInicioAlumno = new frmInicioAlumno();
            frmInicioAlumno.Show();
            this.Hide();
        }
    }
}
