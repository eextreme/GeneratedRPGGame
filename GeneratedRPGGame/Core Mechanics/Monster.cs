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
        Texture2D monsterSpite;
        public int health, defense, attack, size;
        int movSpeedX, movSpeedY, knockResist;
        public int hitForce;
        float hitRate, critRate;
        Vector2 position;
        int xAniFrame = 0 , yAniFrame = 0, xFrame, yFrame, numOfFrames;
        Vector2 centerPosition, monsterDir;
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

        public void spawnMonster(float x, float y) { position.X = x; position.Y = y; }

        public void MoveWithBasicAI(Vector2 target) {
            centerPosition = position + new Vector2(xFrame / 2, yFrame / 2);

            if (target.X > centerPosition.X) {
                monsterDir = Collision.east; position += movSpeedX * monsterDir;
                if (xAniFrame < numOfFrames) { yAniFrame = 0; xAniFrame++; }
            }

            if (target.X < centerPosition.X)
            {
                monsterDir = Collision.west; position += movSpeedX * monsterDir; 
                if (xAniFrame < numOfFrames) { yAniFrame = 0; xAniFrame++; }
            }

            if (target.Y > centerPosition.Y)
            {
                monsterDir = Collision.south; position += movSpeedY * monsterDir; 
                if (xAniFrame < numOfFrames) { yAniFrame = 0; xAniFrame++; }
            }

            if (target.Y < centerPosition.Y)
            {
                monsterDir = Collision.north; position += movSpeedY * monsterDir; 
                if (xAniFrame < numOfFrames) {yAniFrame = 0; xAniFrame++; }
            }

            if (xAniFrame>=numOfFrames)
                xAniFrame = 0;  
        }

        public void Draw(SpriteBatch spriteBatch){
            spriteBatch.Draw(monsterSpite, position, getAnimation(xAniFrame, yAniFrame), Color.White);
        }

        private Rectangle getAnimation(int xF, int yF){
            return new Rectangle(xF * xFrame, yF * xFrame, xFrame, yFrame);
        }

        public Boolean isAlive { get { return health > 0; } }
        
        public void takeDamage(int dmg, float x, float y) {
                        
            Vector2 change= position+new Vector2(x,y);

            position = Vector2.Lerp(position, change, 0.3f);

            int damage = dmg-defense;
            if (damage < 0) { damage = 0;}
            health -= damage;
        }

        public Vector2 monsterCenter { get { return centerPosition; } }
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
        public Vector2 getMonsterDir { get { return monsterDir; }}

    }
}
