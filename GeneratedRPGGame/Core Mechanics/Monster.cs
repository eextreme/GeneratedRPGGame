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
        public int health, defense, attack;
        int movSpeedX, movSpeedY, knockResist;
        public int hitForce;
        float hitRate, critRate;
        int posX, posY, xAniFrame = 0 , yAniFrame = 0;
        int xFrame, yFrame, numOfFrames;
        int centerPosX, centerPosY;

        public Monster(Texture2D image, int hp, int def, int atk, int mov, float hitR, float critR, int knckRes, int hitF, int numXFrames, int numYFrames)
        {
            monsterSpite = image;
            health = hp; defense = def; attack = atk; 
            knockResist = knckRes;
            hitForce = hitF;

            movSpeedX = mov; movSpeedY = mov;


            hitRate = hitR; critRate = critR;
            
            xFrame = image.Width / numXFrames; yFrame = image.Height / numYFrames;
            numOfFrames = numXFrames;
        }

        //Optional but later public in getYspeed() and getXspeed()

        public void spawnMonster(int x, int y) { posX = x; posY = y; }

        public void MoveWithBasicAI(int playerXpos, int playerYPos)
        {
            centerPosX = posX + xFrame / 2; centerPosY = posY + yFrame / 2;

            if (playerXpos > centerPosX) {
                posX += movSpeedX;
                if (xAniFrame < numOfFrames) { yAniFrame = 0; xAniFrame++; }
            }

            if (playerXpos < centerPosX)
            { 
                posX -= movSpeedX; 
                if (xAniFrame < numOfFrames) { yAniFrame = 0; xAniFrame++; }
            }

            if (playerYPos > centerPosY)
            { 
                posY += movSpeedY; 
                if (xAniFrame < numOfFrames) { yAniFrame = 0; xAniFrame++; }
            }

            if (playerYPos < centerPosY)
            { 
                posY -= movSpeedY; 
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

        public Boolean alive() {return health > 0;}
        public void takeDamage(int dmg, int x, int y) 
        {
            posX += x; posY += y;           
            health -= dmg; 
        }

        public Vector2 monsterCenter()
        {
            return new Vector2(centerPosX, centerPosY);
        }

        public Vector2 monsterSpeed()
        {
            return new Vector2(movSpeedX, movSpeedY);
        }

    }
}
