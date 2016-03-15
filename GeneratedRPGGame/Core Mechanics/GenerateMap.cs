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
        TileSets tileSet;
        GraphicsDevice device;

        public GenerateMap(int x, int y, Texture2D tiles)
        {
            tileSet = new TileSets(tiles, 10, 10, device);
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

        }

        private class TileSets
        {
            List <Texture2D> tiles;
            
            public TileSets(Texture2D tileSource, int numXFrames, int numYFrames, GraphicsDevice device)
            {
                int fWidth = tileSource.Width / numXFrames;
                int fHeight = tileSource.Width / numYFrames;
                Rectangle source;
                Texture2D sourceTexture;
                Color[] data;

                for (int i = 0; i * fWidth < tileSource.Width; i++)
                {
                    for( int j =0; j*fHeight <tileSource.Height;j++)
                    {
                        source = new Rectangle(i*fWidth, j*fHeight, tileSource.Width-i*fWidth, tileSource.Height-j*fHeight);
                        sourceTexture = new Texture2D(device, source.Width,source.Height);
                        data = new Color[source.Width * source.Height];
                        
                        tileSource.GetData(0, source, data, 0, data.Length);
                        
                        sourceTexture.SetData(data);
                        
                        tiles.Add(sourceTexture);
                    }
                }
            }
        }
    }
}
