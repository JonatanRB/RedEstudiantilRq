using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using RedEstudiantilRoque.Models;

namespace RedEstudiantilRoque._Repositories
{
    public class _alumnoRepository : BaseRepository, alumnoRepository
    {
        //Constructor
        public _alumnoRepository(string connectionString) { 
            this.connectionString = connectionString;
        }  
        //Methods
        public void Add(alumno alumno)
        {
            throw new NotImplementedException();
        }

        public void Delete(int Nua)
        {
            throw new NotImplementedException();
        }

        public void Edit(alumno alumno)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<alumno> GetAll()
        {
            var alumnoList = new List<alumno>();
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT U.Nombre, U.Correo, A.Nua\r\nFROM Usuarios U INNER JOIN Alumnos A ON (U.UsuarioID = A.UsuarioID);";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var alumnoModel = new alumno();
                        alumnoModel.Nua1 = reader[1].ToString();
                        alumnoModel.Nombre1 = reader[2].ToString();
                        alumnoModel.Correo1 = reader[3].ToString();
                        alumnoList.Add(alumnoModel);
                    }
                }
            }
            return alumnoList;
        }

        public IEnumerable<alumno> GetByValue(string value)
        {
            var alumnoList = new List<alumno>();
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"SELECT U.Nombre, U.Correo, A.Nua FROM Usuarios U INNER JOIN Alumnos A ON (U.UsuarioID = A.UsuarioID)" +
                    "WHERE A.Nua = @Nua OR U.Nombre LIKE @Nombre+'%';";
                command.Parameters.Add("@Nua", sqlDbType.varchar(50)).Value = Nua;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var alumnoModel = new alumno();
                        alumnoModel.Nua1 = reader[1].ToString();
                        alumnoModel.Nombre1 = reader[2].ToString();
                        alumnoModel.Correo1 = reader[3].ToString();
                        alumnoList.Add(alumnoModel);
                    }
                }
            }
            return alumnoList;
        }
    }
}

