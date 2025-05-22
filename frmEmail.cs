using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;

namespace RedEstudiantilRoque
{
    public partial class frmEmail : Form
    {
        bool sidebarExpand;
        public frmEmail()
        {
            InitializeComponent();
        }

        private void sidebarTimer_Tick(object sender, EventArgs e)
        {
           // borar
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            sidebarTimer.Start();
        }

        private void txtMail_Enter(object sender, EventArgs e)
        {
            if(txtMail.Text == "    Email")
            {
                txtMail.Clear();
                txtMail.ForeColor = Color.Red;
            }
        }

        private void txtMail_LocationChanged(object sender, EventArgs e)
        {
            //este se borra
        }

        private void txtMail_Leave(object sender, EventArgs e)
        {
            if(txtMail.Text == "")
            {
                txtMail.ForeColor= Color.Red;
                txtMail.Text = "    Email";
            }
        }

        private void txtSub_Enter(object sender, EventArgs e)
        {
            if (txtSub.Text == "    Subject")
            {
                txtSub.Clear();
                txtSub.ForeColor = Color.Red;
            }
        }

        private void txtSub_Leave(object sender, EventArgs e)
        {
            if (txtSub.Text == "")
            {
                txtSub.ForeColor = Color.Red;
                txtSub.Text = "    Subject";
            }
        }

        private void txtMessage_Enter(object sender, EventArgs e)
        {
            if (txtMessage.Text == "    Message")
            {
                txtMessage.Clear();
                txtMessage.ForeColor = Color.Red;
            }
        }

        private void txtMessage_Leave(object sender, EventArgs e)
        {
            if (txtMessage.Text == "")
            {
                txtMessage.ForeColor = Color.Red;
                txtMessage.Text = "    Message";
            }
        }


        private void btnSend_Click(object sender, EventArgs e)
        {
            string to = txtMail.Text;
            string from = "soydaltonico34@gmail.com";
            string pass = "ypfw gauy eamp mysm";
            string messageBody = txtMessage.Text;

            MailMessage message = new MailMessage();
            message.To.Add(to);
            message.From = new MailAddress(from);
            message.Subject = txtSub.Text;
            message.Body = "<b>Mensaje:</b><br>" + messageBody;
            message.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(from, pass)
            };

            try
            {
                smtp.Send(message);
                MessageBox.Show("Correo enviado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMail.Clear();
                txtMessage.Clear();
                txtSub.Clear();
            }
            catch (SmtpException smtpEx)
            {
                MessageBox.Show("SMTP Error: " + smtpEx.StatusCode + "\n" + smtpEx.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error general: " + ex.Message + "\n" + ex.ToString());
            }
        }

        private void sidebarTimer_Tick_1(object sender, EventArgs e)
        {
            if (sidebarExpand)
            {
                sidebar.Width -= 10;
                if (sidebar.Width == sidebar.MinimumSize.Width)
                {
                    sidebarExpand = false;
                    sidebarTimer.Stop();
                }
            }
            else
            {
                sidebar.Width += 10;
                if (sidebar.Width == sidebar.MaximumSize.Width)
                {
                    sidebarExpand = true;
                    sidebarTimer.Stop();
                }
            }
        }
    }
}
