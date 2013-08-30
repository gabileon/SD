using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

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
        private static Dictionary<string, Usuario> UsuariosConectados = new Dictionary<string, Usuario>();
        private static List<string> nombreUsuariosConectados = new List<string>();
        private static List<string> salaPrincipal = new List<string>();
        string fechaConexion;
        

        // Atributo para la asignación de puertos a los clientes
        static protected int puertosCliente = 11000;

        [WebMethod]
        // Método que realiza el login del usuario al servidor
        public int login(String user, String pass, String ip)
        {
            String linea, passxx;
            puertosCliente++;

            // Búsqueda de si el usuario está en línea
            for (int i = 0; i < UsuariosConectados.Count; i++)
            {
                // Lo está por lo que se retorna un 3, y no se pordrá conectar
                if (UsuariosConectados.ContainsKey(user))
                {
                    return 3;
                }
            }
            // El usuario no está conectado
            try
            {
                // Se lee el archivo de texto que guarda los usuarios para comprobar la contraseña ingresada
                StreamReader sr = new StreamReader("C:/Users/Gabriela/Documents/GitHub/SD/Usuarios.txt");
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
                            Usuario usr = new Usuario(user, ip, pass, puertosCliente);
                            UsuariosConectados.Add(user, usr);
                            nombreUsuariosConectados.Add(user);
                            salaPrincipal.Add(user);
                            // Se envia el mensaje de que se ha conectado el usuario
                            string mensajito = obtenerTimeStamp(DateTime.Now);
                            mensajito += " <El usuario " + user + " se ha conectado a la sala>: ";
                            bcNuevoMensaje(user, mensajito, "Principal");
                            

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

        [WebMethod]
        // Método que entrega la ip usuario
        public string ip(string user)
        {
            Usuario cli;
            UsuariosConectados.TryGetValue(user, out cli);
            return cli.ip;
        }

        [WebMethod]
        //Método que retorna el puerto del usuario
        public int puerto(string user)
        {
            Usuario cli;
            UsuariosConectados.TryGetValue(user, out cli);
            return cli.puerto;
        }

        [WebMethod]
        // Método que entrega los usuarios conectados al chat
        public String entregaConectados()
        {
            Usuario cli;
            String usuarioConec = "", aux1, aux2;
            // Siempre habrá mínimo un usuario contectado
            UsuariosConectados.TryGetValue(nombreUsuariosConectados[0], out cli);
            aux1 = cli.user;
            aux2 = cli.ip;
            usuarioConec = aux1 + ";" + aux2;
            // Se busca si hay más usuarios conectados
            for (int i = 1; i < UsuariosConectados.Count; i++)
            {
                UsuariosConectados.TryGetValue(nombreUsuariosConectados[i], out cli);
                usuarioConec = usuarioConec + ";" + cli.user + ";" + cli.ip;
            }
            return usuarioConec;
        }

        [WebMethod]
        public int registrar(string username, string password)
        {
            try
            {
                if (System.IO.File.Exists("C:/Users/Gabriela/Documents/GitHub/SD/Usuarios.txt"))
                {
                    StreamReader sr = new StreamReader("C:/Users/Gabriela/Documents/GitHub/SD/Usuarios.txt");
                    String line = sr.ReadLine();

                    if (username.CompareTo(line) == 0)
                    {
                        sr.Close();
                        return 1;
                    }
                    line = sr.ReadLine();
                    while (line != null)
                    {
                        if (username.CompareTo(line) == 0)
                        {
                            line = sr.ReadLine();
                                                    }
                        line = sr.ReadLine();
                    }
                    sr.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("EXCEPCIÓN DE TIPO:" + e.Message.ToString() + "----------");
                return -1;
            }
            FileStream stream = new FileStream("C:/Users/Gabriela/Documents/GitHub/SD/Usuarios.txt", FileMode.Append, FileAccess.Write);
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine(username);
            writer.WriteLine(password);
            writer.Close();
            return 0;


        }
        [WebMethod]

        public int cambiarContraseña(string user, string pass, string passvieja) {
        
            try
            {
                StreamReader sr = new StreamReader("C:/Users/Gabriela/Documents/GitHub/SD/Usuarios.txt");
                String line = sr.ReadLine();
                FileStream stream = new FileStream("C:/Users/Gabriela/Documents/GitHub/SD/UsuariosTemporal.txt", FileMode.Create, FileAccess.Write);
                StreamWriter writer = new StreamWriter(stream);

                while (line != null)
                {
                    //Copia todos los usuarios que sean distintos del que queremos cambiar 
                    //la contraseña

                    if (user.CompareTo(line) != 0)
                    {
                        writer.WriteLine(line);
                        line = sr.ReadLine();
                        writer.WriteLine(line);
                        line = sr.ReadLine();
                        writer.WriteLine(line);
                   }
                   //Cuando encuentra al usuario
                    else if (user.CompareTo(line) == 0)
                    {
                        writer.WriteLine(line);
                        line = sr.ReadLine();
                        //Si la pass antigua coincide

                        if(line.CompareTo(passvieja)==0)
                        {
                            //Se escribe la pass nueva que ingresó
                            writer.WriteLine(pass);
                            line = sr.ReadLine();
                            writer.WriteLine(line);
                        }else
                        {
                            sr.Close();
                            writer.Close();
                            return 2;
                        }
                    }
                    line = sr.ReadLine();
                }
                sr.Close();
                writer.Close();

                //Pasó todo lo que escribí del temporal al original

                StreamReader sr2 = new StreamReader("C:/Users/Gabriela/Documents/GitHub/SD/UsuariosTemporal.txt");
                String line2 = sr2.ReadLine();
                FileStream stream2 = new FileStream("C:/Users/Gabriela/Documents/GitHub/SD/Usuarios.txt", FileMode.Create, FileAccess.Write);
                StreamWriter writer2 = new StreamWriter(stream2);

                while (line2 != null)
                {
                    writer2.WriteLine(line2);
                    line2 = sr2.ReadLine();
                }

                sr2.Close();
                writer2.Close();

                //Luego que termina la copia, se elimina el archivo de texto temporal

                System.IO.File.Delete("C:/Users/Gabriela/Documents/GitHub/SD/UsuariosTemporal.txt");
                return 1;
            }
            catch (Exception e)
            {
                Console.WriteLine("Excepción: " + e.Message.ToString());
                return -1;
            }
        }
        

        [WebMethod]
        // Método para enviar un mensaje de una sala pública
        public void enviarMensaje(string user, string mensaje, string sala)
        {
            string mensajito = obtenerTimeStamp(DateTime.Now);
            mensajito += " <" + user + ">: " + mensaje;
            bcNuevoMensaje(user, mensajito, sala);

            //Para guardar Log, hay qe ver si funciona bien
           
          
        }
        [WebMethod]
        // Método para desconectarse
        public int desconectar(string user, string sala)
        {
            string time = obtenerTimeStamp(DateTime.Now);
            string mensajeAdios = time + " - " +user + " ha abandonado la conversación.";
            bcNuevoMensaje(user,mensajeAdios,sala);
            nombreUsuariosConectados.Remove(user);
            Usuario del;
            return 1;
            

        }
        public void generarLog(string user, string mensaje, string sala, string time ){

            //Debería registrar cuando incia la conversación pero no lo puedo obtener todavía, solo debería ser cuando entra a la sala

           // FileStream stream = new FileStream("C:/Users/Gabriela/Documents/GitHub/SD/"+tiempoinici+user+"_"+user2+".txt", FileMode.Append, FileAccess.Write);
           // StreamWriter writer = new StreamWriter(stream);
           // writer.WriteLine(time + "<"+user+">" + ": " + mensaje );
           // writer.Close();
            

        
        
        }


        //Obtiene el timestamp
        public string obtenerTimeStamp(DateTime valor)
        {
            return valor.ToString("<yyyy/MM/dd HH:mm:ss>");
        }

        //Broadcast de un mensaje nuevo
        public void bcNuevoMensaje(string user, string mensaje, string sala)
        {
            string time = obtenerTimeStamp(DateTime.Now);
            generarLog(user, mensaje, sala, time);   //Debería crearrse cuando inicia una conversación pero creo qe no está ese dato aún pq  no hay salas.
            mensaje = "avt´" + mensaje;
            // Si el mensaje es para la sala Principal
            if (sala.Equals("Principal"))
            {
                // Obtiene todos los usuarios de la sala para enviarle el mensaje
                foreach (string cliente in salaPrincipal)
                {
                    Usuario cli;
                    UsuariosConectados.TryGetValue(cliente, out cli);
                    enviar(mensaje, cli);
                }
            }
        }

        // Método que envía broadcast de actualización de usuario
        public void bcActClientes(string usuario, string mensaje, string sala)
        {
            // mensaje[0] indicará que hay que actualizar la ventana
            mensaje = "avt´" + mensaje;
            if (sala.Equals("Principal"))
            {
                foreach (string cliente in salaPrincipal)
                {
                    Usuario cli;
                    UsuariosConectados.TryGetValue(cliente, out cli);
                    if (cli.user.CompareTo(usuario) != 0)
                    {
                        mensaje += "´" + cli.user;
                        enviar(mensaje, cli);
                    }
                }
            }
        }

        //Función que envía un mensaje por sockets al usuario correspondiente
        public void enviar(string mensaje, Usuario user)
        {
            byte[] mens = Encoding.UTF8.GetBytes(mensaje);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(user.ip), user.puerto);
            socket.SendTo(mens, endpoint);
            socket.Close();
        }

    }
}
    
