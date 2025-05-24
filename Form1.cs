using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

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
            string matricula = txtMatricula.Text;
            string contrasena = txtContrasena.Text;

            string connectionString = "Data Source=DESKTOP-8LL593G\\SQLEXPRESS;Initial Catalog=SistemaRoque2;User ID=sa;Password=hola;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = @"
            SELECT u.UsuarioID, u.Nombre, a.Nua
            FROM Alumnos a
            INNER JOIN Usuarios u ON (a.UsuarioID = u.UsuarioID)
            WHERE a.Nua = @Nua AND u.Contraseña = @Contrasena AND u.TipoUsuario = 'Alumno'";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Nua", matricula);
                    command.Parameters.AddWithValue("@Contrasena", contrasena);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        int usuarioID = reader.GetInt32(0);
                        string nombre = reader.GetString(1);
                        string nua = reader.GetString(2);

                        MessageBox.Show("Bienvenido, " + nombre);

                        // PASAR EL NUA AL FORMULARIO INICIO
                        frmInicioAlumno formMainAlumno = new frmInicioAlumno(nua);
                        formMainAlumno.Show();
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
