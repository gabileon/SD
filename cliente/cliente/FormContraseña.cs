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
    public partial class FormContraseña : Form
    {
        string nickname, password;

        public FormContraseña(string user, string pass)
        {
            
            nickname = user;
            password = pass;

            InitializeComponent();
        }

        private void cambiarContraseña_Click(object sender, EventArgs e)
        {

            string pass1, pass2;
            pass1 = textBox1.Text;
            pass2 = textBox2.Text;

            if (pass1.CompareTo(pass2) == 0)
            {

                Comunicacion llamar = new Comunicacion();
                int contraseña = llamar.cambiarContraseña(nickname, pass1, password);

                if (contraseña.CompareTo(1) == 0) {
                    
                    MessageBox.Show("¡Se cambió la clave exitosamente!");
                    
                    this.Hide();
                    FormPerfil perfil = new FormPerfil(nickname, pass1);
                    perfil.Show();     
                
                }
                else{
                    MessageBox.Show("¡No paso nada! + " + contraseña);
                
                }



            }
            else {

                MessageBox.Show("Las contraseñas no coiciden");
            
            }



        }


        
    }
}
