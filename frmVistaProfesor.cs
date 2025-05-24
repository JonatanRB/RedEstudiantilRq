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
        public frmVistaProfesor()
        {
            InitializeComponent();
        }

        private void panel9_Paint(object sender, PaintEventArgs e)
        {
            //borra metodo no sirve
        }

        public void cargarMaestro()
        {
            /*
            maestroIDSeleccionado = Convert.ToInt32(dgvAlumnos.SelectedRows[0].Cells["AlumnoID"].Value);

            DataBaseManagmet db = new DataBaseManagmet();
                db.conectar();
                SqlConnection conn = db.obtenerConexion();

                try
                {
                    string query = @"SELECT U.Nombre, U.Correo, U.Descripcion, M.Matricula
                             FROM Maestros M 
                             JOIN Usuarios U ON (M.UsuarioID = U.UsuarioID) 
                             WHERE M.MaestroID = @MaestroID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MaestroID", maestroIDSeleccionado);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        txtNua.Text = reader["Nua"].ToString();
                        txtNombre.Text = reader["Nombre"].ToString();
                        txtCorreo.Text = reader["Correo"].ToString();
                        txtDescripcion.Text = reader["Descripcion"].ToString();
                    }
                    reader.Close();

                    tabControl1.SelectedTab = tabPage2;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar alumno: " + ex.Message);
                }
                finally
                {
                    db.desconectar();
                }
            */
            
        }

        private void frmVistaProfesor_Load(object sender, EventArgs e)
        {

        }
    }
}
