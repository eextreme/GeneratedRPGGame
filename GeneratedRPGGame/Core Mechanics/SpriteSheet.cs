using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GeneratedRPGGame.Core_Mechanics
{
    class SpriteSheet
    {
        Sprite[,] spriteList;
        int columns, rows;

        public SpriteSheet(Texture2D sSheet, int numOfColumns, int numOfRows, GraphicsDevice device)
        {
            spriteList = new Sprite[numOfColumns, numOfRows];
            columns = numOfColumns; rows = numOfRows;
            
            int tWidth = sSheet.Width, tHeight = sSheet.Height;
            int fWidth = tWidth / numOfColumns, fHeight = tHeight / numOfRows;
            
            Rectangle source;
            Texture2D sourceTexture;
            Color[] data;

            for (int i = 0; i < numOfColumns; i++)
            {
                for (int j = 0; j < numOfRows; j++)
                {
                    source = new Rectangle(i * fWidth, j * fHeight, fWidth, fHeight);

                    sourceTexture = new Texture2D(device, fWidth, fHeight);

                    data = new Color[source.Width * source.Height];

                    
                    sSheet.GetData(0, source, data, 0, data.Length);                                      
                    sourceTexture.SetData(data);

                    spriteList[i,j]=new Sprite(sourceTexture);
                }
            }
        }

        public Texture2D getSpriteAt(int x, int y) { return spriteList[x,y].sprite; }
        public Color[] getColorAt(int x, int y) { return spriteList[x,y].spriteColor; }
    }

    class Sprite
    {
        public Texture2D sprite { get; set; }
        public Color[] spriteColor { get; set; }

        public Sprite(Texture2D spr)
        {
            sprite = spr;
            spriteColor = new Color[spr.Width * spr.Height];
            sprite.GetData(spriteColor);
        }

    }
}
