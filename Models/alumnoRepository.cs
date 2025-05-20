using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedEstudiantilRoque.Models
{
    public interface alumnoRepository
    {
        void Add(alumno alumno);
        void Edit(alumno alumno);
        void Delete(int Nua);
        IEnumerable<alumno> GetAll();
        IEnumerable<alumno> GetByValue(string value);
    }
}
