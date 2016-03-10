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

namespace GeneratedRPGGame
{
    class GenerateMap
    {
        tileSet tiles;
        int[][] mapTile;

        public GenerateMap(int x, int y, ContentManager content)
        {
            tiles = new tileSet("Practice", content.Load<Texture2D>("Tile Sets/part1_tileset"));
            Texture2D[,] test = new Texture2D[x, y];
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

        public void generatedMap(int x, int y)
        {

        }

        public void Draw(SpriteBatch batch)
        {
            Random any = new Random();
            
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++ )
                    batch.Draw(tiles.tileS, new Rectangle(32*i, 32*j, 32, 32), new Rectangle(mapTile[i][j]*32, 0, 32, 32), Color.White);
            }
        }

        public class tileSet
        {
            public Texture2D tileS;
            
            public tileSet(String name, Texture2D tileSet)
            {
                tileS = tileSet;
            }
        }
    }
}
