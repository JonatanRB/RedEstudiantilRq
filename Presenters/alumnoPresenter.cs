using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RedEstudiantilRoque.Models;
using RedEstudiantilRoque.Views;

namespace RedEstudiantilRoque.Presenters
{
    public class alumnoPresenter
    {
        //Fields
        private AlumnosView view;
        private alumnoRepository repository;
        private BindingSource alumnoBindigSource;
        private IEnumerable<alumno> alumnoList;

        //constructor
        public alumnoPresenter(AlumnosView view, alumnoRepository repository)
        {
            this.alumnoBindigSource = new BindingSource();
            this.view = view;
            this.repository = repository;
            this.view.SearchEvent += BuscarAlumno;
            this.view.EditEvent += AddNewAlumno;
            this.view.DeleteEvent += LoadSelectedAlumnoEdit;
            this.view.SaveEvent += SaveAlumno;
            this.view.CancelEvent += CancelAction;
            //Set alumnos binding source
            this.view.SetAlumnoSetBindingSource(alumnoBindigSource);
            //load alumno list view
            LoadALlAlumnoList();
            //show
            this.view.Show();
        }

        private void LoadALlAlumnoList()
        {
            alumnoList = repository.GetAll();
            alumnoBindigSource.DataSource = alumnoList;
        }

        private void BuscarAlumno(object sender, EventArgs e)
        {
            bool emptyValue = string.IsNullOrWhiteSpace(this.view.SearchValue);
            if (emptyValue == false)
            {
                alumnoList = repository.GetByValue(this.view.SearchValue);
                if(emptyValue == false)
                {
                    alumnoList = repository.GetByValue(this.view.SearchValue);
                }
                else
                {
                    alumnoList = repository.GetAll();
                    alumnoBindigSource.DataSource = alumnoList;
                }
            }
        }

        private void CancelAction(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void SaveAlumno(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void LoadSelectedAlumnoEdit(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void AddNewAlumno(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

    }
}
