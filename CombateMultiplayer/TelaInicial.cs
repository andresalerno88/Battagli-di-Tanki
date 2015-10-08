using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CombateMultiplayer
{
    public partial class TelaInicial : Form
    {
        public struct Jogador {
            public string Codenome;
            public string Nome;
            public string IP;
        
        }


        public const int BufferSize = 1024;

        Thread ThreadEnviadoraDeCodenome;
        Thread ThreadEscutadora;

        private Socket socketUDP;

        delegate void AdicionaJogadorAListaDelegate(String msg);

        public TelaInicial()
        {
            InitializeComponent();
        }

        void AdicionaJogador(String msg)
        {
            comboBox1.Items.Add(msg);
        }

        private void TelaInicial_Load(object sender, EventArgs e)
        {
            ThreadEnviadoraDeCodenome = new Thread(EnviaCodenome);
            socketUDP = new Socket(AddressFamily.InterNetwork,
                                         SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 20152);
            socketUDP.Bind(localEndPoint);

            ThreadEscutadora = new Thread(Escuta);

        }

        void Escuta(Object o)
        {
            byte[] buffer = new byte[BufferSize];
            EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0); ;

            // cria o objeto "state"
            StateObject state = new StateObject();
            state.workSocket = socketUDP;

            // Inicia recebimento de dados assíncrono
            socketUDP.BeginReceiveFrom(state.buffer,      // buffer
                                 0,                      // Posição inicial no buffer
                                 StateObject.BufferSize, // tamaho do buffer
                                 SocketFlags.None,       // Comportamento send e receive
                                 ref remoteEndPoint,
                                 new AsyncCallback(ServerReceiveFromCallback),
                                 state);



        }

        public void ServerReceiveFromCallback(IAsyncResult ar)
        {

            AdicionaJogadorAListaDelegate addJogador = new AdicionaJogadorAListaDelegate(AdicionaJogador);

            // Recupera o objeto "state" e o socket de manipulacao
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            try
            {
                // Lê dados do socket cliente. A operação assíncrona BeginReceiveFrom
                // deve, obrigatoriamente, ser completada chamando o método EndReceiveFrom
                EndPoint clientEP = new IPEndPoint(IPAddress.Any, 0);
                int bytesRead = handler.EndReceiveFrom(ar, ref clientEP);

                if (bytesRead > 0)
                {
                    // Obtém os dados recebidos
                    string strContent = Encoding.ASCII.GetString(state.buffer, 0, bytesRead);
                    string ip = IPAddress.Parse(((IPEndPoint)clientEP).Address.ToString()).ToString();

                    // mostra dados recebidos
                    String dados = string.Format("[{0} - {1}] {2}",
                                                 ip, ((IPEndPoint)clientEP).Port, strContent);
                    myForm.BeginInvoke(showMsg, new object[] { dados });
                }
            }
            catch (Exception e)
            {
            }
        }

        void EnviaCodenome(Object o)
        {




        }

     

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }




    }
        public class StateObject
        {
            public Socket workSocket = null;        // socket do cliente
            public const int BufferSize = 1024;     // tamanho do buffer de leitura (recebimento)
            public byte[] buffer = new byte[BufferSize];    // buffer de leitura (recebimento)
        }


      
    
}
