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
        string connectionString = "Server=DESKTOP-8LL593G\\SQLEXPRESS;Database=RoqueSistema3;User Id=sa;Password=hola;";
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
        void CargarSkillsYLogros(string skills, string logros)
        {
            listBoxSkils.Items.Clear();
            listBoxLogros.Items.Clear();

            foreach (var skill in skills.Split(new[] { ',', ';', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                listBoxSkils.Items.Add(skill.Trim());
            }

            foreach (var logro in logros.Split(new[] { ',', ';', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                listBoxLogros.Items.Add(logro.Trim());
            }
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
        void AgregarEtiquetas(FlowLayoutPanel panel, string datos)
        {
            panel.Controls.Clear();
            var elementos = datos.Split(new[] { ',', ';', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in elementos)
            {
                Label lbl = new Label();
                lbl.Text = item.Trim();
                lbl.BackColor = Color.Teal;
                lbl.ForeColor = Color.White;
                lbl.Padding = new Padding(5, 3, 5, 3);
                lbl.Margin = new Padding(4);
                lbl.AutoSize = true;
                lbl.Font = new Font("Segoe UI", 9, FontStyle.Regular);
                panel.Controls.Add(lbl);
            }
        }


        private void btnReticula1_Click(object sender, EventArgs e)
        {
            if (alumnoID <= 0)
            {
                MessageBox.Show("No se ha cargado correctamente el ID del alumno.");
                return;
            }

            FormReticula frmReticula = new FormReticula(alumnoID, nuaAlumno);
            frmReticula.ShowDialog();
            this.Hide();
        }

        private void btnProgreso_Click(object sender, EventArgs e)
        {
            FormProgreso frmProgreso = new FormProgreso();
            frmProgreso.Show();
            this.Hide();
        }

        private void btnReticula_Click(object sender, EventArgs e)
        {
            //Este es el boton para cargar el horario
            if (alumnoID <= 0)
            {
                MessageBox.Show("No se ha cargado correctamente el ID del alumno.");
                return;
            }

            FormHorario frmHorario = new FormHorario(alumnoID, nuaAlumno);
            frmHorario.Show();
            this.Hide();
        }

        private void CargarSkillsYLogros(string nua)
        {
            string connectionString = "Server=LEONEL\\SQLEXPRESS;Database=RoqueSistema3;User Id=sa;Password=hola;";
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
    }

}
    


