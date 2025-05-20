using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RedEstudiantilRoque.Models;
using RedEstudiantilRoque.Views;

namespace RedEstudiantilRoque
{
    public partial class frmBuscar : Form, AlumnosView
    {
        private bool isSuccessful;
        private bool isEdit;
        private string message;
        public frmBuscar()
        {
            InitializeComponent();
            AssociateAndRaiseViewEvents();
            //tabControl1.TabPages.Remove();
        }

        private void AssociateAndRaiseViewEvents()
        {
            btnBuscar.Click += delegate { SearchEvent?.Invoke(this, EventArgs.Empty); };
            txtBuscarAlumno.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    SearchEvent?.Invoke(this, EventArgs.Empty);
                }
            };
        }

        //Propiedades
        public string Nombre
        {
            get { return txtNombre.Text; }
            set { txtNombre.Text = value }
        }
        public string Nua
        {
            get { return txtNua.Text; }
            set { txtNua.Text = value; }
        }
        public string Correo
        {
            get { return txtCorreo.Text; }
            set { txtCorreo.Text = value; }
        }
        public string SearchValue
        {
            get { return txtBuscarAlumno.Text; }
            set { txtBuscarAlumno.Text = value; }
        }
        public bool idEdit
        {
            get { return isEdit; }
            set { isEdit = value; }
        }

        public bool idSuccessful
        {
            get { return isSuccessful; }
            set { isSuccessful = value; }
        }

        public string Massage 
        {
          get { return message; }
          set { message = value; }
        }

        //Eventos
        public event EventHandler SearchEvent;
        public event EventHandler AddNewEvent;
        public event EventHandler EditEvent;
        public event EventHandler DeleteEvent;
        public event EventHandler SaveEvent;
        public event EventHandler CancelEvent;

        //Metodos
        public void SetAlumnoSetBindingSource(BindingSource alumnoList)
        {
            dataGridView1.DataSource = alumnoList;
        }
    }
}
