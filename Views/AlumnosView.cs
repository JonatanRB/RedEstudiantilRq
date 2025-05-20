using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedEstudiantilRoque.Views
{
    public interface AlumnosView
    {
        string Nombre { get; set; }
        string Nua {  get; set; }
        string Correo { get; set; }

        string SearchValue { get; set; }
        bool idEdit { get; set; }
        bool idSuccessful { get; set; }
        string Massage { get; set; }

        //Events
        event EventHandler SearchEvent;
        event EventHandler AddNewEvent;
        event EventHandler EditEvent;
        event EventHandler DeleteEvent;
        event EventHandler SaveEvent;
        event EventHandler CancelEvent;

        void SetAlumnoSetBindingSource(BindingSource alumnoList);
        void Show();
    }
}
