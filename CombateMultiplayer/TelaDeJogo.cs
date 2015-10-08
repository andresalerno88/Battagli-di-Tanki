using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CombateMultiplayer
{
    public partial class TelaDeJogo : Form
    {

        Bitmap areaDeDesenho;
        Graphics desenhista;
        Tanque Tanque1;
        Tanque Tanque2;
        int tanqueLocal;
        System.Windows.Forms.Timer clockUpdate;
        System.Windows.Forms.Timer clockRefresh;
        System.Windows.Forms.Timer clockAnimation;
        ServidorRede server;
        ClienteRede client;

        Thread t;
        String IP;
        public List<ProtoSprite> Sprites;


        const int DimensaoDaTelaX = 800, DimensaoDaTelaY = 600;

        public TelaDeJogo(int numeroDoTanque, string ip)
        {
            InitializeComponent();

            areaDeDesenho = new Bitmap(DimensaoDaTelaX, DimensaoDaTelaY); //Cria a área de desenho;
            pictureBox1.Image = areaDeDesenho; //Manda a região delimitada no Design do Visual mostrar nossa área de desenho;
            desenhista = Graphics.FromImage(areaDeDesenho); //Diz onde o desenhista desenha.        

            Sprites = new List<ProtoSprite>();

           Tanque1 = new Tanque(40, 275,  0, this);
            Tanque2 = new Tanque(710, 275, 2, this);

            Sprites.Add(Tanque1);
            Sprites.Add(Tanque2);

            for (int v = 0; v < 10; v++)
            {
                Sprites.Add(new Obstaculo( 350,50 * v, 50, 50, this));
            }

            IP = ip;

            tanqueLocal = numeroDoTanque;

            t = new Thread(CriaRede);
            t.Start();

            clockUpdate = new System.Windows.Forms.Timer();
            clockUpdate.Tick += Update;
            clockUpdate.Interval = 5;
            clockUpdate.Start();

            clockRefresh = new System.Windows.Forms.Timer();
            clockRefresh.Tick += Draw;
            clockRefresh.Interval = 4;
            clockRefresh.Start();

            clockAnimation = new System.Windows.Forms.Timer();
            clockAnimation.Tick += AnimateAll;
            clockAnimation.Interval = 80;
            clockAnimation.Start();


        }

        private void CriaRede(object a)
        {
            switch (tanqueLocal)
            {
                case 1:
                    server = new ServidorRede(Tanque2);
                    server.inicia();
                    break;
                case 2:
                    client = new ClienteRede(Tanque1, IP);
                    client.roda();
                    break;
            }
        }

        public void Update(object sender, EventArgs e)
        {
            UpdateObjects();

        }

        public void Draw(object sender, EventArgs e)
        {
            DrawObjects();

        }

        public void AnimateAll(object sender, EventArgs e)
        {
            AnimateObjects();

        }

        private void AnimateObjects()
        {
            foreach (ProtoSprite p in Sprites)
            {
                p.Animate();
            }
        }


        private void UpdateObjects()
        {
            foreach (ProtoSprite p in Sprites)
            {
                p.Update();
            }
        }

        private void DrawObjects()
        {
            desenhista.Clear(Color.SandyBrown);
            foreach (ProtoSprite p in Sprites)
            {
                p.Draw(desenhista);
            }
            pictureBox1.Image = areaDeDesenho;
        }

        private void detectarDedada(object sender, KeyEventArgs e)
        {
            switch (tanqueLocal)
            {
                case 1:
                    Tanque1.Botoes(e.KeyCode.ToString());
                    server.recebeTecla(e.KeyCode.ToString());
                    break;
                case 2:
                    Tanque2.Botoes(e.KeyCode.ToString());
                    client.recebeTecla(e.KeyCode.ToString());
                    break;
            }

        }

        private void TelaDeJogo_FormClosing(object sender, FormClosingEventArgs e)
        {

        }





    }
}
