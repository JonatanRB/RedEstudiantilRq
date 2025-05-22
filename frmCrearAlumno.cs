using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RedEstudiantilRoque.Data;
using System.Data;
using System.Data.SqlClient;

namespace RedEstudiantilRoque
{
    public partial class frmCrearAlumno : Form
    {
        public frmCrearAlumno()
        {
            InitializeComponent();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            DataBaseManagmet db = new DataBaseManagmet();
            db.conectar();
            SqlConnection conn = db.obtenerConexion();

            string tipoUsuario = rdbAlumno.Checked ? "Alumno" : rdbProfesor.Checked ? "Maestro" : "";

            try
            {
                
                string insertUsuarioQuery = @"INSERT INTO Usuarios (Nombre, Correo, Contraseña, TipoUsuario, Descripcion) 
                                      OUTPUT INSERTED.UsuarioID
                                      VALUES (@Nombre, @Correo, @Contraseña, @TipoUsuario, @Descripcion)";

                SqlCommand cmdInsertUsuario = new SqlCommand(insertUsuarioQuery, conn);
                cmdInsertUsuario.Parameters.AddWithValue("@Nombre", txtNombre.Text);
                cmdInsertUsuario.Parameters.AddWithValue("@Correo", txtCorreo.Text);
                cmdInsertUsuario.Parameters.AddWithValue("@Contraseña", txtContrasena.Text);
                cmdInsertUsuario.Parameters.AddWithValue("@TipoUsuario", tipoUsuario);
                cmdInsertUsuario.Parameters.AddWithValue("@Descripcion", txtDescripcion.Text);

                int usuarioID = (int)cmdInsertUsuario.ExecuteScalar();

                
                if (tipoUsuario == "Alumno")
                {
                    string insertAlumnoQuery = @"INSERT INTO Alumnos (UsuarioID, Nua) VALUES (@UsuarioID, @Nua)";
                    SqlCommand cmdInsertAlumno = new SqlCommand(insertAlumnoQuery, conn);
                    cmdInsertAlumno.Parameters.AddWithValue("@UsuarioID", usuarioID);
                    cmdInsertAlumno.Parameters.AddWithValue("@Nua", txtNua.Text);
                    cmdInsertAlumno.ExecuteNonQuery();
                }

                MessageBox.Show("Usuario guardado correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                db.desconectar();
            }
        }

    }
}
