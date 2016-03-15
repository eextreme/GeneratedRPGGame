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
    class TileSets
    {
        List<Texture2D> tiles = new List<Texture2D>();
        public int tWidth, tHeight, fWidth, fHeight, listSize;
        int hght, wdth;

        public TileSets(Texture2D tileSource, int numXFrames, int numYFrames, GraphicsDevice device)
        {
            tWidth = tileSource.Width; tHeight = tileSource.Height;
            fWidth = tWidth / numXFrames;
            fHeight = tHeight / numYFrames;
            Rectangle source;
            Texture2D sourceTexture;
            Color[] data;

            for (int i = 0; i < numXFrames; i++)
            {
                for (int j = 0; j  < numYFrames; j++)
                {
                    source = new Rectangle(i * fWidth, j * fHeight, fWidth, fHeight);
                    
                    sourceTexture = new Texture2D(device, fWidth, fHeight);
                    
                    data = new Color[source.Width * source.Height];
                    
                    tileSource.GetData(0, source, data, 0, data.Length);
                    sourceTexture.SetData(data);
                    
                    tiles.Add(sourceTexture);
                } 
            }

            listSize = tiles.Count();
            
            hght = tiles.ElementAt(0).Height;
            wdth = tiles.ElementAt(0).Width;
        }

        public Texture2D getTexture(int index)
        {

            
            return tiles.ElementAt(index);
        }

        public Color[] getTextureColors(int index)
        {
            Color[] data = new Color[hght*wdth];
            tiles.ElementAt(index).GetData<Color>(data);          
            return data;
        }
    }
}
