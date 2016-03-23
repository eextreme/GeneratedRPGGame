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
        public int health, defense, attack, size;
        int movSpeedX, movSpeedY, knockResist;
        public int hitForce;
        float hitRate, critRate;
        float posX, posY;
        int xAniFrame = 0 , yAniFrame = 0, xFrame, yFrame, numOfFrames;
        float centerPosX, centerPosY;
        public Color[] color;

        public Monster(Texture2D image, int hp, int def, int atk, int mov, float hitR, float critR, int knckRes, int hitF, int numXFrames, int numYFrames) {
            monsterSpite = image;
            health = hp; defense = def; attack = atk; 
            knockResist = knckRes;
            hitForce = hitF;

            movSpeedX = mov; movSpeedY = mov;
            
            hitRate = hitR; critRate = critR;
            
            xFrame = image.Width / numXFrames; yFrame = image.Height / numYFrames;
            numOfFrames = numXFrames;

            size = (image.Width + image.Height) / 2;

            color = new Color[image.Width * image.Height];
            image.GetData(color);

        }

        //Optional but later public in getYspeed() and getXspeed()

        public void spawnMonster(int x, int y) { posX = x; posY = y; }

        public void MoveWithBasicAI(int playerXpos, int playerYPos) {
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

        public void Draw(SpriteBatch spriteBatch){
            spriteBatch.Draw(monsterSpite, new Vector2(posX, posY), getAnimation(xAniFrame, yAniFrame), Color.White);
        }

        private Rectangle getAnimation(int xF, int yF){
            return new Rectangle(xF * xFrame, yF * xFrame, xFrame, yFrame);
        }

        public Rectangle getHitBox(){
            hitBox = new Rectangle((int) posX, (int) posY, xFrame, yFrame);
            return hitBox;
        }

        public Boolean alive() {return health > 0;}
        
        public void takeDamage(int dmg, float x, float y) {
            posX += x; posY += y;           
            health -= dmg-defense; 
        }

        public Vector2 monsterCenter(){return new Vector2(centerPosX, centerPosY);}
        public Vector2 monsterSpeed(){return new Vector2(movSpeedX, movSpeedY);}

        public Monster genMonster(int killcount, Texture2D image)
        {
            int hpBuff = killcount/5 * 5 + 100 ;
            int defBuff = killcount/5 * 5 + 5;
            int movBuff = killcount /10 + 3;
            int atkBuff = killcount/5 * 5 + 5;
            int kresBuff = killcount/5 * 5 + 5;
            int hFBuff = killcount /5 * 5 + 20;

            return new Monster(image, hpBuff, defBuff, atkBuff, movBuff, 80, 20, kresBuff, hFBuff, 1, 1);
        }

    }
}
