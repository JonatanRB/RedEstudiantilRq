using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace RedEstudiantilRoque
{
    public partial class inicioSesionAdmins : Form
    {
        private string nuaUsuarioActual;
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

        private void btnAcceder_Click(object sender, EventArgs e)
        {
            string matricula = txtNoIdentificacion.Text;
            string contrasena = txtContrasena.Text;

            string connectionString = "Data Source=DESKTOP-8LL593G\\SQLEXPRESS;Initial Catalog=SistemaRoque2;User ID=sa;Password=hola;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = @"
            SELECT u.UsuarioID, u.Nombre, m.Matricula
            FROM Maestros m
            INNER JOIN Usuarios u ON (m.UsuarioID = u.UsuarioID)
            WHERE m.Matricula = @Matricula AND u.Contraseña = @Contrasena AND u.TipoUsuario = 'Maestro'";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Matricula", matricula);
                    command.Parameters.AddWithValue("@Contrasena", contrasena);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        int usuarioID = reader.GetInt32(0);
                        string nombre = reader.GetString(1);
                        string nua = reader.GetString(2);

                        MessageBox.Show("Bienvenido, " + nombre);

                        // PASAR EL NUA AL FORMULARIO INICIO
                        Inicio formMain = new Inicio(nua);
                        formMain.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Matrícula o contraseña incorrectos.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
    }
}
