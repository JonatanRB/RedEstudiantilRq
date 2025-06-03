using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RedEstudiantilRoque.Data;

namespace RedEstudiantilRoque
{
    public partial class frmVistaProfesor : Form
    {
        private int usuarioId;
        string connectionString = "Server=DESKTOP-8LL593G\\SQLEXPRESS;Database=RoqueSistema3;User Id=sa;Password=hola;";
        public frmVistaProfesor()
        {
            InitializeComponent();
        }

        public frmVistaProfesor(int alumnoID, string matricula)
        {
            InitializeComponent();
        }

        private void panel9_Paint(object sender, PaintEventArgs e)
        {
            //borra metodo no sirve
        }

        private void CargarInformacionMaestro(int id)
        {
            string conexion = "TuCadenaDeConexion";

            using (SqlConnection conn = new SqlConnection(conexion))
            {
                conn.Open();
                string query = @"
                SELECT u.Nombre, u.Correo, u.FotoPerfil, u.Descripcion, m.Matricula
                FROM Maestros m
                INNER JOIN Usuarios u ON m.UsuarioID = u.UsuarioID
                WHERE u.UsuarioID = @UsuarioID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UsuarioID", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lblNombre.Text = reader["Nombre"].ToString();
                            lblCorreo.Text = reader["Correo"].ToString();
                            lblMatricula.Text = reader["Matricula"].ToString();
                            lblDescripcion.Text = reader["Descripcion"].ToString();

                        }
                    }
                }
            }
        }


        private void frmVistaProfesor_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Form1 frm = new Form1();    
            frm.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Inicio frminicio = new Inicio();
            frminicio.Show();
            this.Hide();
        }
    }
}
