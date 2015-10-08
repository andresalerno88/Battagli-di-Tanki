using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombateMultiplayer
{
    class Obstaculo:ProtoSprite
    {
    TelaDeJogo Jogo;
        public Obstaculo(int x,int y,int largura, int altura, TelaDeJogo j)
        {
            Jogo = j; 
            AreaDeContato = new Rectangle(x,y,largura,altura);
            // img = (Image)Properties.Resources.ResourceManager.GetObject("Obstaculo");
        }
       
    }
}
