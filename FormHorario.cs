using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;
using System.IO;

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
            dataGridView1.CellPainting += dataGridView1_CellPainting;

        }

        private void FormHorario_Load(object sender, EventArgs e)
        {
            if (alumnoID <= 0)
            {
                MessageBox.Show("ID de alumno no válido.");
                return;
            }
            CargarDatosHorarios();

            // Estilo general del formulario

            // General
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.BackgroundColor = Color.FromArgb(30, 30, 30);
            dataGridView1.GridColor = Color.LightGray;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Encabezado de columnas
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersHeight = 45;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(40, 27, 60); // Celeste moderno
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Celdas normales
            dataGridView1.DefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);
            dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
            dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 150, 180);
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;
            dataGridView1.RowTemplate.Height = 50;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.DefaultCellStyle.Padding = new Padding(15, 15, 15, 15); // Un poco de espacio

            // Alternancia de filas
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);

            // Quitar bordes de celdas
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.None;
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
            //FormAlumno frmAlum = new FormAlumno(nuaAlumno);
            //frmAlum.Show();
            //this.Hide();
        }

        private void roundedPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex >= 0)
            {
                e.PaintBackground(e.ClipBounds, true);
                using (GraphicsPath path = new GraphicsPath())
                {
                    Rectangle rect = e.CellBounds;
                    int radius = 10;
                    path.AddArc(rect.Left, rect.Top, radius, radius, 180, 90);
                    path.AddArc(rect.Right - radius, rect.Top, radius, radius, 270, 90);
                    path.AddLine(rect.Right, rect.Top + radius, rect.Right, rect.Bottom);
                    path.AddLine(rect.Right, rect.Bottom, rect.Left, rect.Bottom);
                    path.AddLine(rect.Left, rect.Bottom, rect.Left, rect.Top + radius);
                    e.Graphics.FillPath(new SolidBrush(Color.FromArgb(0, 188, 212)), path);
                    TextRenderer.DrawText(e.Graphics, e.FormattedValue?.ToString(), e.CellStyle.Font, rect, Color.White, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                    e.Handled = true;
                }
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            form.Show();
            this.Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            FormAlumno frmAlum = new FormAlumno(nuaAlumno);
            frmAlum.Show();
            this.Hide();
        }

        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "Excel Workbook|*.xlsx",
                Title = "Guardar horario como Excel"
            })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (XLWorkbook workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Horario");

                        // Agregar encabezados
                        for (int i = 0; i < dataGridView1.Columns.Count; i++)
                        {
                            worksheet.Cell(1, i + 1).Value = dataGridView1.Columns[i].HeaderText;
                            worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                            worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.LightSkyBlue;
                        }

                        // Agregar datos
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            for (int j = 0; j < dataGridView1.Columns.Count; j++)
                            {
                                worksheet.Cell(i + 2, j + 1).Value = dataGridView1.Rows[i].Cells[j].Value?.ToString();
                            }
                        }

                        // Autoajuste de columnas
                        worksheet.Columns().AdjustToContents();

                        // Guardar el archivo
                        workbook.SaveAs(sfd.FileName);
                        MessageBox.Show("Horario exportado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }
    }
}
