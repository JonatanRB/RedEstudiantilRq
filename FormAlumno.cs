using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;

namespace RedEstudiantilRoque
{
    public partial class FormAlumno : Form
    {
        private int alumnoID;
        private string nuaAlumno;
        string connectionString = "Server=DESKTOP-8LL593G\\SQLEXPRESS;Database=RoqueSistema2;User Id=sa;Password=hola;";
        public FormAlumno(string nua)
        {
            InitializeComponent();
            nuaAlumno = nua;
        }

        public FormAlumno(int alumnoID, string nua)
        {
            InitializeComponent();
            this.alumnoID = alumnoID;
            nuaAlumno = nua;
        }

        void CargarInformacionAlumno(string nua)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(@"
            SELECT u.Nombre, u.Descripcion, u.FotoPerfil, 
                   a.Nua, a.Skills, a.Logros, a.AlumnoID
            FROM Alumnos a
            INNER JOIN Usuarios u ON a.UsuarioID = u.UsuarioID
            WHERE a.Nua = @nua", conn);

                cmd.Parameters.AddWithValue("@nua", nua);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    lblNombre.Text = reader["Nombre"].ToString();
                    lblNua.Text = reader["Nua"].ToString();
                    lblDescripcion.Text = reader["Descripcion"].ToString();

                    alumnoID = Convert.ToInt32(reader["AlumnoID"]);

                    string rutaFoto = reader["FotoPerfil"].ToString();
                    if (!string.IsNullOrEmpty(rutaFoto) && File.Exists(rutaFoto))
                    {
                        pictureBoxFoto.Image = Image.FromFile(rutaFoto);
                        pictureBoxFoto.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                    else
                    {
                        pictureBoxFoto.Image = null;
                    }

                    // Aquí llamamos a la función que crea etiquetas en los paneles
                    string skills = reader["Skills"].ToString();
                    string logros = reader["Logros"].ToString();

                    CargarSkillsYLogros(skills, logros);
                }
                else
                {
                    MessageBox.Show("Alumno no encontrado.");
                }

                reader.Close();
            }
        }



        private void btnRegresar_Click(object sender, EventArgs e)
        {
            frmInicioAlumno frmInicioAlumno = new frmInicioAlumno();
            frmInicioAlumno.Show();
            this.Hide();
        }

        private void CargarSkillsYLogros(string nua)
        {
            string connectionString = "Server=DESKTOP-8LL593G\\SQLEXPRESS;Database=RoqueSistema2;User Id=sa;Password=hola;";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT Skills, Logros FROM Alumnos WHERE AlumnoID = @AlumnoID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@AlumnoID", alumnoID);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string skills = reader["Skills"].ToString();
                    string logros = reader["Logros"].ToString();

                    listBoxSkils.Items.Clear();
                    listBoxLogros.Items.Clear();

                    foreach (var skill in skills.Split(new[] { ',', ';', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        string texto = skill.Trim();
                        if (!string.IsNullOrEmpty(texto))
                            listBoxSkils.Items.Add("✔️ " + CapitalizarPalabras(texto));
                    }

                    foreach (var logro in logros.Split(new[] { ',', ';', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        string texto = logro.Trim();
                        if (!string.IsNullOrEmpty(texto))
                            listBoxLogros.Items.Add("⭐ " + CapitalizarPalabras(texto));
                    }
                }
                else
                {
                    MessageBox.Show("No se encontraron skills ni logros para este alumno.");
                }

                reader.Close();
            }


        }
        string CapitalizarPalabras(string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return texto;

            var palabras = texto.Split(' ');
            for (int i = 0; i < palabras.Length; i++)
            {
                if (palabras[i].Length > 0)
                    palabras[i] = char.ToUpper(palabras[i][0]) + palabras[i].Substring(1).ToLower();
            }
            return string.Join(" ", palabras);
        }

        private void FormAlumno_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(nuaAlumno))
            {
                CargarInformacionAlumno(nuaAlumno);
            }
            else
            {
                MessageBox.Show("No se ha especificado un NUA para cargar datos.");
            }
        }
    }
}

