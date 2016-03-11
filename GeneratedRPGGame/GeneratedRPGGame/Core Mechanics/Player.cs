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
    class Player
    {
        int xFrames, yFrames;
        public int posX, posY, yRef, frame = 0, numOfFrames, aPosX, aPoxY;
        int wOffsetX, wOffsetY;
        float angle;
        Texture2D sprite { get; set; }
        Weapon weap;
        public Rectangle curLoc { get; set; }
        Rectangle curAnimation { get; set;}
        String playerDir;
        
        public Player(Texture2D playerSprite, int x, int y)
        {
            sprite = playerSprite;
            xFrames = playerSprite.Width / x;
            yFrames = playerSprite.Height / y;
            numOfFrames = y;
        }

        public void equipWeapon(Weapon w) { weap = w;}
        
        public void setSpawnPoint (int x, int y) {posX = x; posY = y;}
        private KeyboardState getKey() { return Keyboard.GetState(); }

        public void move()
        {
            int yMod = 0, xMod = 0;

            if (getKey().IsKeyDown(Keys.Down))
            { yRef = 0; frame++; yMod = 10; angle = MathHelper.Pi; wOffsetX = 0; wOffsetY = (int)weap.wepOffsetY; playerDir = "Down"; }

            if (getKey().IsKeyDown(Keys.Left))
            { yRef = 1; frame++; xMod = -10; angle = 3 * MathHelper.PiOver2; wOffsetX = (int)-weap.wepOffsetX; wOffsetY = 0; playerDir = "Left"; }

            if (getKey().IsKeyDown(Keys.Up))
            { yRef = 2; frame++; yMod = -10; angle = 0; wOffsetX = 0; wOffsetY = (int)-weap.wepOffsetY; playerDir = "Up"; }

            if (getKey().IsKeyDown(Keys.Right))
            { yRef = 3; frame++; xMod = 10; angle = MathHelper.PiOver2; wOffsetX = (int)weap.wepOffsetX; wOffsetY = 0; playerDir = "Right"; }

            if (frame > numOfFrames) { frame = 0; }
           
            posX = posX + xMod; posY = posY + yMod;
            curLoc = new Rectangle(posX, posY, xFrames, yFrames);
            curAnimation = new Rectangle(xFrames * frame, yRef * yFrames, xFrames, yFrames);
        }

        
        public void Draw(SpriteBatch batch)
        {
            aPosX = curLoc.Center.X + wOffsetX;
            aPoxY = curLoc.Center.Y + wOffsetY;
            
            if (getKey().IsKeyDown(Keys.Space))
                batch.Draw(weap.weaponSprite, new Vector2(aPosX, aPoxY), new Rectangle(0, 0, weap.weaponSprite.Width, weap.weaponSprite.Height), Color.White, (float)angle, new Vector2(0, 0), 0.3f, SpriteEffects.None, 1f);
            
            batch.Draw(this.sprite, this.curLoc, this.curAnimation, Color.White);            
        }

        public String getPosition()
        {
            return "X position is: " + posX + " Y position is: " + posY;
        }

        public String getPlayerDir()
        {
            return playerDir;
        }
    }
}
