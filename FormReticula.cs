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
    public partial class FormReticula : Form
    {
        private string nuaAlumno;
        private int alumnoID;
        private string connectionString = "Server=DESKTOP-8LL593G\\SQLEXPRESS;Database=RoqueSistema3;User Id=sa;Password=hola;";
        public FormReticula(int idAlumno, string nua)
        {
            InitializeComponent();
            alumnoID = idAlumno;
            nuaAlumno = nua;
        }

        private void FormReticula_Load(object sender, EventArgs e)
        {
            if (alumnoID <= 0)
            {
                MessageBox.Show("ID de alumno no válido.");
                return;
            }
            CargarReticulaHorizontal();
        }

        private void CargarReticula()
        {
            string query = "SELECT Semestre, NombreClase FROM Clases ORDER BY Semestre";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView2.DataSource = dt;

                // Opcional: Ajusta los encabezados
                dataGridView2.Columns["Semestre"].HeaderText = "Semestre";
                dataGridView2.Columns["NombreClase"].HeaderText = "Nombre de la Materia";
            }
        }
        private void CargarReticulaHorizontal()
        {
            string query = "SELECT Semestre, NombreClase FROM Clases WHERE Semestre IS NOT NULL ORDER BY Semestre, ClaseID";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = null;
                conn.Open();
                reader = cmd.ExecuteReader();

                Dictionary<int, List<string>> materiasPorSemestre = new Dictionary<int, List<string>>();

                while (reader.Read())
                {
                    int semestre = reader.GetInt32(0);
                    string materia = reader.GetString(1);

                    if (!materiasPorSemestre.ContainsKey(semestre))
                        materiasPorSemestre[semestre] = new List<string>();

                    materiasPorSemestre[semestre].Add(materia);
                }
                reader.Close();

                // Obtener el máximo número de materias en un semestre para saber cuántas filas se necesitan
                int maxMaterias = materiasPorSemestre.Values.Max(list => list.Count);

                // Limpiar y configurar el DataGridView
                dataGridView2.Columns.Clear();
                dataGridView2.Rows.Clear();
                dataGridView2.RowHeadersVisible = false;
                dataGridView2.AllowUserToAddRows = false;

                // Crear columnas por semestre
                foreach (var semestre in materiasPorSemestre.Keys.OrderBy(k => k))
                {
                    dataGridView2.Columns.Add($"Semestre{semestre}", $"Semestre {semestre}");
                }

                // Llenar las filas
                for (int i = 0; i < maxMaterias; i++)
                {
                    var row = new DataGridViewRow();
                    row.CreateCells(dataGridView2);

                    int colIndex = 0;
                    foreach (var semestre in materiasPorSemestre.Keys.OrderBy(k => k))
                    {
                        var materias = materiasPorSemestre[semestre];
                        if (i < materias.Count)
                            row.Cells[colIndex].Value = "📘 " + materias[i];
                        else
                            row.Cells[colIndex].Value = ""; // Celda vacía si no hay materia
                        colIndex++;
                    }

                    dataGridView2.Rows.Add(row);
                }
            }
        }

        private void CargarMateriasPorSemestre()
        {
            string connectionString = "Server=DESKTOP-8LL593G\\SQLEXPRESS;Database=RoqueSistema3;User Id=sa;Password=hola;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT Semestre, NombreClase FROM Clases WHERE Semestre IS NOT NULL ORDER BY Semestre, ClaseID";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                Dictionary<int, List<string>> materiasPorSemestre = new Dictionary<int, List<string>>();

                while (reader.Read())
                {
                    int semestre = reader.GetInt32(0);
                    string nombreMateria = reader.GetString(1);

                    if (!materiasPorSemestre.ContainsKey(semestre))
                    {
                        materiasPorSemestre[semestre] = new List<string>();
                    }

                    if (materiasPorSemestre[semestre].Count < 5)
                    {
                        materiasPorSemestre[semestre].Add(nombreMateria);
                    }
                }
                reader.Close();

                // Aquí agregas las materias a los GroupBox con espacio vertical:
                foreach (var kvp in materiasPorSemestre)
                {
                    int semestre = kvp.Key;
                    List<string> materias = kvp.Value;

                    // Busca el GroupBox por nombre (ajusta si tus GroupBox se llaman diferente)
                    GroupBox gb = this.Controls.Find($"groupBox{semestre}", true).FirstOrDefault() as GroupBox;
                    if (gb != null)
                    {
                        gb.Controls.Clear();

                        int y = 20;               // Posición vertical inicial
                        int espacioEntreLabels = 30; // Espacio entre etiquetas

                        foreach (var materia in materias)
                        {
                            Label lbl = new Label();
                            lbl.Text = "📘 " + materia;
                            lbl.AutoSize = true;
                            lbl.Location = new Point(10, y);
                            lbl.Font = new Font("Segoe UI", 10, FontStyle.Regular);

                            gb.Controls.Add(lbl);

                            y += espacioEntreLabels;
                        }
                    }
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
