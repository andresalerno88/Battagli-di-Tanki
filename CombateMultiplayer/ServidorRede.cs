using System;
using System.Text;

using System.Net;
using System.Net.Sockets;

namespace CombateMultiplayer
{
    class ServidorRede
    {
        private string TeclaPressionada;
        private const int BUFFER_SIZE = 1024;
        Tanque Tanque =null;
        int Port = 1138;

        TcpListener listener = null;

        TcpClient client = null;
        NetworkStream stream = null;

        bool rodando = true;

        public ServidorRede(Tanque tanque,int port)
        {
            Port = port;

            try
            {
                listener = new TcpListener(IPAddress.Any, Port);
                listener.Start();
            }
            catch (SocketException e)
            {
                Environment.Exit(e.ErrorCode);
            }

            Tanque = tanque;


            
       }

        public void inicia() {


            client = listener.AcceptTcpClient();


           recebeComandos();

            
        }

        void recebeComandos(){

            

                try
                {
                    stream = client.GetStream();
                    var totalBytesReceived = 0;

                    while (true)
                    {

                        var buffer = new byte[BUFFER_SIZE];
                        var bytesReceived = stream.Read(buffer, 0, BUFFER_SIZE);
                        Tanque.Botoes(Encoding.ASCII.GetString(buffer, totalBytesReceived, bytesReceived));

                        if (TeclaPressionada != null)
                        {
                            var bytesToSend = Encoding.ASCII.GetBytes(TeclaPressionada);

                            stream.Write(bytesToSend, 0, bytesToSend.Length);
                            TeclaPressionada = null;
                        }
                        else
                        {
                            stream.Write(new byte[2], totalBytesReceived, 2);
                        }

                        
                        //totalBytesReceived += bytesReceived;
                       // if (bytesReceived != BUFFER_SIZE) break;

                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                finally
                {
                    stream.Close();
                    client.Close();
                }
        
        

        }

        public void recebeTecla(string tecla)
        {
            TeclaPressionada = tecla;
        }
    }
}


