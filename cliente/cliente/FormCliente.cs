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
        // Atributos de los TextBox que se utilizan para iniciar sesión
        private TextBox nickName;
        private TextBox password;
        //private Button botonRegistrar;
        private Button botonConectar;
        // Hebra para escuchar mensajes
        public Thread receiver;
        delegate void SetTextCallback();
        static List<Usuario> listaConectados = new List<Usuario>();
        string user;
        string miIp;
        int miPuerto = -1;
        string miSala;
     
        Comunicacion servicio = new Comunicacion();
        delegate void writeMsgCallback(string msg);
        
        //Sonidos
        System.Media.SoundPlayer startSoundPlayer = new System.Media.SoundPlayer(@"C:\Users\Gabriela\Documents\GitHub\SD\Servidor\login.wav");
        System.Media.SoundPlayer sonidoMensaje = new System.Media.SoundPlayer(@"C:\Users\Gabriela\Documents\GitHub\SD\Servidor\mensaje.wav");
        System.Media.SoundPlayer sonidoLogout = new System.Media.SoundPlayer(@"C:\Users\Gabriela\Documents\GitHub\SD\Servidor\logout.wav");


        // Constructor que inicializa la vista
        public FormCliente()
        {
            InitializeComponent();
            this.password.PasswordChar = '*';
            this.linkPerfil.Hide();
        }

        // Método que se ejecuta cuando el usuario quiere ingresar en la aplicación
        private void conectarBoton_Click(object sender, EventArgs e)
        {
            // Se obtiene la información del usuario ingresada
            user = this.nickName.Text;
            String pass = password.Text;
            // Atributos para obtener la ip local del usuario
            String ipLocal = "";
            int valorLogin = 0;
            int estado = 1;
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            // Se crea el objeto para acceder a los métodos del WS

            // Se realiza un búsqueda de la ip local que está conectado el cliente
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    ipLocal = ip.ToString();
                }
            }

            // Se verifica que se haya ingresado un usuario y una contraseña
            if ((user.CompareTo("") != 0) || (pass.CompareTo("") != 0))
            {
                try
                {
                    botonRegistrar.Enabled = false;
                    this.botonRegistrar.Enabled = false;
                    valorLogin = servicio.login(user, pass, ipLocal);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    valorLogin = -1;
                }

                // El usuario ha ingresado el nombre y contraseña correctamente
                if (valorLogin == 1)
                {
                    startSoundPlayer.Play();
                    // Solicitamos la ip y el puerto al servidor
                   
                    miIp = servicio.entregaIp(user);
                    miPuerto = servicio.entregaPuerto(user);
                    miSala = "Principal";
                    // Habilitamos y deshabilitamos lo que corresponda
                    MessageBox.Show("Se ha conectado exitosamente");
                    this.chat.Enabled = true;
                    this.botonConectar.Enabled = false;
                    this.desconectarBoton.Enabled = true;
                    this.nombre.Text = "¡Hola "+ user +"!";
                    this.mensaje.Enabled = true;
                    this.participantes.Enabled = true;
                    this.enviarBoton.Enabled = true;
                    this.ayudanteSalaBoton.Enabled = true;
                    this.salirBoton.Enabled = false;
                    //AGREGAR*****
                    this.linkPerfil.Show();
                    // Se muestran los usuarios conectados
                    String usuariosConectados = servicio.entregaConectados();
                    String[] split = usuariosConectados.Split(';');
                    this.participantes.Items.Clear();
                    listaConectados.Clear();
                    for (int i = 0; i < split.Length - 1; i = i + 2)
                    {
                        Usuario xx = new Usuario(split[i], split[i+1], 1);
                        listaConectados.Add(xx);
                        this.participantes.Items.Add(split[i]);
                    }

                    // Comienza a actuar la hebra
                    receiver = new Thread(funcionHiloEscucha);
                    receiver.IsBackground = true;
                    receiver.Start();
                }
                else if (valorLogin == 0)
                {
                    MessageBox.Show("Usuario incorrecto");
                }
                else if (valorLogin == 2)
                {
                    MessageBox.Show("Contraseña incorrecta");
                }
                else if (valorLogin == 3)
                {
                    MessageBox.Show("Usuario ya conectado");
                }
                if (valorLogin == -1)
                {
                    MessageBox.Show("Error: Intente más tarde");
                }
            }
            else
            {
                MessageBox.Show("Complete los campos de usuario y/o contraseña");
            }
        }

        public void packetReceive()
        {
            // Método que hace uso de métodos propios de la Clase Thread. Sleep puede recibir como parámetro
            // un entero que representa el tiempo que la Hebra quedará suspendida en milisegundos, en este
            // caso, se suspende la Hebra por dos segundos. Cada dos segundos se verifica si el Form asociado
            // a la variable "pantalla" ("Principal", como es definido en la línea 32) tiene el control o no, 
            // donde de tenerlo se hace una llamada al método Setboton().
            while (true)
            {
                Thread.Sleep(2000);
                if (chat.IsDisposed)
                {
                    this.Setboton();
                }
            }
        }

        private void Setboton()
        {
            // Método que se lleva a cabo cuando el Form "Principal" toma el control, esto es detectado por la
            // hebra y se lleva a cabo una función asíncrona que compara el ID de la hebra que hizo la llamada
            // (desde el método "clickBotonLogIn") con el ID de la hebra creada al presionar el botón que llama al
            // Form, los cuales de ser distintos generan un retorno de "true". Esto puede desarrollarse también 
            // con un parámetro como se observa en el método "SetTextpriv" del Form "Principal".
            if (this.botonConectar.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(Setboton);
                this.Invoke(d, new object[] { });
            }
            else
            {
                this.botonConectar.Enabled = true;
                this.botonConectar.Enabled = true;
                this.nickName.Text = "";
                this.contrasena.Text = "";
                this.receiver.Abort();
            }
        }
     
        private void botonRegistrar_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormRegistro registro = new FormRegistro();
            registro.Show();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            String user = this.nickName.Text;
            String pass = password.Text;
            FormPerfil perfil = new FormPerfil(user, pass);
            //this.Hide();
            perfil.Show();
        }

        private void enviarBoton_Click(object sender, EventArgs e)
        {
            this.enviarMensaje();
        }

        private void enviarMensaje()
        {
            string mensajito = this.mensaje.Text;
            if (!string.IsNullOrEmpty(mensajito))
            {
                servicio.enviarMsj(user, mensajito, miSala);
                this.mensaje.Clear();
            }
        }

        private void funcionHiloEscucha()
        {
            string mensaje = "";
            string[] separado;
            while (miPuerto == -1) { }
            while (true)
            {
                try
                {
                    
                    UdpClient clienteUDP = new UdpClient(miPuerto);
                    IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, miPuerto);
                    byte[] datos;
                    datos = clienteUDP.Receive(ref endpoint);
                    mensaje = Encoding.UTF8.GetString(datos, 0, datos.Length);
                    separado = mensaje.Split('´');

                    //Verificar tipo de string que viene en el separado[0], caso de ser
                    //una nueva conexión privada, verificar si se ha abierto o no ventanas de esas
                    //Actualiza ventana texto del form principal
                    if (separado[0].CompareTo("avt") == 0)
                    {
                        sonidoMensaje.Play();
                        this.writeMsg(separado[1]);
                        if (separado.Length > 2)
                        {
                            this.participantes.Items.Clear();
                            int saltarCero = -2;
                            foreach (string c in separado)
                            {
                                if (saltarCero == 0)
                                    this.participantes.Items.Add(c);
                                else saltarCero += 1;
                            }
                        }
                    }
                    clienteUDP.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
                mensaje = "";
            }
        }

        private void writeMsg(string msg)
        {
            if (this.chat.InvokeRequired)
            {
                writeMsgCallback d = new writeMsgCallback(writeMsg);
                this.Invoke(d, new object[] { msg });
            }
            else
            {
                this.chat.Text += msg + "\r\n";
                this.chat.SelectionStart = this.chat.Text.Length;
                this.chat.ScrollToCaret();
                this.chat.Refresh();
            }
        }

        private void desconectarBoton_Click(object sender, EventArgs e)
        {
            string user = nickName.Text;
            if (servicio.desconectar(user, miSala).CompareTo(1) == 0)
            {
                sonidoLogout.Play();
                MessageBox.Show("Te has desconectado del Chat del DIINF, nos vemos!");
               
                this.chat.Enabled = false;
                this.chat.Text = "";
                this.participantes.Items.Clear();

                this.botonConectar.Enabled = true;
                this.desconectarBoton.Enabled = false;
                this.mensaje.Enabled = false;
                this.participantes.Enabled = false;
                this.enviarBoton.Enabled = false;
                this.ayudanteSalaBoton.Enabled = false;
                this.nombre.Text = "Chat del DIINF";
                this.linkPerfil.Hide();
                this.salirBoton.Enabled = true;
                this.botonRegistrar.Enabled = true;
                this.mensaje.Text = "";
            }
        }

        private void salirBoton_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

  


        }
  }
