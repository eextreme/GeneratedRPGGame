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
    class Monster
    {
        Rectangle hitBox;
        Texture2D monsterSpite;
        int health, defense, attack, movSpeed, knockResist;
        float hitRate, critRate;
        int posX, posY, xAniFrame = 0 , yAniFrame = 0;
        int xFrame, yFrame, numOfFrames;

        public Monster(Texture2D image, int hp, int def, int atk, int mov, float hitR, float critR, int knckRes, int numXFrames, int numYFrames)
        {
            monsterSpite = image;
            health = hp; defense = def; attack = atk; movSpeed = mov; knockResist = knckRes;
            hitRate = hitR; critRate = critR;
            
            xFrame = image.Width / numXFrames; yFrame = image.Height / numYFrames;
            numOfFrames = numXFrames;
        }

        public int getSpeed() { return movSpeed;}

        //Optional but later public in getYspeed() and getXspeed()

        public void spawnMonster(int x, int y) { posX = x; posY = y; }

        public void MoveWithBasicAI(int playerXpos, int playerYPos)
        {
            if (playerXpos > posX) {
                posX += movSpeed;
                if (xAniFrame < numOfFrames) { yAniFrame = 0; xAniFrame++; }
            }

            if (playerXpos < posX) { 
                posX -= movSpeed; 
                if (xAniFrame < numOfFrames) { yAniFrame = 0; xAniFrame++; }
            }

            if (playerYPos > posY) { posY += movSpeed; 
                if (xAniFrame < numOfFrames) { yAniFrame = 0; xAniFrame++; }
            }

            if (playerYPos < posY) { 
                posY -= movSpeed; 
                if (xAniFrame < numOfFrames) {yAniFrame = 0; xAniFrame++; }
            }

            if (xAniFrame>=numOfFrames)
                xAniFrame = 0;
        }

        public void Draw(SpriteBatch sprites)
        {

            
            sprites.Draw(monsterSpite, new Rectangle(posX, posY, xFrame, yFrame), getAnimation(xAniFrame, yAniFrame), Color.White);
        }

        private Rectangle getAnimation(int xF, int yF)
        {
            return new Rectangle(xF * xFrame, yF * xFrame, xFrame, yFrame);
        }

        public Rectangle getHitBox()
        {
            hitBox = new Rectangle(posX, posY, xFrame, yFrame);
            return hitBox;
        }

        public void knockBack (int x, int y)
        {
            posX = posX - x;
            posY = posY - x;
        }

    }
}
