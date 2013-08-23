using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.IO;

namespace Servidor
{
    /// <summary>
    /// Descripción breve de Service1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]

    public class Service1 : System.Web.Services.WebService
    {
        // Lista de usuarios conectados
        static protected List<Usuario> UsuariosConectados = new List<Usuario>();
        // Atributo para la asignación de puertos a los clientes
        static protected int puertosCliente = 11000;

        [WebMethod]
        // Método que realiza el login del usuario al servidor
        public int login(String user, String pass, String ip)
        {
            String linea, passxx;

            // Búsqueda de si el usuario está en línea
            for (int i = 0; i < UsuariosConectados.Count; i++)
            {
                // Lo está por lo que se retorna un 3, y no se pordrá conectar
                if (UsuariosConectados[i].user.CompareTo(user) == 0)
                {
                    return 3;
                }
            }
            // El usuario no está conectado
            try
            {
                // Se lee el archivo de texto que guarda los usuarios para comprobar la contraseña ingresada
                StreamReader sr = new StreamReader("C:/Users/Macarena/Documents/GitHub/Laboratorio_2_SD/usuarios.txt.txt");
                linea = sr.ReadLine();
                // Se lee el archivo línea por línea hasta que se encuentre el usuario que se busca
                while (linea != null)
                {
                    // Se compara la línea con el nombre de usuario
                    if (linea.CompareTo(user) == 0)
                    {
                        passxx = sr.ReadLine();
                        if (passxx.CompareTo(pass) == 0)
                        {
                            // La contraseña es igual a la ingresada por el usuario
                            Usuario usr = new Usuario(user, ip);
                            UsuariosConectados.Add(usr);
                            sr.Close();
                            return 1;
                        }
                        else
                        {
                            // Contraseña no es igual a la que eingresó el usuario
                            sr.Close();
                            return 2;
                        }
                    }
                    linea = sr.ReadLine();
                }
                sr.Close();
                return 0;
            }
            catch (Exception e)
            {
                // Hubo problemas en abrir el archivo
                Console.WriteLine(e.Message.ToString());
                return -1;
            }
        }
    }
}