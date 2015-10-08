using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombateMultiplayer
{
    public class ProtoSprite 
    {
        protected Rectangle AreaDeContato; 
        protected Rectangle SpriteDesenho;
        /// <summary>
        /// 0 é esquerda; 1 pra cima; 2 direita; 3 pra baixo
        /// </summary>
        protected int Direçao; 
        protected int numQuadrosParado, numQuadrosMovendo;
        int QuadroAtual;
        protected bool isMoving=false;

        protected Image img = (Image)Properties.Resources.ResourceManager.GetObject("ImageNOTFOUND");

        public ProtoSprite(int x, int y,int largura,int altura ) {
            AreaDeContato = new Rectangle(x, y, largura, altura);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="largura"></param>
        /// <param name="altura"></param>
        /// <param name="dimX">Dimensão do Sprite em X</param>
        /// <param name="dimY">Dimensão do Sprite em y</param>
        /// <param name="numQP">Número de quadros da animção parada</param>
        /// <param name="numQM">Número de quadros da animção movendo</param>
        public ProtoSprite(int x, int y, int largura, int altura, int dimX, int dimY, int numQP,int numQM)
        {
            AreaDeContato = new Rectangle(x, y, largura, altura);
            SpriteDesenho = new Rectangle(0, 0, dimX, dimY);
            numQuadrosParado = numQP;
            numQuadrosMovendo = numQM;
        }


        public ProtoSprite()
        {
            AreaDeContato = new Rectangle(0, 0, 50, 050);
            SpriteDesenho = new Rectangle(0, 0, 50, 50);
        }

        public virtual void Draw(Graphics desenhista){
            
            SpriteDesenho.X = QuadroAtual * SpriteDesenho.Width;
            SpriteDesenho.Y = Direçao * SpriteDesenho.Height;
            desenhista.DrawImage(img,AreaDeContato,SpriteDesenho,GraphicsUnit.Pixel);
        
        }

        public virtual void Animate()
        {
            QuadroAtual++;
            if (!isMoving)
            {
                if (QuadroAtual >= numQuadrosParado)
                {
                    QuadroAtual = 0;
                }
            }
            else
            {
                if (QuadroAtual >= numQuadrosMovendo + numQuadrosParado)
                {
                    QuadroAtual = numQuadrosParado;
                }

            }
        }


        public virtual void Update()
        {
            
        }

    }
}
