using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace RedEstudiantilRoque.Data
{
    internal class DataBaseManagmet
    {
        SqlConnection Cn;
        SqlCommand Command;
        SqlDataReader DR;

        public void conectar()
        {
            try
            {
                String cadena = "Data Source=DESKTOP-8LL593G\\SQLEXPRESS; User id=sa; Password=hola; Initial Catalog=SistemaRoque2; TrustServerCertificate=true";
                Cn = new SqlConnection(cadena);

                Cn.Open();
                MessageBox.Show("Conexion Exitosa");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en conexion " + ex.Message);
            }
        }

        public SqlConnection obtenerConexion()
        {
            return Cn;
        }

        public void desconectar()
        {
            try
            {
                Cn.Close();
                MessageBox.Show("DB desconectada");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en desconexion: " + ex.ToString());
            }
        }
    }
}

