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

namespace RedEstudiantilRoque
{
    public partial class FormHorario : Form
    {
        private int alumnoID;
        private string nuaAlumno;  // nuevo campo
        string connectionString = "Server=DESKTOP-8LL593G\\SQLEXPRESS;Database=RoqueSistema3;User Id=sa;Password=hola;";

        public FormHorario(int idAlumno, string nua)
        {
            InitializeComponent();
            alumnoID = idAlumno;
            nuaAlumno = nua;
        }

        private void FormHorario_Load(object sender, EventArgs e)
        {
            if (alumnoID <= 0)
            {
                MessageBox.Show("ID de alumno no válido.");
                return;
            }
            CargarDatosHorarios();
        }

    private void CargarDatosHorarios()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                   SELECT 
                        CONCAT(
                            LEFT(CONVERT(VARCHAR(8), H.HoraInicio, 108), 5), 
                            '-', 
                            LEFT(CONVERT(VARCHAR(8), H.HoraFin, 108), 5)
                        ) AS Horario,
                        MAX(CASE WHEN H.Dia = 'Lunes' THEN C.NombreClase ELSE '' END) AS Lunes,
                        MAX(CASE WHEN H.Dia = 'Martes' THEN C.NombreClase ELSE '' END) AS Martes,
                        MAX(CASE WHEN H.Dia = 'Miércoles' THEN C.NombreClase ELSE '' END) AS Miércoles,
                        MAX(CASE WHEN H.Dia = 'Jueves' THEN C.NombreClase ELSE '' END) AS Jueves,
                        MAX(CASE WHEN H.Dia = 'Viernes' THEN C.NombreClase ELSE '' END) AS Viernes
                    FROM 
                        Calificaciones CA
                    INNER JOIN Clases C ON CA.ClaseID = C.ClaseID
                    INNER JOIN Horarios H ON C.ClaseID = H.ClaseID
                    WHERE 
                        CA.AlumnoID = @AlumnoID
                    GROUP BY H.HoraInicio, H.HoraFin
                    ORDER BY H.HoraInicio;";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@AlumnoID", alumnoID);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    dataGridView1.AutoGenerateColumns = true;
                    dataGridView1.DataSource = dt;


                    if (dataGridView1.Columns.Contains("Horario"))
                        dataGridView1.Columns["Horario"].DisplayIndex = 0;

                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                }
                else
                {
                    MessageBox.Show("No hay horarios registrados para este alumno.");
                }
            }
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            FormAlumno frmAlum = new FormAlumno(nuaAlumno);
            frmAlum.Show();
            this.Hide();
        }
    } 
}
