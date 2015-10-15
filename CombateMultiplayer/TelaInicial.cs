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
    public struct Jogador
    {
        public string Codenome;
        public string Nome;
        public string IP;

    }
    public partial class TelaInicial : Form
    {
        public const int BufferSize = 1024;

        Thread ThreadEnviadoraDeCodenome;
        Thread ThreadEscutadora;
        List<Jogador> Jogadores;


        private Socket socketUDP;

        delegate void AdicionaJogadorAListaDelegate(String msg);

        public TelaInicial()
        {
            Jogadores = new List<Jogador>();

            InitializeComponent();
            CriaNome();
        }

        void CriaNome()
        {
            string[] nomes = { "Ninja", "Guerreiro", "Fantasma", "Mercenário", "Gladiador", "Unicórnio", "Cozinheiro", "Andróide", "Exterminador", "Cavaleiro" };
            string[] adjetivos = { "Rubro", "Cinzento", "da Dor", "Destruidor", "Mortífero", "do Arco-Íris", "Polonês", "Voador", "Impiedoso", "Paspalhão" };
            Random brandom = new Random();
            textBox1.Text = nomes[brandom.Next(10)] + " " + adjetivos[brandom.Next(10)];


        }

        void AdicionaJogador(Jogador j)
        {
            if (Jogadores.Contains(j))
            {

            }
            else
            {
                Jogadores.Add(j);
                comboBox1.Items.Add(j.Codenome + " # " + j.Nome);
            }

        }

        private void TelaInicial_Load(object sender, EventArgs e)
        {
            // ThreadEnviadoraDeCodenome = new Thread(EnviaCodenome);
            socketUDP = new Socket(AddressFamily.InterNetwork,
                                         SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 20152);
            socketUDP.Bind(localEndPoint);

            ThreadEscutadora = new Thread(Escuta);
            ThreadEscutadora.Start();
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


                    ProcessData(strContent, ip);

                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            Escuta(new object());
        }




        void ProcessData(string cadeia, string ip)
        {
            int codigo, tamanho;
            codigo = int.Parse(cadeia[0].ToString() + cadeia[1].ToString());
            tamanho = int.Parse(cadeia[2].ToString() + cadeia[3].ToString() + cadeia[4].ToString());
            char[] msg = new char[tamanho - 5];
            cadeia.CopyTo(5, msg, 0, tamanho - 5);

            switch (codigo)
            {
                case 01:
                    {
                        RecebimentoMensagem01(new String(msg), ip);

                        break;
                    }
                case 02: {
                        RecebimentoMensagem02(new String(msg), ip);
                    break;
                }
                case 03:
                    {
                        RecebimentoMensagem03(new String(msg), ip);
                        break;
                    }

                default:
                    break;
            }




        }

        private void RecebimentoMensagem01(string cadeia, string ip)
        {

            string[] strings = cadeia.Split(new Char[] { '|' });
            Jogador j = new Jogador();
            j.Codenome = strings[0];
            j.Nome = strings[1];
            j.IP = ip;

            EnviaMsg02(ip);
            Invoke((MethodInvoker)delegate() { AdicionaJogador(j); });
        }

        private void RecebimentoMensagem02(string cadeia, string ip)
        {
            string[] strings = cadeia.Split(new Char[] { '|' });
            Jogador j = new Jogador();
            j.Codenome = strings[0];
            j.Nome = strings[1];
            j.IP = ip;

            Invoke((MethodInvoker)delegate() { AdicionaJogador(j); });
        }

        private void RecebimentoMensagem03(string cadeia, string ip)
        {
            Form f = new TelaConvite(cadeia);
            f.Show();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            EnviaMsg03(Jogadores[comboBox1.SelectedIndex].IP);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            EnviaMsg01();
        }

        private void EnviaMsg01()
        {
            byte[] buffer = new byte[BufferSize];
            EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Broadcast, 20152);

            Socket UDPEnvia = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            UDPEnvia.EnableBroadcast = true;
            //UDPEnvia.SetSocketOption(SocketOptionLevel.Socket,SocketOptionName.Broadcast,1);


            string msg = textBox1.Text + "|" + textBox2.Text;


            buffer = Encoding.ASCII.GetBytes("01" + string.Format("{0:000}", msg.Length + 5) + msg);
            UDPEnvia.SendTo(buffer, SocketFlags.None, remoteEndPoint);

        }


        private void EnviaMsg02(string ip)
        {
            byte[] mensage = new byte[BufferSize];
            EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse(ip), 20152);

            Socket UDPEnvia = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

           // UDPEnvia.EnableBroadcast = true;


            string msg = textBox1.Text + "|" + textBox2.Text;


            mensage = Encoding.ASCII.GetBytes("02" + string.Format("{0:000}", msg.Length + 5) + msg);
            UDPEnvia.SendTo(mensage, SocketFlags.None, remoteEndPoint);

        }


        private void EnviaMsg03(string ip)
        {
            byte[] mensage = new byte[BufferSize];
            EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse(ip), 20152);

            Socket UDPEnvia = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            string msg = textBox1.Text;


            mensage = Encoding.ASCII.GetBytes("03" + string.Format("{0:000}", msg.Length + 5) + msg);
            UDPEnvia.SendTo(mensage, SocketFlags.None, remoteEndPoint);

        }
    }

        public class StateObject
        {
            public Socket workSocket = null;        // socket do cliente
            public const int BufferSize = 1024;     // tamanho do buffer de leitura (recebimento)
            public byte[] buffer = new byte[BufferSize];    // buffer de leitura (recebimento)
        }


      
    
}
