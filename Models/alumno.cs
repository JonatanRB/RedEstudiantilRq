using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;


namespace RedEstudiantilRoque.Models
{
    public class alumno
    {
        private String Nombre;
        private String Correo;
        private String Nua;

        //Propíedades 
        [DisplayName("Alumno")]
        [Required(ErrorMessage = "El alumno necesita tener un nombre")]
        public string Nombre1 { get => Nombre; set => Nombre = value; }

        [DisplayName("Correo")]
        [Required(ErrorMessage = "El alumno debe tener un correo asignado")]
        public string Correo1 { get => Correo; set => Correo = value; }
        [DisplayName("Matricula")]
        [Required(ErrorMessage = "El alumno necesita un identificador")]
        public string Nua1 { get => Nua; set => Nua = value; }
    }
}
