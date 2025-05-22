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
using RedEstudiantilRoque.Data;
using System.Data.SqlClient;

namespace RedEstudiantilRoque
{
    public partial class frmBuscar : Form
    {
        int alumnoIDSeleccionado = -1;

        public frmBuscar()
        {
            InitializeComponent();
        }

        public void cargarAlumnos()
        {
            DataBaseManagmet db = new DataBaseManagmet();
            db.conectar();

            SqlConnection conn = db.obtenerConexion();

            try
            {
                string query = @"
                               SELECT a.AlumnoID, u.Nombre, u.Correo, a.Nua
                               FROM Alumnos a INNER JOIN Usuarios u ON (a.UsuarioID = u.UsuarioID)
                               WHERE TipoUsuario = 'Alumno'";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvAlumnos.DataSource = dt;
            }catch(Exception ex)
            {
                MessageBox.Show("Error en la carga de datos: " + ex.Message);
            }
            finally
            {
                db.conectar();
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string texto = txtBuscarAlumno.Text;
            DataBaseManagmet db = new DataBaseManagmet();
            db.conectar();

            SqlConnection conn = db.obtenerConexion();

            try
            {
                string query = @"
                               SELECT a.AlumnoID, u.Nombre, u.Correo, a.Nua
                               FROM Alumnos a INNER JOIN Usuarios u ON (a.UsuarioID = u.UsuarioID)
                               WHERE u.Nombre LIKE @Texto OR a.Nua LIKE @Texto";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Texto", "%" + texto + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvAlumnos.DataSource = dt;
            }catch (Exception ex)
            {
                MessageBox.Show("Error en busqueda: " + ex.Message);
            }
            finally
            {
                db.desconectar();
            }

        }

        private void frmBuscar_Load(object sender, EventArgs e)
        {
            cargarAlumnos();
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            if (dgvAlumnos.SelectedRows.Count > 0)
            {
                int alumnoID = Convert.ToInt32(dgvAlumnos.SelectedRows[0].Cells["AlumnoID"].Value);

                DialogResult result = MessageBox.Show("¿Deseas eliminar este alumno y su usuario asociado?",
                                                      "Confirmar eliminación",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    DataBaseManagmet db = new DataBaseManagmet();
                    db.conectar();
                    SqlConnection conn = db.obtenerConexion();

                    SqlTransaction trans = conn.BeginTransaction();

                    try
                    {
                        
                        int usuarioID = 0;
                        string selectQuery = "SELECT UsuarioID FROM Alumnos WHERE AlumnoID = @AlumnoID";
                        SqlCommand selectCmd = new SqlCommand(selectQuery, conn, trans);
                        selectCmd.Parameters.AddWithValue("@AlumnoID", alumnoID);

                        SqlDataReader reader = selectCmd.ExecuteReader();
                        if (reader.Read())
                        {
                            usuarioID = Convert.ToInt32(reader["UsuarioID"]);
                        }
                        reader.Close();

                        string deleteUsuario = "DELETE FROM Usuarios WHERE UsuarioID = @UsuarioID";
                        SqlCommand deleteUsuarioCmd = new SqlCommand(deleteUsuario, conn, trans);
                        deleteUsuarioCmd.Parameters.AddWithValue("@UsuarioID", usuarioID);
                        deleteUsuarioCmd.ExecuteNonQuery();

                        string deleteAlumno = "DELETE FROM Alumnos WHERE AlumnoID = @AlumnoID";
                        SqlCommand deleteAlumnoCmd = new SqlCommand(deleteAlumno, conn, trans);
                        deleteAlumnoCmd.Parameters.AddWithValue("@AlumnoID", alumnoID);
                        deleteAlumnoCmd.ExecuteNonQuery();

                        trans.Commit();
                        MessageBox.Show("Alumno y usuario eliminados exitosamente.");
                        cargarAlumnos(); 
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        MessageBox.Show("Error al eliminar: " + ex.Message);
                    }
                    finally
                    {
                        db.desconectar();
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecciona un alumno para eliminar.");
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvAlumnos.SelectedRows.Count > 0)
            {
                alumnoIDSeleccionado = Convert.ToInt32(dgvAlumnos.SelectedRows[0].Cells["AlumnoID"].Value);

                DataBaseManagmet db = new DataBaseManagmet();
                db.conectar();
                SqlConnection conn = db.obtenerConexion();

                try
                {
                    string query = @"SELECT A.Nua, U.Nombre, U.Correo 
                             FROM Alumnos A 
                             JOIN Usuarios U ON A.UsuarioID = U.UsuarioID 
                             WHERE A.AlumnoID = @AlumnoID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@AlumnoID", alumnoIDSeleccionado);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        txtNua.Text = reader["Nua"].ToString();
                        txtNombre.Text = reader["Nombre"].ToString();
                        txtCorreo.Text = reader["Correo"].ToString();
                    }
                    reader.Close();

                    tabControl1.SelectedTab = tabPage2;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar alumno: " + ex.Message);
                }
                finally
                {
                    db.desconectar();
                }
            }
            else
            {
                MessageBox.Show("Selecciona un alumno para editar.");
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {

            if (alumnoIDSeleccionado == -1)
            {
                MessageBox.Show("No hay alumno seleccionado.");
                return;
            }

            string nuevaNua = txtNua.Text;
            string nuevoNombre = txtNombre.Text;
            string nuevoCorreo = txtCorreo.Text;

            DataBaseManagmet db = new DataBaseManagmet();
            db.conectar();
            SqlConnection conn = db.obtenerConexion();

            SqlTransaction trans = conn.BeginTransaction();

            try
            {
                // Obtener el UsuarioID relacionado
                int usuarioID = 0;
                string getUsuarioID = "SELECT UsuarioID FROM Alumnos WHERE AlumnoID = @AlumnoID";
                SqlCommand getCmd = new SqlCommand(getUsuarioID, conn, trans);
                getCmd.Parameters.AddWithValue("@AlumnoID", alumnoIDSeleccionado);

                SqlDataReader reader = getCmd.ExecuteReader();
                if (reader.Read())
                {
                    usuarioID = Convert.ToInt32(reader["UsuarioID"]);
                }
                reader.Close();

                // Actualizar tabla Usuarios
                string updateUsuario = @"UPDATE Usuarios 
                                 SET Nombre = @Nombre, Correo = @Correo 
                                 WHERE UsuarioID = @UsuarioID";

                SqlCommand updateUsuarioCmd = new SqlCommand(updateUsuario, conn, trans);
                updateUsuarioCmd.Parameters.AddWithValue("@Nombre", nuevoNombre);
                updateUsuarioCmd.Parameters.AddWithValue("@Correo", nuevoCorreo);
                updateUsuarioCmd.Parameters.AddWithValue("@UsuarioID", usuarioID);
                updateUsuarioCmd.ExecuteNonQuery();

                // Actualizar tabla Alumnos
                string updateAlumno = @"UPDATE Alumnos 
                                SET Nua = @Nua 
                                WHERE AlumnoID = @AlumnoID";

                SqlCommand updateAlumnoCmd = new SqlCommand(updateAlumno, conn, trans);
                updateAlumnoCmd.Parameters.AddWithValue("@Nua", nuevaNua);
                updateAlumnoCmd.Parameters.AddWithValue("@AlumnoID", alumnoIDSeleccionado);
                updateAlumnoCmd.ExecuteNonQuery();

                trans.Commit();
                MessageBox.Show("Datos actualizados correctamente.");

                cargarAlumnos(); // Recargar DataGridView
                tabControl1.SelectedTab = tabPage1; // Regresar a la tabla
            }
            catch (Exception ex)
            {
                trans.Rollback();
                MessageBox.Show("Error al guardar cambios: " + ex.Message);
            }
            finally
            {
                db.desconectar();
            }
        }

        private void btnCerrarAll_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnRegresar_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}


