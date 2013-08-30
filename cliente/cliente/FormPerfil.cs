using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace cliente
{
    public partial class FormPerfil : Form
    {

        string nickname, password;

        public FormPerfil(string user, string pass)
        {
            InitializeComponent();

            nickname = user;
            password = pass;
            textBox1.Text = nickname;
            textBox1.Enabled = false;
            textBox2.Text = password;
            textBox2.Enabled = false;
        }



        private void volverMenu_Click(object sender, EventArgs e)
        {
            FormCliente cliente = new FormCliente();
            this.Hide();
            cliente.Show();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox1.Text = nickname;
            textBox2.Text = password;
            FormContraseña contraseña = new FormContraseña(nickname, password);
            contraseña.Show();
            this.Hide();

        }

        

        
    }
}
