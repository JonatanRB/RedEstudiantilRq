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
    public partial class frmInicioAlumno : Form
    {
        bool sidebarExpand;
        bool crearCollapse;
        private string nuaUsuarioActual;
        public frmInicioAlumno()
        {
            InitializeComponent();
        }

        public frmInicioAlumno(string nua)
        {
            InitializeComponent();
            nuaUsuarioActual = nua;
        }

        private void slidebarTimer_Tick(object sender, EventArgs e)
        {
            if (sidebarExpand)
            {
                sidebar.Width -= 10;
                if (sidebar.Width == sidebar.MinimumSize.Width)
                {
                    sidebarExpand = false;
                    slidebarTimer.Stop();
                }
            }
            else
            {
                sidebar.Width += 10;
                if (sidebar.Width == sidebar.MaximumSize.Width)
                {
                    sidebarExpand = true;
                    slidebarTimer.Stop();
                }
            }
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            slidebarTimer.Start();
        }

        private void btnInicio_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Form1 frmLogin = new Form1();
            frmLogin.Show();
            this.Hide();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            frmBuscar frmBuscar = new frmBuscar();
            frmBuscar.Show();
            this.Hide();
        }

        private void btnMensajes_Click(object sender, EventArgs e)
        {
            frmEmail frmEmail = new frmEmail();
            frmEmail.Show();
            this.Hide();
        }

        private void btnPerfil_Click(object sender, EventArgs e)
        {
            FormAlumno frmAlumno = new FormAlumno();
            frmAlumno.Show();
            this.Hide();
        }
    }
}
