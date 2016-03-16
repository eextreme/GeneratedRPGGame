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
    class GenerateMap
    {
        Color[] mapColor, tileColor;
        Texture2D generatedMap;
        int[,] mapProperties;
        int xMax, yMax;
        
        public GenerateMap(int x, int y, TileSets set, GraphicsDevice device)
        {
            mapColor = new Color [x*y*set.fWidth*set.fHeight];
            Random rnd = new Random();
            int gen;

            for (int i=0;i<mapColor.Length;i++)
            {
                gen = rnd.Next(0,set.listSize);
                tileColor = set.getTextureColors(gen);
                for (int j = 0; j < tileColor.Length; j++)
                {
                    mapColor[i] = Color.Lerp(mapColor[i], tileColor[j], 1f);
                }
            }

            generatedMap = new Texture2D(device, x * set.fWidth, y * set.fHeight);

            generatedMap.SetData(mapColor);
        }

        public GenerateMap (int xMaxed, int yMaxed, TileSets set)
        {
            xMax = xMaxed; yMax = yMaxed;
            mapProperties = new int[xMax, yMax];
            for (int i=0; i < xMax; i++)
            {
                Random rnd = new Random();
                for (int j=0; j<yMax;j++)
                {
                    mapProperties[i, j] = rnd.Next(0, set.listSize);
                }
            }
        }

        public void smartMapGenerator()
        {
            //better map generator
        }

        public void Draw(SpriteBatch sprite)
        {
            sprite.Draw(generatedMap, new Rectangle(0, 0, 800, 800), Color.White);
        }

        public void DrawSeperate(SpriteBatch sprite, TileSets set)
        {
            Random rnd = new Random();
            int index = rnd.Next(0, set.listSize);

            for (int i=0; i<xMax;i+=set.fWidth)
            {
                for (int j=0; j<yMax;j+=set.fHeight)
                {
                    Rectangle loc = new Rectangle(i, j, set.fWidth, set.fHeight);
                    sprite.Draw(set.getTexture(mapProperties[i,j]),loc, Color.White);
                }
            }

        }

        /* Requires tile classes
         * - Left wall
         * - Right wall
         * - Top wall
         * - Bottome wall
         * - Right-Down Bend
         * - Right-Up Bend
         * - Left-Down Bend
         * - Left-Up Bend
         * - Door
         * - Lava/Pits/Spikes
         
         * * Required Algorithms
         * - Surround outer screen with a wall
         * - 
         * */
    }
}
