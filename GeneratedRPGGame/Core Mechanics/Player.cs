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
        public int xFrames, yFrames, size;
        float posX, posY;
        public float speed;
        public int yRef, frame = 0, numOfFrames, health, mana, defence;
        
        //used to determin the center of the sprite
        float centerPosX, centerPosY;
        
        public Texture2D sprite { get; set; }
        SpriteSheet spriteSheet;
        Weapon weap;
        Vector2 curLoc { get; set; }
        Rectangle curAnimation { get; set;}
        String playerDir;
        
        public Player(Texture2D playerSprite, int x, int y, int hp, int mp, int def, float spd)
        {
            sprite = playerSprite;
            xFrames = playerSprite.Width / x;
            yFrames = playerSprite.Height / y;
            numOfFrames = y;

            size = (playerSprite.Width + playerSprite.Height) / 2;

            health = hp; mana = mp;
            centerPosX = 0; centerPosY = 0;
            speed = spd; defence = def;
        }

        public Player(SpriteSheet playerSpriteSheet, int hp, int mp, int def, float spd, int frames)
        {
            spriteSheet = playerSpriteSheet;
            health = hp; mana = mp; defence = def; speed = spd;
            numOfFrames = frames;
            centerPosX = 0; centerPosY = 0;            
        }

        public void equipWeapon(Weapon w) { weap = w;}
        
        public void setSpawnPoint (int x, int y) {posX = x; posY = y;}
        private KeyboardState getKey() {return Keyboard.GetState();}

        public void move()
        {
            float yMod = 0, xMod = 0;

            if (getKey().IsKeyDown(Keys.Down))
            { yRef = 0; frame++; yMod = speed; playerDir = "Down";  }

            if (getKey().IsKeyDown(Keys.Left))
            { yRef = 1; frame++; xMod = -speed; playerDir = "Left"; }

            if (getKey().IsKeyDown(Keys.Up))
            { yRef = 2; frame++; yMod = -speed; playerDir = "Up"; }

            if (getKey().IsKeyDown(Keys.Right))
            { yRef = 3; frame++; xMod = speed; playerDir = "Right"; }

            if (frame > numOfFrames) { frame = 0; }
           
            posX = posX + xMod; posY = posY + yMod;           
            
            curLoc = new Vector2(posX, posY);
            sprite = spriteSheet.getSpriteAt(frame,yRef);

            centerPosX = posX + sprite.Width/2; centerPosY = posY + sprite.Height/2;
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {            
            spriteBatch.Draw(this.sprite, this.curLoc, Color.White);            
        }

        public String getPlayerDir()
        {
            return playerDir;
        }

        public void takeDamage(int dmg, float x, float y)
        {
            posX = posX + x; posY = posY+y;
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

        public Rectangle getHitBox()
        {
            return new Rectangle((int) posX, (int) posY, xFrames, yFrames);
        }

        public Color[] getColor()
        {
            Color[] data = new Color[sprite.Width * sprite.Height];
            sprite.GetData<Color>(data);
            return data;
        }

        public Matrix getPlayerWorld()
        {
            return Matrix.CreateTranslation(new Vector3(playerCenter(), 0f));
        }
    }
}
