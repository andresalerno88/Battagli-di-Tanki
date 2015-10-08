using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombateMultiplayer
{
    class Tirinho:ProtoSprite
    {
        Image img = (Image)Properties.Resources.ResourceManager.GetObject("Tiro");
        int Velocidade = 11;
        public Tirinho(int x,int y,int direçao)
        {
            AreaDeContato = new Rectangle(x,y,10,10);
            Direçao = direçao;
          //img.RotateFlip(RotateFlipType.RotateNoneFlipX);
            

        }
        public override void Draw(Graphics desenhista)
        {
            desenhista.DrawImage(img, AreaDeContato.X, AreaDeContato.Y);

        }
        public override void Update()
        {
            switch (Direçao)
            {
                case 0:
                    AreaDeContato.X -= Velocidade;
                    break;
                case 1:
                    AreaDeContato.Y -= Velocidade;
                    break;
                case 2:
                    AreaDeContato.X += Velocidade;
                    break;
                case 3:
                    AreaDeContato.Y += Velocidade;
                    break;

            }
        }
    }
}
