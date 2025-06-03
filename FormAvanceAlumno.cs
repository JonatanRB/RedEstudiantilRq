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
using System.Windows.Forms.DataVisualization.Charting;

namespace RedEstudiantilRoque
{
    public partial class FormAvanceAlumno : Form
    {
        private int alumnoID;
        private string nuaAlumno;
        private SqlConnection con = new SqlConnection("Data Source=DESKTOP-8LL593G\\SQLEXPRESS;Initial Catalog=RoqueSistema3;User ID=sa;Password=hola;");
        public FormAvanceAlumno(int idAlumno, string nua)
        {
            InitializeComponent();
            CargarEstadisticas();
            alumnoID = idAlumno;
            nuaAlumno = nua;
        }

        private void comboBoxAlumnos_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private void roundedPanel4_Paint(object sender, PaintEventArgs e)
        {
        }
        private void label10_Click(object sender, EventArgs e)
        {

        }
        private void FormAvanceAlumno_Load(object sender, EventArgs e)
        {
            CargarEstadisticas();
        }

        private void CargarEstadisticas()
        {
            con.Open();

            // Datos personales
            SqlCommand cmdInfo = new SqlCommand(@"SELECT a.Nua, u.Nombre, u.Correo 
                                                  FROM Alumnos a 
                                                  JOIN Usuarios u ON a.UsuarioID = u.UsuarioID 
                                                  WHERE a.AlumnoID = @AlumnoID", con);
            cmdInfo.Parameters.AddWithValue("@AlumnoID", alumnoID);
            SqlDataReader reader = cmdInfo.ExecuteReader();
            if (reader.Read())
            {
                lblNua.Text = reader["Nua"].ToString();
                lblNombre.Text = reader["Nombre"].ToString();
                lblCorreo.Text = reader["Correo"].ToString();
            }
            reader.Close();

            // Promedio general
            SqlCommand cmdPromedio = new SqlCommand("SELECT AVG(Calificacion) FROM Calificaciones WHERE AlumnoID = @AlumnoID", con);
            cmdPromedio.Parameters.AddWithValue("@AlumnoID", alumnoID);
            var promedio = cmdPromedio.ExecuteScalar();
            lblPromedio.Text = promedio != DBNull.Value ? Convert.ToDecimal(promedio).ToString("F2") : "N/A";

            // Materias reprobadas
            SqlCommand cmdReprobadas = new SqlCommand("SELECT COUNT(*) FROM Calificaciones WHERE AlumnoID = @AlumnoID AND Calificacion < 70", con);
            cmdReprobadas.Parameters.AddWithValue("@AlumnoID", alumnoID);
            lblReprobadas.Text = cmdReprobadas.ExecuteScalar().ToString();

            // Créditos aprobados
            SqlCommand cmdCreditosAprobados = new SqlCommand(@"
                SELECT ISNULL(SUM(cl.Creditos), 0)
                FROM Calificaciones c
                JOIN Clases cl ON c.ClaseID = cl.ClaseID
                WHERE c.AlumnoID = @AlumnoID AND c.Calificacion >= 70", con);
            cmdCreditosAprobados.Parameters.AddWithValue("@AlumnoID", alumnoID);
            int creditosAprobados = Convert.ToInt32(cmdCreditosAprobados.ExecuteScalar());

            // Créditos totales
            SqlCommand cmdTotalCreditos = new SqlCommand("SELECT ISNULL(SUM(Creditos), 0) FROM Clases", con);
            int totalCreditos = Convert.ToInt32(cmdTotalCreditos.ExecuteScalar());

            // Materias aprobadas
            SqlCommand cmdMateriasAprobadas = new SqlCommand("SELECT COUNT(*) FROM Calificaciones WHERE AlumnoID = @AlumnoID AND Calificacion >= 70", con);
            cmdMateriasAprobadas.Parameters.AddWithValue("@AlumnoID", alumnoID);
            int materiasAprobadas = Convert.ToInt32(cmdMateriasAprobadas.ExecuteScalar());

            // Total materias
            SqlCommand cmdTotalMaterias = new SqlCommand("SELECT COUNT(*) FROM Clases", con);
            int totalMaterias = Convert.ToInt32(cmdTotalMaterias.ExecuteScalar());

            // Progreso en barras
            progressCreditos.Maximum = totalCreditos;
            progressCreditos.Value = Math.Min(creditosAprobados, totalCreditos);

            progressMaterias.Maximum = totalMaterias;
            progressMaterias.Value = Math.Min(materiasAprobadas, totalMaterias);

            // Gráfico por semestre
            SqlCommand cmdGrafica = new SqlCommand(@"
                SELECT cl.Semestre, AVG(c.Calificacion) AS PromedioSemestre
                FROM Calificaciones c
                JOIN Clases cl ON c.ClaseID = cl.ClaseID
                WHERE c.AlumnoID = @AlumnoID
                GROUP BY cl.Semestre
                ORDER BY cl.Semestre", con);
            cmdGrafica.Parameters.AddWithValue("@AlumnoID", alumnoID);
            SqlDataReader grafReader = cmdGrafica.ExecuteReader();

            chartCalificaciones.Series["Series1"].Points.Clear();
            while (grafReader.Read())
            {
                chartCalificaciones.Series["Series1"].Points.AddXY(
                    "Semestre " + grafReader["Semestre"].ToString(),
                    Convert.ToDouble(grafReader["PromedioSemestre"])
                );
            }
            grafReader.Close();

            con.Close();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            frmInicioAlumno frminicio = new frmInicioAlumno();
            frminicio.Show();
            this.Hide();
        }
    }
}


