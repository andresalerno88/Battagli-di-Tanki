﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CombateMultiplayer
{
    class Tanque : ProtoSprite
    {

        TelaDeJogo Jogo;

        public Tanque(int x,int y,int direçao, TelaDeJogo j)
        {
            Jogo = j; 
            AreaDeContato = new Rectangle(x,y,82,90);
            Direçao = direçao;
            img = (Image)Properties.Resources.ResourceManager.GetObject("Tank");
            SpriteDesenho.Width = 82;
            SpriteDesenho.Height = 90;
            numQuadrosParado = 3;
            numQuadrosMovendo = 4;
        }
        public void Botoes(String dedadaDetectada)
        {
            if(dedadaDetectada == null){

                isMoving = false;
            }
            if (dedadaDetectada == Keys.Down.ToString())
            {
                isMoving = true;
                AreaDeContato.Y += 10;
                Direçao = 3;
            }
            if (dedadaDetectada == Keys.Up.ToString())
            {
                isMoving = true;
                AreaDeContato.Y -= 10;
                Direçao = 1;
            }
            if (dedadaDetectada == Keys.Right.ToString())
            {
                isMoving = true;
                AreaDeContato.X += 10;
                Direçao = 2;
            }
            if (dedadaDetectada == Keys.Left.ToString())
            {
                isMoving = true;
                AreaDeContato.X -= 10;
                Direçao = 0;
            }
            if (dedadaDetectada == Keys.A.ToString())
            {
                switch (Direçao)
                {
                    case 0:
                        Jogo.Sprites.Add(new Tirinho(AreaDeContato.X - 12, AreaDeContato.Y + 15, 0));
                        break;
                    case 1:
                        Jogo.Sprites.Add(new Tirinho(AreaDeContato.X + 15, AreaDeContato.Y -12, 1));
                        break;
                    case 2:
                        Jogo.Sprites.Add(new Tirinho(AreaDeContato.X + 52, AreaDeContato.Y + 15, 2));
                        break;
                    case 3:
                        Jogo.Sprites.Add(new Tirinho(AreaDeContato.X + 15, AreaDeContato.Y + 52, 3));
                        break;
                
                }
            }
        }
        /*
        public override void Draw(Graphics desenhista)
        {
            desenhista.DrawImage(img, AreaDeContato.X, AreaDeContato.Y);
        
        }
        */


    }
}
