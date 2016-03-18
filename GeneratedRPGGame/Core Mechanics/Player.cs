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
        public int xFrames, yFrames;
        public int posX, posY, yRef, frame = 0, numOfFrames, health, mana;
        
        //used to determin the center of the sprite
        int centerPosX, centerPosY;
        
        Texture2D sprite { get; set; }
        Weapon weap;
        public Rectangle curLoc { get; set; }
        Rectangle curAnimation { get; set;}
        String playerDir;
        
        public Player(Texture2D playerSprite, int x, int y, int hp, int mp)
        {
            sprite = playerSprite;
            xFrames = playerSprite.Width / x;
            yFrames = playerSprite.Height / y;
            numOfFrames = y;
            health = hp; mana = mp;
            centerPosX = 0; centerPosY = 0;
        }

        public void equipWeapon(Weapon w) { weap = w;}
        
        public void setSpawnPoint (int x, int y) {posX = x; posY = y;}
        private KeyboardState getKey() { return Keyboard.GetState(); }

        public void move()
        {
            int yMod = 0, xMod = 0;

            if (getKey().IsKeyDown(Keys.Down))
            { yRef = 0; frame++; yMod = 10; playerDir = "Down";  }

            if (getKey().IsKeyDown(Keys.Left))
            { yRef = 1; frame++; xMod = -10; playerDir = "Left"; }

            if (getKey().IsKeyDown(Keys.Up))
            { yRef = 2; frame++; yMod = -10;  playerDir = "Up"; }

            if (getKey().IsKeyDown(Keys.Right))
            { yRef = 3; frame++; xMod = 10;  playerDir = "Right"; }

            if (frame > numOfFrames) { frame = 0; }
           
            posX = posX + xMod; posY = posY + yMod;
            centerPosX = posX + xFrames / 2; centerPosY = posY + yFrames / 2;
            
            curLoc = new Rectangle(posX, posY, xFrames, yFrames);
            curAnimation = new Rectangle(xFrames * frame, yRef * yFrames, xFrames, yFrames);
        }

        
        public void Draw(SpriteBatch batch)
        {            
            batch.Draw(this.sprite, this.curLoc, this.curAnimation, Color.White);            
        }

        public String getPlayerDir()
        {
            return playerDir;
        }

        public void takeDamage(int dmg, int x, int y)
        {
            posX = posX+ x; posY = posY+y;
            health -= dmg;
        }

        public Boolean isAlive()
        {
            return health > 0;
        }

        public Vector2 playerCenter()
        {
            return new Vector2(centerPosX, centerPosY);
        }
    }
}
