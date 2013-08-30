using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Servidor
{
    public class Usuario
    {
        public String user;
        public String ip;
        public String pass;
        public int puerto;
      

        public Usuario()
        {
            this.user = null;
            this.ip = null;
            this.pass = null;
            this.puerto = -1;
          
        }

        public Usuario(String nick, String ip, String password, int puert)
        {
            this.user = nick;
            this.ip = ip;
            this.pass = password;
            this.puerto = puert;
          
        }
    }
}