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
    public partial class FormRegistro : Form
    {
        public FormRegistro()
        {
            InitializeComponent();
            this.textBox5.PasswordChar = '*';

        }



       

        private void Registrar_Click(object sender, EventArgs e)
        {
            Comunicacion llamar = new Comunicacion();
            string user, pass;
            int registroRetorno;
            user = textBox1.Text;
            pass = textBox5.Text;
            
            

                if ((user.CompareTo(null) == 0) || pass.CompareTo(null) == 0)
                {
                    MessageBox.Show("Ingrese todos los campos");
                }
                else
                {
                    registroRetorno = llamar.Registrar(user, pass);
                    if (registroRetorno == 0)
                    {
                        MessageBox.Show("¡Te has registrado al chat del DIINF, Felicitaciones!" + registroRetorno );
                        this.Dispose();
                        FormCliente cliente = new FormCliente();
                        cliente.Show();
                    }
                    else{
                        MessageBox.Show("El username ya existe, intente con otro" );
                                         
                    }

                }
            }

        private void Volver_Click(object sender, EventArgs e)
        {

            FormCliente cliente = new FormCliente();
            cliente.Show();
            this.Hide();



        }

        
            }

        
    }





