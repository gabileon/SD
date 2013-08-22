using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;
using cliente.servicioChat;
using System.Net;


namespace cliente
{
    public partial class FormCliente : Form
    {
        private TextBox nick;
        private TextBox password;

        public FormCliente()
        {
            InitializeComponent();
        }

        private void conectarBoton_Click(object sender, EventArgs e)
        {
            String user = nick.Text;
            String pass = password.Text;
            String ipLocal = "";
            int valorLogin = 0;
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            Comunicacion servicio = new Comunicacion();





        }



    }
}
