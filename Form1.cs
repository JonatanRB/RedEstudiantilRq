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
using RedEstudiantilRoque.Modelo;
using static RedEstudiantilRoque.FormAvanceAlumno;
using System.Drawing.Drawing2D;

namespace RedEstudiantilRoque
{
    
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void EstilizarBoton(Button boton)
        {
            boton.FlatStyle = FlatStyle.Flat;
            boton.FlatAppearance.BorderSize = 0;
            boton.BackColor = Color.FromArgb(255, 253, 151);
            boton.ForeColor = Color.Black;
            boton.Font = new Font("Segoe UI", 10);
            boton.Size = new Size(148, 30);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            EstilizarBoton(btnAcceder);
            EstilizarBoton(btnAccesoAdmin);
            EstilizarBoton(btnCalendario);
            RedondearBoton(btnAcceder, 15);
            RedondearBoton(btnAccesoAdmin, 15);
            RedondearBoton(btnCalendario, 15);
        }

        private void btnAccesoAdmin_Click(object sender, EventArgs e)
        {
            inicioSesionAdmins frmini = new inicioSesionAdmins();
            frmini.Show();
            this.Hide();    
        }

        private void btnAcceder_Click(object sender, EventArgs e)
        {
            string matricula = txtMatricula.Text;
            string contrasena = txtContrasena.Text;

            string connectionString = "Data Source=DESKTOP-8LL593G\\SQLEXPRESS;Initial Catalog=RoqueSistema3;User ID=sa;Password=hola;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = @"
            SELECT u.UsuarioID, u.Nombre, a.Nua
            FROM Alumnos a
            INNER JOIN Usuarios u ON a.UsuarioID = u.UsuarioID
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

                        Session.UsuarioID = usuarioID;


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

        private void lblRecuperarContrasena_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void RedondearBoton(Button btn, int radio = 20)
        {
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(new Rectangle(0, 0, radio, radio), 180, 90);
            path.AddArc(new Rectangle(btn.Width - radio, 0, radio, radio), 270, 90);
            path.AddArc(new Rectangle(btn.Width - radio, btn.Height - radio, radio, radio), 0, 90);
            path.AddArc(new Rectangle(0, btn.Height - radio, radio, radio), 90, 90);
            path.CloseFigure();
            btn.Region = new Region(path);
        }
    }
}
