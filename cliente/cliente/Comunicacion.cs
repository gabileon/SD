using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServicioChat = cliente.servicioChat;

namespace cliente
{
    class Comunicacion
    {
        // Atributo para hacer uso de los servivios
        static protected ServicioChat.Service1 Servicio = new servicioChat.Service1();


        public int login(String user, String pass, String ip)
        {
            // Método que permite utilzar el método login del WS
            try
            {
                return Servicio.login(user, pass, ip);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return -1;
            }
        }

        public int Registrar(String user, String pass)
        {
            return Servicio.registrar(user, pass);

            // Método que permite utilizar el Servicio "registrar", retornando de forma directa lo recibido
            // como retorno del WebService, con los parámetros capturados a través del Form "Registrar".
            // De fallar la operación, retorna un valor que pueda ser identificado por el cliente como tal.

        }

        public int desconectar(String user, String sala) {

            return Servicio.desconectar(user, sala);
      
        }

        public String entregaConectados()
        {
            // Método que permite utilizar el Servicio "entregaconect", retornando de forma directa lo
            // recibido como retorno del WebService, con los parámetros capturados a través del Form
            // "Principal". De fallar la operación, retorna un valor fijo, a pesar de que este no es 
            // controlado como un posible error a nivel de WebService.
            try
            {
                return Servicio.entregaConectados();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return "";
            }
        }

        // Método que permite utilizar el método "ip" del WS
        public string entregaIp(string user)
        {
            try
            {
                return Servicio.ip(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return "";
            }
        }


        public int cambiarContraseña(string user, string pass, string passvieja)
        {


            return Servicio.cambiarContraseña(user, pass, passvieja);


        }


        // Método que permite utilizar el método "puerto" del WS
        public int entregaPuerto(string user)
        {
            try
            {
                return Servicio.puerto(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return -1;
            }
        }

        public void enviarMsj(string nombre, string mensaje, string sala)
        {
            try
            {
                Servicio.enviarMensaje(nombre, mensaje, sala);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }

    }
}
